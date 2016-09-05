namespace SimpleDataGrid.ViewModel
{
    public class HeaderDateFilterModel : HeaderFilterBaseModel
    {
        public HeaderDateFilterModel(string name)
            : base(name, "DateFilter")
        {
            FilterValue = System.DateTime.Now;
        }
    }
}
