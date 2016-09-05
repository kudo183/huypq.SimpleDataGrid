using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleDataGrid.DataGridColumnHeaderFilterModel;
using SimpleDataGrid.ViewModel;

namespace SimpleDataGrid
{
    public class TestReferenceData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class TestData
    {
        public string Name { get; set; }
        public int TestReferenceDataID { get; set; }

        public TestReferenceData ReferenceData { get; set; }
        public List<TestReferenceData> TestReferenceDataList { get; set; }
    }

    public static class DesignTimeTestData
    {
        private static HeaderTextFilterModel _header_BaiXe;
        public static HeaderTextFilterModel Header_BaiXe
        {
            get
            {
                if (_header_BaiXe != null)
                {
                    return _header_BaiXe;
                }

                _header_BaiXe = new HeaderTextFilterModel("BaiXe");
                return _header_BaiXe;
            }
        }

        private static EditableGridViewModel<TestData> _viewModel;
        public static EditableGridViewModel<TestData> ViewModel
        {
            get
            {
                if (_viewModel != null)
                    return _viewModel;

                _viewModel = new EditableGridViewModel<TestData>();
                _viewModel.Entities = new ObservableCollection<TestData>(Entities);
                _viewModel.PagerViewModel = new PagerViewModel()
                                                {
                                                    CurrentPageIndex = 1,
                                                    ItemCount = 3,
                                                    PageCount = 5
                                                };
                return _viewModel;
            }
        }

        private static List<TestData> _entities;
        public static List<TestData> Entities
        {
            get
            {
                if (_entities != null)
                    return _entities;

                var referenceDataList = new List<TestReferenceData>()
                                            {
                                                new TestReferenceData() {ID = 1, Name = "Ref 1"},
                                                new TestReferenceData() {ID = 2, Name = "Ref 2"},
                                                new TestReferenceData() {ID = 3, Name = "Ref 3"},
                                                new TestReferenceData() {ID = 4, Name = "Ref 4"}
                                            };

                _entities = new List<TestData>()
                           {
                               new TestData()
                                   {
                                       Name = "name 1",
                                       TestReferenceDataID = 1,
                                       ReferenceData = new TestReferenceData() {ID = 1, Name = "Ref 1"},
                                       TestReferenceDataList = referenceDataList
                                   },
                               new TestData()
                                   {
                                       Name = "name 2",
                                       TestReferenceDataID = 2,
                                       ReferenceData = new TestReferenceData() {ID = 2, Name = "Ref 2"},
                                       TestReferenceDataList = referenceDataList
                                   },
                               new TestData()
                                   {
                                       Name = "name 3",
                                       TestReferenceDataID = 3,
                                       ReferenceData = new TestReferenceData() {ID = 3, Name = "Ref 3"},
                                       TestReferenceDataList = referenceDataList
                                   },
                               new TestData()
                                   {
                                       Name = "name 4",
                                       TestReferenceDataID = 4,
                                       ReferenceData = new TestReferenceData() {ID = 4, Name = "Ref 4"},
                                       TestReferenceDataList = referenceDataList
                                   }
                           };

                return _entities;
            }
        }
    }
}
