using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SimpleDataGrid.ViewModel;
using System.Collections;

namespace SimpleDataGrid
{
    public class DataGridExt : DataGrid
    {
        public readonly List<int> SkippedColumnIndex = new List<int>();

        public bool SkippedSelectionChangedEvent = false;

        public DataGridExt()
        {
            RowBackground = new SolidColorBrush(Colors.LightBlue);
            AlternatingRowBackground = new SolidColorBrush(Colors.LightGoldenrodYellow);
            AlternationCount = 2;
            CanUserSortColumns = false;
            CanUserReorderColumns = false;
            CanUserResizeRows = false;
            HeadersVisibility = DataGridHeadersVisibility.Column;

            var menu = new ContextMenu();
            var menuItem = new MenuItem { Header = "Copy Data" };
            menuItem.Click += menuItem_Click;
            menu.Items.Add(menuItem);
            ContextMenu = menu;
        }


        public IEnumerable ItemsSourceEx
        {
            get { return (IEnumerable)GetValue(ItemsSourceExProperty); }
            set
            {
                SetValue(ItemsSourceExProperty, value);
                ItemsSource = value;
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSourceEx.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceExProperty =
            DependencyProperty.Register(
                "ItemsSourceEx", typeof(IEnumerable), typeof(DataGridExt),
                new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceExChanged)));

        private static void OnItemsSourceExChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(ItemsSourceProperty, e.NewValue);
            var dg = d as DataGridExt;

            var oldValue = e.OldValue as INotifyCollectionChangedEx;
            if (oldValue != null)
            {
                oldValue.BeginReset -= dg.NewValue_BeginReset;
                oldValue.EndReset -= dg.NewValue_EndReset;
            }

            var newValue = e.NewValue as INotifyCollectionChangedEx;
            if (newValue != null)
            {
                newValue.BeginReset += dg.NewValue_BeginReset;
                newValue.EndReset += dg.NewValue_EndReset;
            }
        }

        private object _oldSelectedItem;

        private void NewValue_BeginReset()
        {
            _oldSelectedItem = SelectedValue;
        }

        private void NewValue_EndReset()
        {
            SelectedValue = _oldSelectedItem;
            Items.Refresh();
        }

        void menuItem_Click(object sender, RoutedEventArgs e)
        {
            var data = ExportData();
            var builder = new StringBuilder();
            foreach (var row in data)
            {
                var lastCellIndex = row.Count - 1;
                for (var i = 0; i < lastCellIndex; i++)
                {
                    builder.Append(row[i]);
                    builder.Append("\t");
                }

                builder.AppendLine(row[lastCellIndex].ToString());
            }

            Clipboard.SetText(builder.ToString());
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SkippedSelectionChangedEvent)
                return;

            base.OnSelectionChanged(e);
        }

        public int FindFirstEditableColumnIndex(int beginIndex, DataGrid dataGrid)
        {
            var index = -1;
            for (int i = beginIndex; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].IsReadOnly == false
                    && SkippedColumnIndex.Contains(i) == false)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public void FocusCell(int row, int column, bool callBeginEdit = true)
        {
            Keyboard.Focus(this);

            SelectedIndex = row;

            CurrentCell = new DataGridCellInfo(Items[row], Columns[column]);

            if (callBeginEdit)
            {
                BeginEdit();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (!Keyboard.IsKeyDown(Key.Tab)) return;

            if (SelectedIndex == -1) return;

            var current = Columns.IndexOf(CurrentColumn);

            var index = FindFirstEditableColumnIndex(current + 1, this);

            if (index != -1)
            {
                CurrentCell = new DataGridCellInfo(Items[SelectedIndex], Columns[index]);

                BeginEdit();
            }
            else//move to next row, select first editable column
            {
                CommitEdit();

                var firstEditableColumnIndex = FindFirstEditableColumnIndex(0, this);

                const int itemPlaceHolderCount = 1;
                if (SelectedIndex < Items.Count - 1 - itemPlaceHolderCount)
                {
                    SelectedIndex = SelectedIndex + 1;
                }
                else //need add new item
                {
                    var type = ItemsSource.GetType();
                    var item = Items as IEditableCollectionViewAddNewItem;
                    item.AddNewItem(Activator.CreateInstance(type.GetGenericArguments()[0]));

                    SelectedIndex = Items.Count - 1 - itemPlaceHolderCount;
                }

                CurrentCell = new DataGridCellInfo(Items[SelectedIndex], Columns[firstEditableColumnIndex]);

                BeginEdit();
            }

            e.Handled = true;
        }

        protected override void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
        {
            base.OnPreparingCellForEdit(e);
            var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(e.EditingElement);

            var editingElementType = e.EditingElement.GetType();
            if (editingElementType.IsSubclassOf(typeof(ComboBox)) == true)
            {
                ActiveComboBox(e.EditingElement as ComboBox);
                return;
            }
            if (editingElementType == typeof(DatePicker))
            {
                ActiveDatePicker(e.EditingElement as DatePicker);
                return;
            }

            //for template column: e.EditingElement is ContentPresenter
            for (int i = 0; i < count; i++)
            {
                var element = System.Windows.Media.VisualTreeHelper.GetChild(e.EditingElement, i);
                var type = element.GetType();
                if (type == typeof(DatePicker))
                {
                    ActiveDatePicker(element as DatePicker);
                }
                else if (type.IsSubclassOf(typeof(ComboBox)) == true)
                {
                    ActiveComboBox(element as ComboBox);
                }
            }
        }

        private void ActiveComboBox(ComboBox combo)
        {
            var txt = VisualTreeUtils.FindChild<TextBox>(combo, "PART_EditableTextBox");
            Keyboard.Focus(txt);
            txt.SelectAll();
            combo.IsDropDownOpen = true;
        }

        private void ActiveDatePicker(DatePicker datePicker)
        {
            var txt = VisualTreeUtils.FindChild<TextBox>(datePicker, "PART_TextBox");
            Keyboard.Focus(txt);
            txt.Select(0, 2);
        }
        
        public List<List<object>> ExportData()
        {
            var result = new List<List<object>>();

            var bindingsPath = new List<string>();

            var header = new List<object>();

            foreach (var column in Columns)
            {
                if (column.Visibility == Visibility.Visible)
                {
                    if (column is DataGridBoundColumn)
                    {
                        var temp = column as DataGridBoundColumn;
                        bindingsPath.Add((temp.Binding as System.Windows.Data.Binding).Path.Path);

                        if (temp.Header is HeaderFilterBaseModel)
                            header.Add((temp.Header as HeaderFilterBaseModel).Name);
                        else
                            header.Add(temp.Header);
                    }
                    else if (column is DataGridComboBoxColumn)
                    {
                        var temp = column as DataGridComboBoxColumn;
                        if (temp.TextBinding != null)
                            bindingsPath.Add((temp.TextBinding as System.Windows.Data.Binding).Path.Path);
                        else
                            bindingsPath.Add((temp.SelectedValueBinding as System.Windows.Data.Binding).Path.Path);

                        if (temp.Header is HeaderFilterBaseModel)
                            header.Add((temp.Header as HeaderFilterBaseModel).Name);
                        else
                            header.Add(temp.Header);
                    }
                }
            }

            result.Add(header);

            foreach (var item in ItemsSource)
            {
                var row = new List<object>();

                foreach (var path in bindingsPath)
                {
                    var t = DataBinderUtils.Eval(item, path);
                    row.Add(t);
                }

                result.Add(row);
            }

            return result;
        }
    }
}
