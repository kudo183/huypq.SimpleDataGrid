﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SimpleDataGrid.ViewModel;
using System.Collections;
using System.Linq;
using huypq.wpf.Utils;

namespace SimpleDataGrid
{
    public class DataGridExt : DataGrid
    {
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

            KeepSelectionType = KeepSelection.KeepSelectedIndex;

            var menu = new ContextMenu();
            var menuItem = new MenuItem { Header = "Copy Data" };
            menuItem.Click += menuItem_Click;
            menu.Items.Add(menuItem);
            ContextMenu = menu;

            GotKeyboardFocus += DataGridExt_GotKeyboardFocus;
            Loaded += DataGridExt_Loaded;
            Unloaded += DataGridExt_Unloaded;
        }

        private bool _isLoaded = false;
        private bool _needUpdateSelectedIndexInGotFocusEvent = false;
        private bool _needUpdateCurrentCellInGotFocusEvent = false;

        private void DataGridExt_Unloaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = false;
        }

        private void DataGridExt_Loaded(object sender, RoutedEventArgs e)
        {
            //don't know why Loaded event fire two time, so need flag _isLoaded
            if (_isLoaded == true)
                return;

            _isLoaded = true;
            //CurrentCell not set to correct SelectedIndex when load -> need update CurrentCell in GotFocus
            _needUpdateCurrentCellInGotFocusEvent = true;
        }

        private void DataGridExt_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var cell = e.NewFocus as DataGridCell;
            if (cell != null)
            {
                if (_needUpdateSelectedIndexInGotFocusEvent)
                {
                    _needUpdateSelectedIndexInGotFocusEvent = false;

                    var index = Items.IndexOf(CurrentCell.Item);
                    if (SelectedIndex != index)
                    {
                        SelectedIndex = index;
                    }
                }
                else if (_needUpdateCurrentCellInGotFocusEvent == true)
                {
                    _needUpdateCurrentCellInGotFocusEvent = false;

                    if ((SelectedIndex < Items.Count - 1) && SelectedIndex > 0)
                    {
                        var index = Items.IndexOf(CurrentCell.Item);
                        if (SelectedIndex != index)
                        {
                            CurrentCell = new DataGridCellInfo(Items[SelectedIndex], Columns[1]);
                        }
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Handled == false)
            {
                //SelectedIndex not update when move focus from header/footer to cell using Up/Down Arrow key
                // -> need update SelectedIndex base on current focused cell in GotFocus event
                if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up))
                {
                    _needUpdateSelectedIndexInGotFocusEvent = true;
                }
            }
        }

        //private DataGrid GetDataGridFromChild(DependencyObject child)
        //{
        //    if (child == null)
        //    {
        //        return null;
        //    }
        //    var p = VisualTreeHelper.GetParent(child);
        //    var datagrid = p as DataGrid;
        //    if (datagrid != null)
        //    {
        //        return datagrid;
        //    }
        //    return GetDataGridFromChild(p);
        //}

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

        public enum KeepSelection
        {
            NotKeepSelection,
            KeepSelectedValue,
            KeepSelectedIndex
        }

        /// <summary>
        /// default value is KeepSelectedIndex
        /// </summary>
        public KeepSelection KeepSelectionType { get; set; }

        private void NewValue_BeginReset()
        {
            CommitEdit(DataGridEditingUnit.Row, true);

            switch (KeepSelectionType)
            {
                case KeepSelection.NotKeepSelection:
                    break;
                case KeepSelection.KeepSelectedValue:
                    _oldSelectedItem = SelectedValue;
                    break;
                case KeepSelection.KeepSelectedIndex:
                    _oldSelectedItem = SelectedIndex;
                    break;
            }
        }

        private void NewValue_EndReset()
        {
            switch (KeepSelectionType)
            {
                case KeepSelection.NotKeepSelection:
                    break;
                case KeepSelection.KeepSelectedValue:
                    SelectedValue = _oldSelectedItem;
                    break;
                case KeepSelection.KeepSelectedIndex:

                    //don't know why but set SelectedIndex not work, set SelectedValue work
                    //SelectedIndex = (int)_oldSelectedItem;

                    var index = (int)_oldSelectedItem;
                    if (index == -1)
                    {
                        SelectedValue = null;
                    }
                    else if (string.IsNullOrEmpty(SelectedValuePath) == false)
                    {
                        var i = 0;

                        foreach (var item in ItemsSourceEx)
                        {
                            if (i == index)
                            {
                                var p = item.GetType().GetProperty(SelectedValuePath);
                                if (p != null)
                                {
                                    SelectedValue = p.GetValue(item);
                                }
                                break;
                            }
                            i++;
                        }
                    }
                    break;
            }
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

        public DataGridColumn FindFirstEditableColumn(int beginDisplayIndex = 0)
        {
            for (int i = beginDisplayIndex; i < Columns.Count; i++)
            {
                var col = Columns.First(p => p.DisplayIndex == i);

                if (DataGridColumnAttachedProperty.GetIsTabStop(col) == false)
                    continue;

                if (col.IsReadOnly == true)
                    continue;

                return col;
            }

            return null;
        }

        public void FocusCell(int row, DataGridColumn column, bool callBeginEdit = true)
        {
            Keyboard.Focus(this);

            SelectedIndex = row;

            if (column != null)
            {
                CurrentCell = new DataGridCellInfo(Items[row], column);

                if (callBeginEdit)
                {
                    BeginEdit();
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (!Keyboard.IsKeyDown(Key.Tab)) return;

            if (SelectedIndex == -1) return;

            e.Handled = true;

            var current = CurrentColumn.DisplayIndex;

            var firstEditableColumn = FindFirstEditableColumn(current + 1);

            if (firstEditableColumn != null)
            {
                CurrentCell = new DataGridCellInfo(Items[SelectedIndex], firstEditableColumn);

                BeginEdit();
            }
            else //move to next row, select first editable column            
            {
                CommitEdit();

                firstEditableColumn = FindFirstEditableColumn();

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

                CurrentCell = new DataGridCellInfo(Items[SelectedIndex], firstEditableColumn);

                BeginEdit();
            }
        }

        protected override void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
        {
            base.OnPreparingCellForEdit(e);

            var editingElementType = e.EditingElement.GetType();
            if (editingElementType == typeof(ComboBox))
            {
                ActiveComboBox(e.EditingElement as ComboBox);
                return;
            }
            if (editingElementType == typeof(DatePicker))
            {
                ActiveDatePicker(e.EditingElement as DatePicker);
                return;
            }
            if (editingElementType == typeof(ForeignKeyPicker))
            {
                ActiveForeignKeyPicker(e.EditingElement as ForeignKeyPicker);
                return;
            }

            //for template column: e.EditingElement is ContentPresenter
            var count = VisualTreeHelper.GetChildrenCount(e.EditingElement);
            for (int i = 0; i < count; i++)
            {
                var element = VisualTreeHelper.GetChild(e.EditingElement, i);
                var type = element.GetType();
                if (type == typeof(DatePicker))
                {
                    ActiveDatePicker(element as DatePicker);
                    return;
                }
                if (editingElementType == typeof(ComboBox))
                {
                    ActiveComboBox(element as ComboBox);
                    return;
                }
                if (editingElementType == typeof(ForeignKeyPicker))
                {
                    ActiveForeignKeyPicker(e.EditingElement as ForeignKeyPicker);
                    return;
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

        private void ActiveForeignKeyPicker(ForeignKeyPicker foreignKeyPicker)
        {
            foreignKeyPicker.IsPopupOpen = true;
        }

        public List<List<object>> ExportData()
        {
            var result = new List<List<object>>();

            var bindingsInfo = new List<BindingInfo>();

            var header = new List<object>();

            foreach (var column in Columns.OrderBy(p => p.DisplayIndex))
            {
                if (column.Visibility == Visibility.Visible)
                {
                    if (column is DataGridBoundColumn)
                    {
                        var temp = column as DataGridBoundColumn;
                        bindingsInfo.Add(new BindingInfo()
                        {
                            Type = 0,
                            Path = (temp.Binding as System.Windows.Data.Binding).Path.Path
                        });

                        if (temp.Header is HeaderFilterBaseModel)
                            header.Add((temp.Header as HeaderFilterBaseModel).Name);
                        else
                            header.Add(temp.Header);
                    }
                    else if (column is DataGridComboBoxColumn)
                    {
                        var temp = column as DataGridComboBoxColumn;
                        if (temp.TextBinding != null)
                        {
                            bindingsInfo.Add(new BindingInfo()
                            {
                                Type = 0,
                                Path = (temp.TextBinding as System.Windows.Data.Binding).Path.Path
                            });
                        }
                        else
                        {
                            var binding = System.Windows.Data.BindingOperations.GetBinding(temp, ComboBox.ItemsSourceProperty);
                            bindingsInfo.Add(new BindingInfo()
                            {
                                Type = 1,
                                Path = (temp.SelectedValueBinding as System.Windows.Data.Binding).Path.Path,
                                DisplayPath = temp.DisplayMemberPath,
                                SelectedValuePath = temp.SelectedValuePath,
                                ItemsSourcePath = binding.Path.Path
                            });
                        }
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

                foreach (var binding in bindingsInfo)
                {
                    switch (binding.Type)
                    {
                        case 0:
                            row.Add(BindingUtils.Eval(item, binding.Path));
                            break;
                        case 1:
                            var itemsSource = BindingUtils.Eval(item, binding.ItemsSourcePath) as IEnumerable;
                            if (itemsSource == null)
                            {
                                row.Add(null);
                                break;
                            }

                            var v = BindingUtils.Eval(item, binding.Path);
                            foreach (var comboBoxItem in itemsSource)
                            {
                                var v1 = BindingUtils.Eval(comboBoxItem, binding.SelectedValuePath);
                                if (v.Equals(v1) == true)
                                {
                                    row.Add(BindingUtils.Eval(comboBoxItem, binding.DisplayPath));
                                    break;
                                }
                            }
                            break;
                        default:
                            row.Add(null);
                            break;
                    }
                }

                result.Add(row);
            }

            return result;
        }

        private class BindingInfo
        {
            public int Type { get; set; }
            public string Path { get; set; }
            public string DisplayPath { get; set; }
            public string SelectedValuePath { get; set; }
            public string ItemsSourcePath { get; set; }
        }
    }
}
