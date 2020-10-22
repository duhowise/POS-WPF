using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Magentix.Presentation.Controls.Interaction
{
    public class ButtonHelper
    {
        public readonly static DependencyProperty DialogResultProperty;

        static ButtonHelper()
        {
            Type type = typeof(bool?);
            Type type1 = typeof(ButtonHelper);
            UIPropertyMetadata uIPropertyMetadatum = new UIPropertyMetadata()
            {
                PropertyChangedCallback = (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                    Button button = obj as Button;
                    if (button == null)
                    {
                        throw new InvalidOperationException("Can only use ButtonHelper.DialogResult on a Button control");
                    }
                    button.Click += new RoutedEventHandler((object sender, RoutedEventArgs e2) => {
                        Window window = Window.GetWindow(button);
                        if (window != null)
                        {
                            window.DialogResult = ButtonHelper.GetDialogResult(button);
                        }
                    });
                }
            };
            ButtonHelper.DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", type, type1, uIPropertyMetadatum);
        }

        public ButtonHelper()
        {
        }

        public static bool? GetDialogResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(ButtonHelper.DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(ButtonHelper.DialogResultProperty, value);
        }
    }
}
