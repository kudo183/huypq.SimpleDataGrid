using System.Collections.Generic;

namespace SimpleDataGridTest
{
    public class ChildData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Data
    {
        public int Id { get; set; }
        public int ChildDataId { get; set; }

        public ChildData ChildData { get; set; }
        public List<ChildData> ChildDatas { get; set; }
    }

    public class TestViewModel : SimpleDataGrid.ViewModel.EditableGridViewModel<Data>
    {
        private List<ChildData> _childData;
        public TestViewModel()
        {
            _childData = new List<ChildData>()
            {
                new ChildData() {Id=1, Name="Name 1" },
                new ChildData() {Id=2, Name="Name 2" },
                new ChildData() {Id=3, Name="Name 3" },
                new ChildData() {Id=4, Name="Name 4" },
                new ChildData() {Id=5, Name="Name 5" },
            };

            var data = new List<Data>()
            {
                new Data() {Id=1, ChildDataId=1, ChildDatas=_childData },
                new Data() {Id=2, ChildDataId=2, ChildDatas=_childData },
                new Data() {Id=3, ChildDataId=3, ChildDatas=_childData },
                new Data() {Id=4, ChildDataId=4, ChildDatas=_childData }
            };

            Entities = new System.Collections.ObjectModel.ObservableCollection<Data>(data);
            Entities.CollectionChanged += Entities_CollectionChanged;
            PagerViewModel = new SimpleDataGrid.ViewModel.PagerViewModel()
            {
                CurrentPageIndex = 1,
                PageCount = 1,
                IsEnablePaging = true,
                ItemCount = Entities.Count
            };
        }

        private void Entities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Data).ChildDatas = _childData;
            }
        }
    }
}
