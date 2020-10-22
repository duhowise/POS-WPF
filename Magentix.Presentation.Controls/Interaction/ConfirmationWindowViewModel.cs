using Microsoft.Practices.Prism.Commands;
using Magentix.Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Magentix.Presentation.Controls.Interaction
{
    internal class ConfirmationWindowViewModel : ObservableObject
    {
        private readonly Window _window;

        private string _backgroundColor;

        private string _foregroundColor;

        public string BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }
        }

        public DelegateCommand<string> ButtonClickCommand
        {
            get;
            set;
        }

        public IEnumerable<ConfirmationWindowCommandButton> Buttons
        {
            get;
            set;
        }

        public string ForegroundColor
        {
            get
            {
                return this._foregroundColor;
            }
        }

        public bool IsHorizontalLayout
        {
            get;
            set;
        }

        public bool IsVerticalLayout
        {
            get;
            set;
        }

        public string Question
        {
            get;
            set;
        }

        public ConfirmationWindowViewModel(string question, string buttons, Window window, string background)
        {
            this._window = window;
            this.SetBackgroundColor(background);
            this.Question = question.Replace("\\r", Environment.NewLine);
            this.Buttons =
                from button in buttons.SplitCsv()
                select new ConfirmationWindowCommandButton(button) into x
                where !string.IsNullOrEmpty(x.DisplayName)
                select x;
            this.IsHorizontalLayout = this.Buttons.Any<ConfirmationWindowCommandButton>((ConfirmationWindowCommandButton x) => !string.IsNullOrEmpty(x.Description));
            this.IsVerticalLayout = !this.IsHorizontalLayout;
            this.ButtonClickCommand = new DelegateCommand<string>(new Action<string>(this.OnButtonClick));
        }

        private string B2F(string backgroundColor)
        {
            object obj = ColorConverter.ConvertFromString(backgroundColor);
            if (obj != null)
            {
                if (ConfirmationWindowViewModel.Brightness((obj is Color ? (Color)obj : new Color())) < 156)
                {
                    return "White";
                }
            }
            return "Black";
        }

        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt((double)(c.R * c.R) * 0.241 + (double)(c.G * c.G) * 0.691 + (double)(c.B * c.B) * 0.068);
        }

        private void OnButtonClick(string obj)
        {
            this._window.Tag = obj;
            this._window.Close();
        }

        private void SetBackgroundColor(string backgroundColor)
        {
            this._backgroundColor = backgroundColor;
            this._foregroundColor = this.B2F(this._backgroundColor);
        }
    }
}
