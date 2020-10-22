using Magentix.Infrastructure.Settings;
using Magentix.Localization.Properties;
using Magentix.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Magentix.Presentation.Controls.Interaction
{
    [Export(typeof(IDialogService))]
    internal class DialogService : IDialogService
    {
        public DialogService()
        {
        }

        public string AskAdminPassword()
        {
            PasswordEditWindow passwordEditWindow = new PasswordEditWindow()
            {
                Owner = Application.Current.MainWindow
            };
            passwordEditWindow.ShowDialog();
            return Regex.Replace(passwordEditWindow.AdminPin ?? "", "[^.0-9]", "");
        }

        public bool AskQuestion(string question)
        {
            return MessageBox.Show(question, Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
        }

        public string AskQuestion(string question, string buttons, string backgroundColor)
        {
            ConfirmationWindow confirmationWindow = new ConfirmationWindow(question, buttons, backgroundColor)
            {
                Owner = Application.Current.MainWindow
            };
            ConfirmationWindow confirmationWindow1 = confirmationWindow;
            if (LocalSettings.WindowScale > 0)
            {
                ScaleTransform layoutTransform = confirmationWindow1.LayoutTransform as ScaleTransform;
                if (layoutTransform != null)
                {
                    layoutTransform.ScaleX = LocalSettings.WindowScale;
                    layoutTransform.ScaleY = LocalSettings.WindowScale;
                }
            }
            confirmationWindow1.ShowDialog();
            return confirmationWindow1.Tag.ToString();
        }

        public bool Confirm(string question)
        {
            ConfirmationWindow confirmationWindow = new ConfirmationWindow(question, string.Format("{0},{1}", Resources.Yes, Resources.No), "White");
            confirmationWindow.ShowDialog();
            return confirmationWindow.Tag.ToString() == Resources.Yes;
        }

        public Dictionary<string, string> EditProperties(Dictionary<string, string> values)
        {
            return null;
            //DictionaryEditWindow dictionaryEditWindow = new DictionaryEditWindow()
            //{
            //    Owner = Application.Current.MainWindow
            //};
            //dictionaryEditWindow.SetDictionary(values);
            //bool? nullable = dictionaryEditWindow.ShowDialog();
            //values = dictionaryEditWindow.GetDictionary();
            //if (!nullable.GetValueOrDefault())
            //{
            //    return null;
            //}
            //return values;
        }
    }
}
