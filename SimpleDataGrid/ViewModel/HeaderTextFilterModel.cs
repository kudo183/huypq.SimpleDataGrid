namespace SimpleDataGrid.DataGridColumnHeaderFilterModel
{
    public class HeaderTextFilterModel : HeaderFilterBaseModel
    {
        public HeaderTextFilterModel(string name)
            : base(name, "TextFilter")
        {
        }

        private string _text = string.Empty;
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                {
                    return;
                }

                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public override object GetFilterValue()
        {
            if (IsUsed == false)
                return null;

            return Text;
        }
    }
}
