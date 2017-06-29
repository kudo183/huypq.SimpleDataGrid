using System;
using System.Collections.Generic;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderCheckFilterModel : HeaderFilterBaseModel
    {
        public HeaderCheckFilterModel(string name, string propertyName, Type propertyType)
            : base(name, "CheckFilter", propertyName, propertyType)
        {
            FilterValue = true;
        }

        protected override void InitPredicatesList()
        {
            Predicates = new List<string>();
            Predicates.Add("=");
            Predicates.Add("!=");
        }
    }
}
