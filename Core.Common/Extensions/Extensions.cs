using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Extensions
{
    public class Extensions
    {
        public static object GetExtensionDataMemberValue(IExtensibleDataObject extensibleObject, string dataMemberName)
        {
            object innerValue = null;

            PropertyInfo membersProperty = typeof(ExtensionDataObject).GetProperty("Members", BindingFlags.NonPublic | BindingFlags.Instance);

            IList members = (IList)membersProperty.GetValue(extensibleObject.ExtensionData, null);

            foreach (object member in members)
            {
                PropertyInfo nameProperty = member.GetType().GetProperty("Name");

                string name = (string)nameProperty.GetValue(member, null);

                if (name == dataMemberName)
                {
                    PropertyInfo valueProperty = member.GetType().GetProperty("Value");

                    object value = valueProperty.GetValue(member, null);

                    PropertyInfo innerValueProperty = value.GetType().GetProperty("Value");

                    innerValue = innerValueProperty.GetValue(value, null);

                    break;
                }
            }
            return innerValue;
        }
    }
}
