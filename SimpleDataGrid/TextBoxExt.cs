using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleDataGrid
{
    public class TextBoxExt : TextBox
    {
        public bool IsUseEnterKeyPressBinding { get; set; }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                e.Handled = true;

                Focus();
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            SelectAll();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (IsUseEnterKeyPressBinding == true && e.Key == Key.Return)
            {
                var exp = GetBindingExpression(TextBox.TextProperty);

                exp.UpdateSource();
            }
        }
    }
}
