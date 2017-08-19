using huypq.wpf.Utils;
using System;
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
        //UI binding
        object SelectedValue { get; set; }
        //UI binding
        string SelectedValuePath { get; set; }
        //UI binding
        string Msg { get; set; }
        //UI binding
        string SysMsg { get; set; }
        //UI binding
        PagerViewModel PagerViewModel { get; set; }
        //UI binding
        SimpleCommand LoadCommand { get; set; }
        //UI binding
        SimpleCommand SaveCommand { get; set; }
        //UI binding
        List<HeaderFilterBaseModel> HeaderFilters { get; set; }

        Action<object, string> ShowDialogAction { get; set; }
        void Load();
        void Save();
        void LoadReferenceData();
        object ParentItem { get; set; }
        object GetSelectedItem();
        
    }
}
