using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BenjaminMoore.Api.Retail.Pos.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToEnumMemberAttrValue(this Enum @enum)
        {
            var attribute = @enum.GetType().GetMember(@enum.ToString()).FirstOrDefault()?.
                    GetCustomAttributes(false).OfType<EnumMemberAttribute>().
                    FirstOrDefault();
            if (attribute == null)
                return @enum.ToString();
            return attribute.Value;
        }
    }
}
