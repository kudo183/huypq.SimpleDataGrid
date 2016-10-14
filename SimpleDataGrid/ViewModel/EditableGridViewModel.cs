using System.Collections.Generic;

namespace SimpleDataGrid.ViewModel
{
    public interface EditableGridViewModel<T>
    {
        ObservableCollectionEx<T> Entities { get; set; }
        int SelectedIndex { get; set; }
        PagerViewModel PagerViewModel { get; set; }
        List<HeaderFilterBaseModel> HeaderFilters { get; set; }
    }
}
