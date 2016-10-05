using System;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderCheckFilterModel : HeaderFilterBaseModel
    {
        public HeaderCheckFilterModel(string name, string propertyName, Type propertyType)
            : base(name, "CheckFilter", propertyName, propertyType)
        {
            FilterValue = true;
        }
    }
}
