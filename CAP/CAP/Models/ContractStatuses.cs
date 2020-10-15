using CAP.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace CAP
{
    public partial class ContractStatuses : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContractStatuses()
        {
            Companies = new ObservableCollection<Companies>();
        }

        public int Id { get; set; }

        [StringLength(20)]
        public string ContractStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Companies> Companies { get; set; }
    }
}
