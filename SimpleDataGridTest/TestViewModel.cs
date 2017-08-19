using System;
using System.Collections.Generic;
using System.ComponentModel;
using SimpleDataGrid;
using SimpleDataGrid.ViewModel;
using huypq.wpf.Utils;
using huypq.QueryBuilder;
using System.Linq;

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

        public List<ChildData> ChildDatas { get; set; }
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
                _data.Add(new Data() { ID = i, Date = now.AddDays(i), ChildDataID = i, ChildDatas = _childData });
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
    }
}
