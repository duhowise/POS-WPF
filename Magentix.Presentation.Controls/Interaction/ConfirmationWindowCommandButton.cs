using System;
using System.Runtime.CompilerServices;

namespace Magentix.Presentation.Controls.Interaction
{
    public class ConfirmationWindowCommandButton
    {
        public string Color
        {
            get;
            set;
        }

        public string CommandName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string HoverColor
        {
            get;
            set;
        }

        public ConfirmationWindowCommandButton(string commandDefinition)
        {
            this.HoverColor = "Silver";
            this.Color = "Gainsboro";
            if (string.IsNullOrWhiteSpace(commandDefinition) || commandDefinition.Trim().StartsWith("=") || commandDefinition.Trim().StartsWith("#"))
            {
                return;
            }
            commandDefinition = commandDefinition.Trim(new char[] { '\"' });
            this.SetDisplayName(commandDefinition);
            if (!commandDefinition.Contains("="))
            {
                this.CommandName = commandDefinition;
            }
            else
            {
                char[] chrArray = new char[] { '=' };
                string[] strArrays = commandDefinition.Split(chrArray, 2);
                this.SetDisplayName(strArrays[0]);
                this.CommandName = strArrays[1];
            }
            if (this.CommandName.Contains(":"))
            {
                string commandName = this.CommandName;
                char[] chrArray1 = new char[] { ':' };
                string[] strArrays1 = commandName.Split(chrArray1, 2);
                this.CommandName = strArrays1[0];
                this.SetColor(strArrays1[1]);
            }
        }

        private void SetColor(string color)
        {
            this.HoverColor = color;
            if (color.Contains(";"))
            {
                char[] chrArray = new char[] { ';' };
                string[] strArrays = color.Split(chrArray, 2);
                this.Color = strArrays[0];
                this.HoverColor = strArrays[1];
            }
        }

        private void SetDisplayName(string displayName)
        {
            this.DisplayName = displayName;
            if (displayName.Contains("#"))
            {
                string[] strArrays = displayName.Split(new char[] { '#' });
                this.DisplayName = strArrays[0];
                this.Description = strArrays[1];
                if (string.IsNullOrEmpty(this.Description))
                {
                    this.DisplayName = "";
                }
            }
        }
    }
}
