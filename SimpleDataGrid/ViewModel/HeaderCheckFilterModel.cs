namespace SimpleDataGrid.ViewModel
{
    public class HeaderCheckFilterModel : HeaderFilterBaseModel
    {
        public HeaderCheckFilterModel(string name)
            : base(name, "CheckFilter")
        {
            FilterValue = true;
        }
    }
}
