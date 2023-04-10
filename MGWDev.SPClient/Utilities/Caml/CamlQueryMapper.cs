using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MGWDev.SPClient.Utilities.Caml
{
    public class CamlQueryMapper : IExpressionMapper
    {
        public string BuildFilterQuery<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var filterExpression = predicate.Body;

            XElement query = new XElement("Where");
            query.Add(VisitExpression<T>(filterExpression));
            return query.ToString(SaveOptions.DisableFormatting);
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
        private XElement VisitExpression<T>(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                return VisitBinaryExpression<T>(binaryExpression);
            }
            else if (expression is MemberExpression memberExpression)
            {
                return VisitMemberExpression<T>(memberExpression);
            }
            else if (expression is ConstantExpression constantExpression)
            {
                return VisitConstantExpression(constantExpression);
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                return VisitExpression<T>(unaryExpression.Operand);
            }
            else if (expression is NewExpression newExpression)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException($"Unsupported expression type: {expression.GetType()}");
            }
        }

        protected virtual XElement VisitMemberExpression<T>(MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo property)
            {
                if (IsPropertyOfTypeOrBase<T>(property))
                {
                    var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    return new XElement("FieldRef", new XAttribute("Name", jsonPropertyNameAttribute?.Name ?? property.Name));
                }
                else if (memberExpression.Expression is not null && memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    //VisitExpression<T>(filterQuery, memberExpression.Expression);
                    //filterQuery.Append("/");
                    //var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    //filterQuery.Append(jsonPropertyNameAttribute?.Name ?? property.Name);
                }
                else
                {
                    LambdaExpression lambda = Expression.Lambda(memberExpression);
                    Delegate fn = lambda.Compile();
                    return VisitConstantExpression(Expression.Constant(fn.DynamicInvoke(null), memberExpression.Type));
                }
            }
            else if (memberExpression.Member.MemberType == MemberTypes.Field && memberExpression.Expression is ConstantExpression constantExpression)
            {
                LambdaExpression lambda = Expression.Lambda(memberExpression);
                Delegate fn = lambda.Compile();
                return VisitConstantExpression(Expression.Constant(fn.DynamicInvoke(null), memberExpression.Type));
            }

            return new XElement("FieldRef", new XAttribute("Name", "Test"));
        }
        protected virtual XElement ParseNodeType(ExpressionType type)
        {
            XElement node;

            switch (type)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    node = new XElement("And");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    node = new XElement("Or");
                    break;
                case ExpressionType.Equal:
                    node = new XElement("Eq");
                    break;
                case ExpressionType.GreaterThan:
                    node = new XElement("Gt");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    node = new XElement("Geq");
                    break;
                case ExpressionType.LessThan:
                    node = new XElement("Lt");
                    break;
                case ExpressionType.LessThanOrEqual:
                    node = new XElement("Leq");
                    break;
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", type));
            }

            return node;
        }
        protected virtual XElement VisitBinaryExpression<T>(BinaryExpression binaryExpression)
        {
            var left = VisitExpression<T>(binaryExpression.Left);
            var right = VisitExpression<T>(binaryExpression.Right);
            XElement node = ParseNodeType(binaryExpression.NodeType);
            if (left != null && right != null)
            {
                node.Add(left, right);
            }

            return node;
        }

        protected virtual XElement VisitConstantExpression(ConstantExpression constant)
        {
            return new XElement("Value", ParseValueType(constant.Type), constant.Value);
        }
        protected virtual XAttribute ParseValueType(Type type)
        {
            string name = "Text";
            if(type == typeof(DateTime))
            {
                name = "DateTime";
            }
            if(type == typeof(DateOnly))
            {
                name = "DateOnly";
            }
            if(type == typeof(int))
            {
                name = "Counter";
            }
            if(type == typeof(double))
            {
                name = "Decimal";
            }
            return new XAttribute("Type", name);
        }
    }
}
