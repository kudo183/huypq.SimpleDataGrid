using System;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderDateFilterModel : HeaderFilterBaseModel
    {
        public HeaderDateFilterModel(string name, string propertyName, Type propertyType)
            : base(name, "DateFilter", propertyName, propertyType)
        {
            FilterValue = System.DateTime.Now.Date;
        }
    }
}
