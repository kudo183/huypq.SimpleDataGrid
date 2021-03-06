﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for EditableGridView.xaml
    /// </summary>
    public partial class EditableGridView : UserControl
    {
        public Visibility SaveButtonVisibility
        {
            get { return btnSave.Visibility; }
            set { btnSave.Visibility = value; }
        }

        public Visibility LoadButtonVisibility
        {
            get { return btnLoad.Visibility; }
            set { btnLoad.Visibility = value; }
        }

        public bool IsReadOnly
        {
            get { return dataGrid.IsReadOnly; }
            set { dataGrid.IsReadOnly = value; }
        }

        public ObservableCollection<DataGridColumn> Columns { get { return dataGrid.Columns; } }
        public UIElementCollection CustomMenuItems { get { return sp.Children; } }

        public EditableGridView()
        {
            InitializeComponent();
        }

        public void MapHeaderFilterModelToColumnHeader<T>(ViewModel.EditableGridViewModel<T> viewModel) where T : class
        {
            for (int i = 0; i < viewModel.HeaderFilters.Count; i++)
            {
                var filter = viewModel.HeaderFilters[i];
                if (filter.IsShowInUI == false)
                {
                    continue;
                }
                foreach (var column in Columns)
                {
                    if (column.Header.ToString() == filter.PropertyName)
                    {
                        column.Header = filter;
                        break;
                    }
                }
            }

        }

        public int FindColumnIndex(string columnName)
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                var vm = column.Header as ViewModel.HeaderFilterBaseModel;
                if (vm != null && vm.PropertyName == columnName)
                {
                    return i;
                }
            }
            return -1;
        }

        public DataGridColumn FindColumn(string columnName)
        {
            foreach (var column in Columns)
            {
                var vm = column.Header as ViewModel.HeaderFilterBaseModel;
                if (vm != null && vm.PropertyName == columnName)
                {
                    return column;
                }
            }
            return null;
        }

        public ViewModel.HeaderFilterBaseModel FindHeaderFilter(string columnName)
        {
            foreach (var column in Columns)
            {
                var vm = column.Header as ViewModel.HeaderFilterBaseModel;
                if (vm != null && vm.PropertyName == columnName)
                {
                    return vm;
                }
            }
            return null;
        }
    }
}
