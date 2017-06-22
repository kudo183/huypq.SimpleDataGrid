using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleDataGrid
{
    /// <summary>
    /// Because if derive from UserControl, when set focus to Popup.Child control, Popup.IsKeyboardFocusWithin = true but Popup.Parent.IsKeyboardFocusWithin = false (don't know why).
    /// It will cause DataGridCell commit edit because Focus is lost from this control (IsKeyboardFocusWithin = false).
    /// Derive from Control will fix this issue.
    /// </summary>
    public class ForeignKeyPicker : Control
    {
        private const string ElementRoot = "PART_Root";
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";
        private const string ElementPopup = "PART_Popup";

        private ButtonBase dropDownButton;
        private Popup popup;
        private TextBox textBox;
        
        public FrameworkElement PopupView
        {
            get { return (FrameworkElement)GetValue(PopupViewProperty); }
            set { SetValue(PopupViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupViewProperty =
            DependencyProperty.Register("PopupView", typeof(FrameworkElement), typeof(ForeignKeyPicker), new PropertyMetadata(null));

        public string PopupViewSelectedIDPath
        {
            get { return (string)GetValue(PopupViewSelectedIDPathProperty); }
            set { SetValue(PopupViewSelectedIDPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupViewSelectedIDPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupViewSelectedIDPathProperty =
            DependencyProperty.Register("PopupViewSelectedIDPath", typeof(string), typeof(ForeignKeyPicker), new PropertyMetadata(string.Empty));

        public bool IsPopupOpen
        {
            get
            {
                if (popup != null)
                {
                    return popup.IsOpen;
                }
                return false;
            }
            set
            {
                if (popup != null)
                {
                    if (value == true)
                    {
                        OpenPopup();
                    }
                    else
                    {
                        ClosePopup();
                    }
                }
            }
        }

        private bool _disablePopupReopen;

        public int SelectedForeignKey
        {
            get { return (int)GetValue(SelectedForeignKeyProperty); }
            set { SetValue(SelectedForeignKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedForeignKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedForeignKeyProperty =
            DependencyProperty.Register("SelectedForeignKey", typeof(int), typeof(ForeignKeyPicker), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedForeignKeyChanged));

        private static void OnSelectedForeignKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fp = d as ForeignKeyPicker;

            fp.textBox.Text = fp.SelectedForeignKey.ToString();
        }

        static ForeignKeyPicker()
        {
            EventManager.RegisterClassHandler(typeof(ForeignKeyPicker), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown));
            EventManager.RegisterClassHandler(typeof(ForeignKeyPicker), UIElement.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(ForeignKeyPicker), UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDown));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForeignKeyPicker), new FrameworkPropertyMetadata(typeof(ForeignKeyPicker)));
            //KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ForeignKeyPicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(ForeignKeyPicker), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// Builds the visual tree for the DatePicker control when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (popup != null)
            {
                popup.Opened -= popup_Opened;
                popup.Closed -= popup_Closed;
                popup.Child = null;
            }

            if (dropDownButton != null)
            {
                dropDownButton.Click -= dropdownButton_Click;
                dropDownButton.RemoveHandler(MouseLeaveEvent, new MouseEventHandler(dropdownButton_MouseLeave));
            }

            if (textBox != null)
            {
                textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                textBox.TextChanged -= TextBox_TextChanged;
            }

            base.OnApplyTemplate();

            popup = GetTemplateChild(ElementPopup) as Popup;

            if (popup != null)
            {
                popup.Opened += popup_Opened;
                popup.Closed += popup_Closed;

                popup.Child = PopupView;

            }

            dropDownButton = GetTemplateChild(ElementButton) as Button;
            if (dropDownButton != null)
            {
                dropDownButton.Click += dropdownButton_Click;
                dropDownButton.AddHandler(MouseLeaveEvent, new MouseEventHandler(dropdownButton_MouseLeave), true);

                // If the user does not provide a Content value in template, we provide a helper text that can be used in Accessibility
                // this text is not shown on the UI, just used for Accessibility purposes
                if (dropDownButton.Content == null)
                {
                    dropDownButton.Content = "...";
                }
            }

            textBox = GetTemplateChild(ElementTextBox) as TextBox;
            if (textBox != null)
            {
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var foreignKeyPicker = sender as ForeignKeyPicker;

            if (foreignKeyPicker.IsPopupOpen == true)
            {
                var p = (e.OriginalSource as FrameworkElement).Parent;
                if (p is DataGridCell)
                {
                    foreignKeyPicker.ClosePopup();
                    //not set Handled because need this event to update SelectedIndex
                }
            }
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var foreignKeyPicker = sender as ForeignKeyPicker;
            if (e.Key == Key.Enter)
            {
                if (foreignKeyPicker.IsPopupOpen == true)
                {
                    if (e.OriginalSource is DataGridCell)
                    {
                        foreignKeyPicker.ClosePopup();
                        e.Handled = true;//set Handled for not move to next row
                    }
                }
            }
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            var foreignKeyPicker = sender as ForeignKeyPicker;
            if (e.Key == Key.Enter)
            {
                if (foreignKeyPicker.IsPopupOpen == true)
                {
                    //prevent Enter key bubble to parent when not Enter in DataGridCell
                    //need set in this event instead of PreviewKeyDown event because if set in PreviewKeyDown event page index, header text filter, ... will not receive Enter key down event.
                    e.Handled = true;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SelectedForeignKey.ToString();
            if (textBox.Text != text)
            {
                SetCurrentValue(SelectedForeignKeyProperty, int.Parse(textBox.Text));
                var p = PopupView.DataContext.GetType().GetProperty(PopupViewSelectedIDPath);
                p.SetValue(PopupView.DataContext, SelectedForeignKey);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void dropdownButton_Click(object sender, RoutedEventArgs e)
        {
            if (popup.IsOpen == true)
            {
                ClosePopup();
            }
            else
            {
                if (_disablePopupReopen)
                {
                    _disablePopupReopen = false;
                }
                else
                {
                    OpenPopup();
                }
            }
        }

        private void popup_Opened(object sender, EventArgs e)
        {
            PopupView.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            var iNotify = PopupView.DataContext as INotifyPropertyChanged;
            if (iNotify != null)
            {
                iNotify.PropertyChanged += INotify_PropertyChanged;
            }
        }

        private void INotify_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PopupViewSelectedIDPath)
            {
                SetSelectedForeignKey();
            }
        }

        public void SetSelectedForeignKey()
        {
            if (PopupView == null || PopupView.DataContext == null)
            {
                return;
            }
            var p = PopupView.DataContext.GetType().GetProperty(PopupViewSelectedIDPath);
            if (p == null)
            {
                return;
            }
            var v = p.GetValue(PopupView.DataContext);
            if (v != null)
            {
                SetCurrentValue(SelectedForeignKeyProperty, (int)v);
            }
        }

        private void popup_Closed(object sender, EventArgs e)
        {
            if (PopupView.IsKeyboardFocusWithin)
            {
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }
            var iNotify = PopupView.DataContext as INotifyPropertyChanged;
            if (iNotify != null)
            {
                iNotify.PropertyChanged -= INotify_PropertyChanged;
            }
        }

        private void dropdownButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _disablePopupReopen = false;
        }

        private void OpenPopup()
        {
            if (popup.Child == null)
            {
                popup.Child = PopupView;
            }

            popup.IsOpen = true;
        }

        private void ClosePopup()
        {
            popup.IsOpen = false;
        }
    }
}
