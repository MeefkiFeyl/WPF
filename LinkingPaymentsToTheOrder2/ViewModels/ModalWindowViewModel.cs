using LinkingPaymentsToTheOrder2.Commands;
using System.Windows.Input;
using LinkingPaymentsToTheOrder2.Context;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace LinkingPaymentsToTheOrder2.ViewModels
{
    public class ModalWindowViewModel : BaseModalViewModel
    {
        #region Properties

        public string Err { get; }
        public bool IsYesNoMode { get; set; }

        private string _textMessage;
        public string TextMessage
        {
            get => _textMessage;
            set
            {
                _textMessage = value;
                OnPropertyChanged(nameof(TextMessage));
            }
        }

        public bool IsVisibleExpander { get; set; }

        public AppDbContext Context { get; }
        #endregion Properties

        public ModalWindowViewModel()
        {
        }
        public ModalWindowViewModel(AppDbContext context, string message, int code, string e = "")
        {
            TextMessage = message;
            Err = e;
            Context = context;

            IsVisibleExpander = true;

            if (code == 1)
                IsYesNoMode = false;
            else
                IsYesNoMode = true;
        }

        public ICommand OnYes => new DelegateCommand(obj =>
        {
            Context.LoadData(Context);
        });
    }
}
