using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable

namespace LinkingPaymentsToTheOrder2.Models
{
    public partial class MoneyComing : BaseModel
    {
        private decimal _balance;

        public MoneyComing()
        {
            Payments = new ObservableCollection<Payment>();
        }

        public int Number { get; set; }
        public DateTime DatePayment { get; set; }
        public decimal Summ { get; set; }
        public decimal Balance 
        { 
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            } 
        }

        public virtual ObservableCollection<Payment> Payments { get; set; }
    }
}
