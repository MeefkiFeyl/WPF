using System;
using System.Collections.Generic;

#nullable disable

namespace LinkingPaymentsToTheOrder2.Models
{
    public partial class Payment : BaseModel
    {
        private int _id;
        public int Id 
        { 
            get => _id;
            set 
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            } 
        }
        public int NumberOrder { get; set; }
        public int NumberMoneyComing { get; set; }
        public decimal SummPay { get; set; }

        public virtual MoneyComing NumberMoneyComingNavigation { get; set; }
        public virtual Order NumberOrderNavigation { get; set; }
    }
}
