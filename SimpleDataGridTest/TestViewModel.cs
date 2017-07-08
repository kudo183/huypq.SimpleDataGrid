using System;
using System.Collections.Generic;
using System.ComponentModel;
using SimpleDataGrid;
using SimpleDataGrid.ViewModel;

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

    public class TestViewModel : SimpleDataGrid.ViewModel.IEditableGridViewModel<Data>
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

            Entities = new ObservableCollectionEx<Data>(data);
            Entities.CollectionChanged += Entities_CollectionChanged;
            PagerViewModel = new SimpleDataGrid.ViewModel.PagerViewModel()
            {
                CurrentPageIndex = 1,
                PageCount = 1,
                IsEnablePaging = true,
                ItemCount = Entities.Count
            };
        }

        public Action<object> ActionSelectedValueChanged
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollectionEx<Data> Entities
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public List<HeaderFilterBaseModel> HeaderFilters
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValid
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SimpleCommand LoadCommand
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public PagerViewModel PagerViewModel
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public object ParentItem
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SimpleCommand SaveCommand
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Data SelectedItem
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SelectedValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedValuePath
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SysMsg
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object GetSelectedItem()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void LoadReferenceData()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
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
