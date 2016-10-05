using System;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderTextFilterModel : HeaderFilterBaseModel
    {
        public HeaderTextFilterModel(string name, string propertyName, Type propertyType)
            : base(name, "TextFilter", propertyName, propertyType)
        {
        }
    }
}
