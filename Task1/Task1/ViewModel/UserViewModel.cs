using System.Collections.ObjectModel;

namespace Task1.ViewModel
{
    class UserViewModel : BaseViewModel
    {
        private Users selectedUser;
        public ObservableCollection<Users> Users { get; set; }
        public Users SelectedUsers
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
    }
}
