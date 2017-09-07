using System;
using System.Collections.Generic;
using System.ComponentModel;
using SimpleDataGrid;
using SimpleDataGrid.ViewModel;
using huypq.wpf.Utils;
using huypq.QueryBuilder;
using System.Linq;
using System.IO;

namespace SimpleDataGridTest
{
    public class ChildData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Data
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int ChildDataID { get; set; }
        public string ImagePath { get; set; }
        public Stream ImageStream { get; set; }

        public List<ChildData> ChildDatas { get; set; }

        public override string ToString()
        {
            return string.Format("ID {0} Date {1} ChildDataID {2} ImagePath {3}", ID, Date, ChildDataID, ImagePath);
        }
    }

    public class TestViewModel : EditableGridViewModel<Data>
    {
        List<ChildData> _childData;
        List<Data> _data;

        HeaderFilterBaseModel _idFilter;
        HeaderFilterBaseModel _dateFilter;
        HeaderFilterBaseModel _childIDFilter;

        public TestViewModel() : base()
        {
            SelectedValuePath = nameof(Data.ID);

            _childData = new List<ChildData>();
            for (var i = 1; i < 51; i++)
            {
                _childData.Add(new ChildData() { ID = i, Name = string.Format("child data {0}", i) });
            }

            _data = new List<Data>();
            var now = DateTime.Now.Date;
            for (var i = 1; i < 51; i++)
            {
                var data = new Data() { ID = i, Date = now.AddDays(i), ChildDataID = i, ChildDatas = _childData };
                if (i == 5)
                {
                    data.ImageStream = new MemoryStream(System.IO.File.ReadAllBytes(@"C:\Users\Public\Pictures\Sample Pictures\Tulipsx.jpg"));
                }
                else if (i == 2)
                {
                    data.ImagePath = @"C:\Users\Public\Pictures\Sample Pictures\Chrysanthemum.jpg";
                }
                _data.Add(data);
            }

            _idFilter = new HeaderTextFilterModel("ID", nameof(Data.ID), typeof(int));
            _dateFilter = new HeaderDateFilterModel("Date", nameof(Data.Date), typeof(DateTime));
            _childIDFilter = new HeaderComboBoxFilterModel(
                "Child ID", HeaderComboBoxFilterModel.ComboBoxFilter,
                nameof(Data.ChildDataID),
                typeof(int),
                nameof(ChildData.Name),
                nameof(ChildData.ID))
            {
                AddCommand = new SimpleCommand("ChildDataIDAddCommand", null),
                ItemSource = _childData
            };

            AddHeaderFilter(_idFilter);
            AddHeaderFilter(_dateFilter);
            AddHeaderFilter(_childIDFilter);
        }

        public override void Load()
        {
            var qe = new QueryExpression()
            {
                PageIndex = PagerViewModel.CurrentPageIndex,
                PageSize = PagerViewModel.PageSize,
                WhereOptions = WhereOptionsFromHeaderFilter(HeaderFilters),
                OrderOptions = OrderOptionsFromHeaderFilter(HeaderFilters)
            };

            var data = QueryExpression.AddQueryExpression(_data.AsQueryable(), ref qe, out int pageCount);

            Entities.Reset(data);

            PagerViewModel.ItemCount = Entities.Count;
            PagerViewModel.PageCount = pageCount;
            PagerViewModel.SetCurrentPageIndexWithoutAction(qe.PageIndex);

            SysMsg = "OK";
        }

        public override void Save()
        {
            foreach (var item in Entities)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
