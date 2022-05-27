using commercetools.Sdk.Api.Models.Types;

namespace Noodle_Assignment_API.Model
{
    public class CustomTypeModel
    {
        public CustomField CustomField { get; set; }
        public string Key { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }

    }

    public class CustomField
    {
        public string CustomFieldName { get; set; }
        public bool Required { get; set; }
        public string Label { get; set; }

    }
}
