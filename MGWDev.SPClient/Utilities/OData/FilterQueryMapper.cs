using PnP.Core.Model.SharePoint;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace MGWDev.SPClient.Utilities.OData
{
    public class FilterQueryMapper : ExpressionVisitor, IExpressionMapper
    {
        private readonly StringBuilder filterQuery;

        public FilterQueryMapper()
        {
            filterQuery = new StringBuilder();
        }

        public string BuildFilterQuery<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            CurrentType = typeof(T);
            Visit(predicate);
            return filterQuery.ToString();
        }

        protected Type CurrentType { get; set; }

        public bool IsPropertyOfTypeOrBase(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            Type currentType = CurrentType;
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

        protected override Expression VisitBinary(BinaryExpression node)
        {
            filterQuery.Append("(");

            Visit(node.Left);

            switch (node.NodeType)
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
                    throw new NotSupportedException($"Unsupported binary operator: {node.NodeType}");
            }

            Visit(node.Right);

            filterQuery.Append(")");

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            AppendConstantValue(node.Value);

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo property)
            {
                if (IsPropertyOfTypeOrBase(property))
                {
                    var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    filterQuery.Append(jsonPropertyNameAttribute?.Name ?? property.Name);
                }
                else if (node.Expression is not null)
                {
                    Visit(node.Expression);
                    filterQuery.Append("/");
                    var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    filterQuery.Append(jsonPropertyNameAttribute?.Name ?? property.Name);
                }
                else
                {
                    object value = property.GetValue(node.Member);
                    AppendObjectToQuery(value);

                }
            }
            else if (node.Member.MemberType == MemberTypes.Field && node.Expression is ConstantExpression constantExpression)
            {
                var fieldInfo = (FieldInfo)node.Member;
                var fieldValue = fieldInfo.GetValue(constantExpression.Value);

                AppendObjectToQuery(fieldValue);
            }

            return node;
        }

        private void AppendObjectToQuery(object? fieldValue)
        {
            if (fieldValue is DateTime dateTimeValue)
            {
                filterQuery.AppendFormat("'{0:yyyy-MM-ddTHH:mm:ss}'", dateTimeValue);
            }
            else
            {
                filterQuery.Append(fieldValue);
            }
        }

        protected virtual void AppendConstantValue(object value)
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
