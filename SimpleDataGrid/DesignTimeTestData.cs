﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SimpleDataGrid.ViewModel;
using huypq.wpf.Utils;

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
        public DateTime Date { get; set; }
        public bool Check { get; set; }

        public TestReferenceData ReferenceData { get; set; }
        public List<TestReferenceData> TestReferenceDataList { get; set; }
    }

    public class TestViewModel : EditableGridViewModel<TestData>
    {
        
    }

    public static class DesignTimeTestData
    {
        private static HeaderTextFilterModel _testTextHeader;
        public static HeaderTextFilterModel TestTextHeader
        {
            get
            {
                if (_testTextHeader != null)
                    return _testTextHeader;

                _testTextHeader = new HeaderTextFilterModel("Name", "PropertyName", typeof(string));
                return _testTextHeader;
            }
        }

        private static HeaderDateFilterModel _testDateHeader;
        public static HeaderDateFilterModel TestDateHeader
        {
            get
            {
                if (_testDateHeader != null)
                    return _testDateHeader;

                _testDateHeader = new HeaderDateFilterModel("Date", "PropertyName", typeof(DateTime));
                return _testDateHeader;
            }
        }

        private static HeaderComboBoxFilterModel _testComboBoxHeader;
        public static HeaderComboBoxFilterModel TestComboBoxHeader
        {
            get
            {
                if (_testComboBoxHeader != null)
                    return _testComboBoxHeader;

                _testComboBoxHeader = new HeaderComboBoxFilterModel("Reference", HeaderComboBoxFilterModel.TextFilter, "PropertyName", typeof(string), "Ma", "Value");
                return _testComboBoxHeader;
            }
        }

        private static HeaderCheckFilterModel _testCheckHeader;
        public static HeaderCheckFilterModel TestCheckHeader
        {
            get
            {
                if (_testCheckHeader != null)
                    return _testCheckHeader;

                _testCheckHeader = new HeaderCheckFilterModel("Check", "PropertyName", typeof(bool));
                return _testCheckHeader;
            }
        }

        private static TestViewModel _viewModel;
        public static TestViewModel ViewModel
        {
            get
            {
                if (_viewModel != null)
                    return _viewModel;

                _viewModel = new TestViewModel();
                _viewModel.Entities = new ObservableCollectionEx<TestData>(Entities);
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
                                       Date = DateTime.Now,
                                       Check = true,
                                       TestReferenceDataID = 1,
                                       ReferenceData = new TestReferenceData() {ID = 1, Name = "Ref 1"},
                                       TestReferenceDataList = referenceDataList
                                   },
                               new TestData()
                                   {
                                       Name = "name 2",
                                       Check = false,
                                       Date = DateTime.Now.AddDays(1),
                                       TestReferenceDataID = 2,
                                       ReferenceData = new TestReferenceData() {ID = 2, Name = "Ref 2"},
                                       TestReferenceDataList = referenceDataList
                                   },
                               new TestData()
                                   {
                                       Name = "name 3",
                                       Check = false,
                                       Date = DateTime.Now.AddDays(2),
                                       TestReferenceDataID = 3,
                                       ReferenceData = new TestReferenceData() {ID = 3, Name = "Ref 3"},
                                       TestReferenceDataList = referenceDataList
                                   },
                               new TestData()
                                   {
                                       Name = "name 4",
                                       Check = true,
                                       Date = DateTime.Now.AddDays(3),
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
