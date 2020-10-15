using CAP.Models;
using System.ComponentModel.DataAnnotations;

namespace CAP
{
    public partial class Users : BaseModel
    {
        private string _name;
        private string _login;
        private string _password;

        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Required]
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

        [StringLength(25)]
        public string Login 
        { 
            get => _login; 
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            } 
        }

        [StringLength(25)]
        public string Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            } 
        }

        public virtual Companies Companies { get; set; }
    }
}
