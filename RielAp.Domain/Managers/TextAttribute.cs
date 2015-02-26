using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Managers
{
    public class LocalizationTextAttribute : Attribute
    {
        string text;

        string additionalText;
        bool hasAditionalText;

        public LocalizationTextAttribute(string resourceId, string additionalResourceId)
        {
            var property = typeof(Translation.Translation).GetProperty(resourceId);
            this.text = (string)property.GetValue(property);

            var additionalProperty = typeof(Translation.Translation).GetProperty(additionalResourceId);
            this.additionalText = (string)additionalProperty.GetValue(additionalProperty);
            hasAditionalText = true;
        }

        public LocalizationTextAttribute(string resourceId)
        {
            var property = typeof(Translation.Translation).GetProperty(resourceId);
            this.text = (string)property.GetValue(property);

            this.additionalText = null;
            hasAditionalText = false;
        }

        public string GetText()
        {
            return text;
        }

        public string GetAdditionalText()
        {
            if (hasAditionalText)
            {
                return additionalText;
            }
            else {
                return text;
            }
        }
    }
}
