using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Utilities.OData
{
    public class SelectQueryMapResult
    {
        public string SelectQuery { get; set; }
        public string ExpandQuery { get; set; }
    }
    public class SelectQueryMapper
    {
        public virtual SelectQueryMapResult MapToSelectQuery<T>()
        {
            List<string> selectProperties = new List<string>();
            List<string> expandProperties = new List<string>();
            var properties = GetSerializableProperties(typeof(T));
            var propertyCount = properties.Count;

            for (int i = 0; i < propertyCount; i++)
            {
                var property = properties[i];
                var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                var columnName = jsonPropertyNameAttribute != null
                    ? jsonPropertyNameAttribute.Name
                    : property.Name;
                if (IsPrimitive(property))
                {
                    selectProperties.Add(columnName);

                }
                else
                {
                    expandProperties.Add(columnName);
                    //it can work only one level down, so no need to implement any recursion here
                    var childProperties = GetSerializableProperties(property.PropertyType);
                    for (int j = 0; j < childProperties.Count; j++)
                    {
                        var childProperty = childProperties[j];
                        var jsonPropertyNameAttributeOfChild = childProperty.GetCustomAttribute<JsonPropertyNameAttribute>();
                        var childColumnName = jsonPropertyNameAttributeOfChild != null
                            ? jsonPropertyNameAttributeOfChild.Name
                            : childProperty.Name;
                        if (IsPrimitive(childProperty))
                        {
                            selectProperties.Add($"{columnName}/{childColumnName}");
                        }
                    }
                }
            }

            return new SelectQueryMapResult()
            {
                SelectQuery = String.Join(',', selectProperties),
                ExpandQuery = String.Join(',', expandProperties)
            };
        }

        private static bool IsPrimitive(PropertyInfo property)
        {
            return property.PropertyType.IsPrimitive || property.PropertyType.IsEnum || property.PropertyType.Namespace == "System";
        }

        private static List<PropertyInfo> GetSerializableProperties(Type type)
        {
            return type.GetProperties()
                .Where(p => !p.IsDefined(typeof(JsonIgnoreAttribute)) && p.CanRead)
                .ToList();
        }
    }
}
