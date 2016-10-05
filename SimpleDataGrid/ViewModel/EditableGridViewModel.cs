using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleDataGrid.ViewModel
{
    public interface EditableGridViewModel<T>
    {
        ObservableCollection<T> Entities { get; set; }
        PagerViewModel PagerViewModel { get; set; }
        List<HeaderFilterBaseModel> HeaderFilters { get; set; }
    }
}
