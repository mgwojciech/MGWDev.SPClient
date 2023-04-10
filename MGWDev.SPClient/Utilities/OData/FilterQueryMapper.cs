using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MGWDev.SPClient.Utilities.OData
{
    public class FilterQueryMapper
    {
        public string BuildFilterQuery<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var filterExpression = predicate.Body;
            var filterQuery = new StringBuilder("");
            VisitExpression<T>(filterQuery, filterExpression);
            return filterQuery.ToString();
        }
        public static bool IsPropertyOfTypeOrBase<T>(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Type currentType = typeof(T);

            while (currentType != null)
            {
                if (property.DeclaringType == currentType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }
        private void VisitExpression<T>(StringBuilder filterQuery, Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                filterQuery.Append("(");
                VisitBinaryExpression<T>(filterQuery, binaryExpression);
                filterQuery.Append(")");
            }
            else if (expression is MemberExpression memberExpression)
            {
                VisitMemberExpression<T>(filterQuery, memberExpression);
            }
            else if (expression is ConstantExpression constantExpression)
            {
                AppendConstantValue(filterQuery, constantExpression.Value);
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                VisitExpression<T>(filterQuery, unaryExpression.Operand);
            }
            else
            {
                throw new NotSupportedException($"Unsupported expression type: {expression.GetType()}");
            }
        }

        protected virtual void VisitMemberExpression<T>(StringBuilder filterQuery, MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo property)
            {
                if (IsPropertyOfTypeOrBase<T>(property))
                {
                    var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    filterQuery.Append(jsonPropertyNameAttribute?.Name ?? property.Name);
                }
                else if (memberExpression.Expression is not null && memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    VisitExpression<T>(filterQuery, memberExpression.Expression);
                    filterQuery.Append("/");
                    var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    filterQuery.Append(jsonPropertyNameAttribute?.Name ?? property.Name);
                }
                else
                {
                    var lambda = Expression.Lambda(memberExpression);
                    var compiledValue = lambda.Compile().DynamicInvoke();
                    if (property.PropertyType == typeof(DateTime))
                    {
                        filterQuery.Append("'");
                        filterQuery.Append(((DateTime)compiledValue).ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
                        filterQuery.Append("'");
                    }
                    else
                    {
                        filterQuery.Append(compiledValue.ToString());
                    }
                }
            }
            else if (memberExpression.Member.MemberType == MemberTypes.Field && memberExpression.Expression is ConstantExpression constantExpression)
            {
                var fieldInfo = (FieldInfo)memberExpression.Member;
                var fieldValue = fieldInfo.GetValue(constantExpression.Value);

                if (fieldValue is DateTime dateTimeValue)
                {
                    filterQuery.AppendFormat("'{0:yyyy-MM-ddTHH:mm:ss}'", dateTimeValue);
                }
                else
                {
                    filterQuery.Append(fieldValue);
                }
            }
        }

        protected virtual void VisitBinaryExpression<T>(StringBuilder filterQuery, BinaryExpression binaryExpression)
        {
            VisitExpression<T>(filterQuery, binaryExpression.Left);

            switch (binaryExpression.NodeType)
            {
                case ExpressionType.Equal:
                    filterQuery.Append(" eq ");
                    break;
                case ExpressionType.NotEqual:
                    filterQuery.Append(" ne ");
                    break;
                case ExpressionType.LessThan:
                    filterQuery.Append(" lt ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    filterQuery.Append(" le ");
                    break;
                case ExpressionType.GreaterThan:
                    filterQuery.Append(" gt ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    filterQuery.Append(" ge ");
                    break;
                case ExpressionType.AndAlso:
                    filterQuery.Append(" and ");
                    break;
                case ExpressionType.OrElse:
                    filterQuery.Append(" or ");
                    break;
                default:
                    throw new NotSupportedException($"Unsupported binary operator: {binaryExpression.NodeType}");
            }

            VisitExpression<T>(filterQuery, binaryExpression.Right);
        }

        protected virtual void AppendConstantValue(StringBuilder filterQuery, object value)
        {
            if (value == null)
            {
                filterQuery.Append("null");
            }
            else if (value is string stringValue)
            {
                filterQuery.Append($"'{Uri.EscapeDataString(stringValue)}'");
            }
            else if (value is DateTime dateTimeValue)
            {
                filterQuery.Append($"datetime'{dateTimeValue:O}'");
            }
            else if (value is bool boolValue)
            {
                filterQuery.Append(boolValue.ToString().ToLowerInvariant());
            }
            else if (value is int || value is double)
            {
                filterQuery.Append(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                throw new NotSupportedException($"Unsupported constant value type: {value.GetType()}");
            }
        }
    }
}
