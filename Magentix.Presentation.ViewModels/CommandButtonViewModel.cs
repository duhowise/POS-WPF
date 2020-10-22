using Magentix.Localization.Properties;
using Magentix.Presentation.Common;
using Magentix.Presentation.Common.Commands;

namespace Magentix.Presentation.ViewModels
{
    public class CommandButtonViewModel<T> : ObservableObject
    {
        public ICaptionCommand Command { get; set; }

        private string _caption;
        public string Caption
        {
            get {
                string caption = _caption;
                switch(_caption.Replace(" ", ""))
                {
                    case "Cash":
                        caption = Resources.Cash;
                        break;
                    case "CreditCard":
                        caption = Resources.CreditCard;
                        break;
                    case "Voucher":
                        caption = Resources.Voucher;
                        break;
                    case "CustomerAccount":
                        caption = Resources.CustomerAccount;
                        break;
                    case "Discount%":
                        caption = Resources.DiscountPercentSign;
                        break;
                    case "Round":
                        caption = Resources.Round;
                        break;
                    case "PrintBill":
                        caption = Resources.PrintBill;
                        break;
                }
                return caption; 
            }
            set
            {
                _caption = value; 
                RaisePropertyChanged(() => Caption);
            }
        }

        public T Parameter { get; set; }
        public string Color { get; set; }
        public int FontSize { get; set; }
    }
}
