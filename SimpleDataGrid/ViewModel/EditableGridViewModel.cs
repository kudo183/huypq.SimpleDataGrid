using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleDataGrid.ViewModel
{
    public interface IEditableGridViewModel<T> : IEditableGridViewModel where T : class
    {
        ObservableCollectionEx<T> Entities { get; set; }
        T SelectedItem { get; }
    }

    public interface IEditableGridViewModel : INotifyPropertyChanged
    {
        object SelectedValue { get; set; }
        string SelectedValuePath { get; set; }
        PagerViewModel PagerViewModel { get; set; }
        SimpleCommand LoadCommand { get; set; }
        SimpleCommand SaveCommand { get; set; }
        bool IsValid { get; set; }
        void Load();
        void Save();
        void LoadReferenceData();
        string SysMsg { get; set; }
        object ParentItem { get; set; }
        object GetSelectedItem();
        List<HeaderFilterBaseModel> HeaderFilters { get; set; }
        System.Action<object> ActionSelectedValueChanged { get; set; }
    }
}
