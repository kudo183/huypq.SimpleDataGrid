namespace SimpleDataGrid.ViewModel
{
    public interface IEditableGridViewModel<T>
    {
        ObservableCollectionEx<T> Entities { get; set; }
        object SelectedValue { get; set; }
        object SelectedItem { get; set; }        
        PagerViewModel PagerViewModel { get; set; }
        SimpleCommand LoadCommand { get; set; }
        SimpleCommand SaveCommand { get; set; }
    }
}
