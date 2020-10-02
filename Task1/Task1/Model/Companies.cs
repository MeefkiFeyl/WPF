using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Task1
{
    public partial class Companies
    {
        public Companies()
        {
            Users = new ObservableCollection<Users>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContractStatus { get; set; }

        public virtual ObservableCollection<Users> Users { get; set; }
    }
}
