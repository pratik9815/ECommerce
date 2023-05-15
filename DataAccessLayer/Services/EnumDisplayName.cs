using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    //This can get the enum without the display attribute
    //public static class EnumExtensions
    //{
    //    public static string GetEnumDisplayName(this Enum enumValue)
    //    {
    //        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
    //        var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();

    //        return displayAttribute?.GetName() ?? enumValue.ToString();
    //    }
    //    private static string GetName(this DisplayAttribute displayAttribute)
    //    {
    //        return displayAttribute?.Name ?? displayAttribute?.Description;
    //    }
    //}
    //This code needs the display attribute to not throw an exception       
    public static class EnumDisplayName
    {
        public static string GetEnumDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>().Name;
        }
    }

    //public static class EnumDisplayInteger
    //{
    //    public static int GetDisplayInteger(this Enum enumValue)
    //    {
    //        if (enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault() is MemberInfo memberInfo)
    //        {
    //            if (memberInfo.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
    //            {
    //                if (int.TryParse(displayAttribute.Name, out int intValue))
    //                {
    //                    return intValue;
    //                }
    //            }
    //        }
    //        return Convert.ToInt32(enumValue);
    //    }
    //}
}
