using System;
using System.Collections.Generic;
using System.Linq;

namespace Magentix.Services
{
    public interface IDialogService
    {
        string AskAdminPassword();

        bool AskQuestion(string question);

        string AskQuestion(string question, string buttons, string backgroundColor = "White");

        bool Confirm(string question);

        Dictionary<string, string> EditProperties(Dictionary<string, string> values);
    }
}
