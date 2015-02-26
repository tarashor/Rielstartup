using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RielAp.Web.Utils
{
    public class NumberDataTypeAttribute : ValidationAttribute
    {
        public NumberDataTypeAttribute(string resourceId)
        {
            var property = typeof(Translation.Translation).GetProperty(resourceId);
            ErrorMessage = (string)property.GetValue(property);
        }
        public override bool IsValid(object value)
        {
            return (value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal || value == null);
        }
    }
}