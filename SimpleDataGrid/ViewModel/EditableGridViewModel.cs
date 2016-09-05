using System.Collections.ObjectModel;

namespace SimpleDataGrid.ViewModel
{
    public class EditableGridViewModel<T>
    {
        public ObservableCollection<T> Entities { get; set; }
        public PagerViewModel PagerViewModel { get; set; }
    }
}
