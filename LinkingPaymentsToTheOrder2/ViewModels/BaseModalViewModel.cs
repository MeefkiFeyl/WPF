using System;
using System.Collections.Generic;
using System.Text;

namespace LinkingPaymentsToTheOrder2.ViewModels
{
    public class BaseModalViewModel : BaseViewModel
    {
        private bool? _answer = null;
        public bool? Answer
        {
            get => _answer;
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
