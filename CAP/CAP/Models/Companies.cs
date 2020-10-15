using CAP.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CAP
{
    public partial class Companies : BaseModel
    {
        private string _name;
        private int? _contractStatus;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Companies()
        {
            Users = new ObservableCollection<Users>();
        }

        public int Id { get; set; }

        [StringLength(25)]
        public string Name 
        { 
            get => _name; 
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }

        public int? ContractStatus 
        { 
            get => _contractStatus; 
            set
            {
                _contractStatus = value;
                OnPropertyChanged(nameof(ContractStatus));
            }
        }

        public virtual ContractStatuses ContractStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Users> Users { get; set; }
    }
}
