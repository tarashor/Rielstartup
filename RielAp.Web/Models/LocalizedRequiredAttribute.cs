using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        public LocalizedRequiredAttribute(string resourceIdPropertyName)
        {
            var property = typeof(Translation.Translation).GetProperty(resourceIdPropertyName);
            string propertyName = (string)property.GetValue(property);

            var mainMessagePropertyProperty = typeof(Translation.Translation).GetProperty("RequiredMessage");
            string mainMessage = (string)mainMessagePropertyProperty.GetValue(mainMessagePropertyProperty);


            ErrorMessage = string.Format(mainMessage, propertyName);
        }
    }
}