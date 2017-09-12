using huypq.QueryBuilder;
using huypq.wpf.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDataGrid.ViewModel
{
    public class EditableGridViewModel<T> : BindableObject, IEditableGridViewModel<T> where T : class
    {
        protected string _debugName;

        public object ParentItem { get; set; }

        protected readonly List<T> _originalEntities = new List<T>();

        ObservableCollectionEx<T> _entities;
        public ObservableCollectionEx<T> Entities
        {
            get { return _entities; }
            set { SetField(ref _entities, value); }
        }

        public T SelectedItem
        {
            get
            {
                if (_selectedValue == null)
                    return null;

                var p = typeof(T).GetProperty(_selectedValuePath);
                foreach (var entity in _entities)
                {
                    if (Equals(p.GetValue(entity), _selectedValue) == true)
                    {
                        return entity;
                    }
                }

                return null;
            }
        }

        object _selectedValue;
        public object SelectedValue
        {
            get { return _selectedValue; }
            set { SetField(ref _selectedValue, value); }
        }

        string _selectedValuePath;
        public string SelectedValuePath
        {
            get { return _selectedValuePath; }
            set { SetField(ref _selectedValuePath, value); }
        }

        public PagerViewModel PagerViewModel { get; set; }
        public SimpleCommand LoadCommand { get; set; }
        public SimpleCommand SaveCommand { get; set; }
        public Action<object, string> ShowDialogAction { get; set; }

        private string msg;
        public string Msg
        {
            get { return msg; }
            set { SetField(ref msg, value); }
        }

        private string sysMsg;
        public string SysMsg
        {
            get { return sysMsg; }
            set { SetField(ref sysMsg, string.Format("{0:hh:mm:ss.fff}  {1}", DateTime.Now, value)); }
        }

        public List<HeaderFilterBaseModel> HeaderFilters { get; set; }

        public EditableGridViewModel()
        {
            Entities = new ObservableCollectionEx<T>();
            Entities.CollectionChanged += Entities_CollectionChanged;
            HeaderFilters = new List<HeaderFilterBaseModel>();

            PagerViewModel = new PagerViewModel()
            {
                ActionCurrentPageIndexChanged = Load,
                ActionPageSizeChanged = Load
            };
        }

        void Entities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ProcessNewAddedDto(e.NewItems[0] as T);
            }
        }

        public object GetSelectedItem()
        {
            return SelectedItem;
        }

        public virtual void Load()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadReferenceData()
        {
            throw new NotImplementedException();
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }

        protected virtual void AddHeaderFilter(HeaderFilterBaseModel filter)
        {
            HeaderFilters.Add(filter);
            filter.ActionFilterValueChanged = Load;
            filter.ActionIsUsedChanged = Load;
            filter.ActionIsSortedChanged = Load;
            filter.ActionPredicateChanged = Load;
        }

        protected virtual void ProccessHeaderAddCommand(object content, string title, Action AfterCloseDialogAction)
        {
            ShowDialogAction?.Invoke(content, title);
            AfterCloseDialogAction?.Invoke();
        }

        protected virtual void ProcessDtoBeforeAddToEntities(T dto) { }

        protected virtual void ProcessNewAddedDto(T dto) { }

        protected virtual void BeforeLoad() { }

        protected virtual void AfterLoad() { }

        protected virtual void BeforeSave() { }

        protected virtual void AfterSave() { }

        protected List<WhereExpression.IWhereOption> WhereOptionsFromHeaderFilter(
            List<HeaderFilterBaseModel> headerFilters)
        {
            var result = new List<WhereExpression.IWhereOption>();

            foreach (var filter in headerFilters.Where(p => p.IsUsed == true))
            {
                var wo = WhereOptionFromHeaderFilter(filter);
                if (wo != null)
                {
                    result.Add(wo);
                }
            }

            return result;
        }

        protected WhereExpression.IWhereOption WhereOptionFromHeaderFilter(
            HeaderFilterBaseModel filter)
        {
            if (filter.FilterValue != null && filter.PropertyType == typeof(string))
            {
                var wo = new WhereExpression.WhereOptionString()
                {
                    PropertyPath = filter.PropertyName,
                    Value = (string)filter.FilterValue,
                    Predicate = filter.Predicate
                };
                return wo;
            }
            else if (filter.FilterValue != null && filter.PropertyType == typeof(int))
            {
                int number;
                if (int.TryParse(filter.FilterValue.ToString(), out number) == true)
                {
                    var wo = new WhereExpression.WhereOptionInt()
                    {
                        PropertyPath = filter.PropertyName,
                        Value = number,
                        Predicate = filter.Predicate
                    };
                    return wo;
                }
                else
                {
                    throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                }
            }
            else if (filter.PropertyType == typeof(int?))
            {
                var wo = new WhereExpression.WhereOptionNullableInt()
                {
                    PropertyPath = filter.PropertyName
                };
                if (filter.FilterValue == null)
                {
                    if (filter.Predicate == WhereExpression.Equal || filter.Predicate == WhereExpression.NotEqual)
                    {
                        wo.Predicate = filter.Predicate;
                        wo.Value = null;
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                    }
                }
                else
                {
                    int number;
                    if (int.TryParse(filter.FilterValue.ToString(), out number) == true)
                    {
                        wo.Predicate = filter.Predicate;
                        wo.Value = number;
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                    }
                }
                return wo;
            }
            else if (filter.FilterValue != null && filter.PropertyType == typeof(long))
            {
                long number;
                if (long.TryParse(filter.FilterValue.ToString(), out number) == true)
                {
                    var wo = new WhereExpression.WhereOptionLong()
                    {
                        PropertyPath = filter.PropertyName,
                        Value = number,
                        Predicate = filter.Predicate
                    };
                    return wo;
                }
                else
                {
                    throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                }
            }
            else if (filter.PropertyType == typeof(long?))
            {
                var wo = new WhereExpression.WhereOptionNullableLong()
                {
                    PropertyPath = filter.PropertyName
                };
                if (filter.FilterValue == null)
                {
                    if (filter.Predicate == WhereExpression.Equal || filter.Predicate == WhereExpression.NotEqual)
                    {
                        wo.Predicate = filter.Predicate;
                        wo.Value = null;
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                    }
                }
                else
                {
                    long number;
                    if (long.TryParse(filter.FilterValue.ToString(), out number) == true)
                    {
                        wo.Predicate = filter.Predicate;
                        wo.Value = number;
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                    }
                }
                return wo;
            }
            else if (filter.FilterValue != null && filter.PropertyType == typeof(bool))
            {
                if (filter.Predicate == WhereExpression.Equal || filter.Predicate == WhereExpression.NotEqual)
                {
                    var wo = new WhereExpression.WhereOptionBool()
                    {
                        Predicate = filter.Predicate,
                        PropertyPath = filter.PropertyName,
                        Value = (bool)filter.FilterValue
                    };
                    return wo;
                }
                else
                {
                    throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                }
            }
            else if (filter.PropertyType == typeof(bool?))
            {
                if (filter.Predicate == WhereExpression.Equal || filter.Predicate == WhereExpression.NotEqual)
                {
                    var wo = new WhereExpression.WhereOptionNullableBool()
                    {
                        Predicate = filter.Predicate,
                        PropertyPath = filter.PropertyName,
                        Value = (bool?)filter.FilterValue
                    };
                    return wo;
                }
                else
                {
                    throw new ArgumentException(string.Format("filter: {0} not valid", filter.Name));
                }
            }
            else if (filter.FilterValue != null && filter.PropertyType == typeof(DateTime))
            {
                var wo = new WhereExpression.WhereOptionDate()
                {
                    Predicate = filter.Predicate,
                    PropertyPath = filter.PropertyName,
                    Value = (DateTime)filter.FilterValue
                };
                return wo;
            }
            else if (filter.PropertyType == typeof(DateTime?))
            {
                var wo = new WhereExpression.WhereOptionNullableDate()
                {
                    Predicate = filter.Predicate,
                    PropertyPath = filter.PropertyName,
                    Value = (DateTime?)filter.FilterValue
                };
                return wo;
            }
            else if (filter.FilterValue != null && filter.PropertyType == typeof(TimeSpan))
            {
                var wo = new WhereExpression.WhereOptionTime()
                {
                    Predicate = filter.Predicate,
                    PropertyPath = filter.PropertyName,
                    Value = (TimeSpan)filter.FilterValue
                };
                return wo;
            }
            else if (filter.PropertyType == typeof(TimeSpan?))
            {
                var wo = new WhereExpression.WhereOptionNullableTime()
                {
                    Predicate = filter.Predicate,
                    PropertyPath = filter.PropertyName,
                    Value = (TimeSpan?)filter.FilterValue
                };
                return wo;
            }

            return null;
        }

        protected List<OrderByExpression.OrderOption> OrderOptionsFromHeaderFilter(
            List<HeaderFilterBaseModel> headerFilters)
        {
            var result = new List<OrderByExpression.OrderOption>();

            foreach (var filter in headerFilters)
            {
                switch (filter.IsSorted)
                {
                    case HeaderFilterBaseModel.SortDirection.Ascending:
                        result.Add(new OrderByExpression.OrderOption()
                        {
                            PropertyPath = filter.SortPropertyName,
                            IsAscending = true
                        });
                        break;
                    case HeaderFilterBaseModel.SortDirection.Descending:
                        result.Add(new OrderByExpression.OrderOption()
                        {
                            PropertyPath = filter.SortPropertyName,
                            IsAscending = false
                        });
                        break;
                }
            }

            return result;
        }
    }
}
