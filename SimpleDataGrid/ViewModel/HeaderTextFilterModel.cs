using System;
using System.Collections.Generic;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderTextFilterModel : HeaderFilterBaseModel
    {
        public HeaderTextFilterModel(string name, string propertyName, Type propertyType)
            : base(name, "TextFilter", propertyName, propertyType)
        {
        }

        protected override void InitPredicatesList()
        {
            if (PropertyType == typeof(string))
            {
                Predicates = new List<string>();
                Predicates.Add("=");
                Predicates.Add("(");
                Predicates.Add("*");
                Predicates.Add("!=");
                Predicates.Add("!(");
                Predicates.Add("!*");
            }
            else
            {
                base.InitPredicatesList();
            }
        }
    }
}
