using System;
using System.Collections.ObjectModel;

#nullable disable

namespace LinkingPaymentsToTheOrder2.Models
{
    public partial class Order : BaseModel
    {
        private decimal? _summPay;
        private ObservableCollection<Payment> _payments;

        public Order()
        {
            Payments = new ObservableCollection<Payment>();
        }

        public Order(Order value)
        {
            Number = value.Number;
            DateOrder = value.DateOrder;
            Summ = value.Summ;
            SummPay = value.SummPay;
            Payments = value.Payments;
        }

        public int Number { get; set; }
        public DateTime DateOrder { get; set; }
        public decimal Summ { get; set; }
        public decimal? SummPay 
        { 
            get => _summPay;
            set
            {
                _summPay = value;
                OnPropertyChanged(nameof(SummPay));
            }
        }

        public virtual ObservableCollection<Payment> Payments 
        { 
            get => _payments;
            set 
            {
                _payments = value;
                OnPropertyChanged(nameof(Payments));
            } 
        }
    }
}
