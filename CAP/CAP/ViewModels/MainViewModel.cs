using CAP.Command;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace CAP.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region Properties

        private ObservableCollection<string> _contractStatus;
        private Companies _selectedCompany;
        private Companies _company;
        private Users _selectedUser;
        private Users _user;

        public ObservableCollection<string> ContractStatus
        {
            get => _contractStatus;
            set
            {
                _contractStatus = value;
                OnPropertyChanged(nameof(ContractStatus));
            }
        }

        public CAPContext Context { get; }

        public Companies SelectedCompany
        { 
            get => _selectedCompany;
            set
            {
                _selectedCompany = value;
                if (value != null)
                    Company = new Companies
                    {
                        Name = value.Name,
                        ContractStatus = value.ContractStatus,
                    };

                OnPropertyChanged(nameof(SelectedCompany));
            }
        }
        public Companies Company
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public Users SelectedUser { 
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (value != null)
                    User = new Users
                    {
                        Name = value.Name,
                        Login = value.Login,
                        Password = value.Password
                    };

                OnPropertyChanged(nameof(SelectedUser));
            } 
        }
        public Users User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        

        #endregion

        public MainViewModel()
        {
            ContractStatus = new ObservableCollection<string>();
            SelectedCompany = new Companies
            {
                Name = "",
                ContractStatus = 0,
                Users = new ObservableCollection<Users>()
            };
            SelectedUser = new Users();


            Context = new CAPContext();
          
            Context.ContractStatuses.Load();
            Context.Companies.Load();
            Context.Users.Load();

            foreach (ContractStatuses item in Context.ContractStatuses.Local)
                ContractStatus.Add(item.ContractStatus);
        }

        #region Commands

        public ICommand AddCompany => new DelegateCommand(obj =>
        {
            if (Company.Name != null && Company.ContractStatus != null)
            {
                Context.Companies.Add(new Companies
                {
                    Name = Company.Name,
                    ContractStatus = Company.ContractStatus,
                    ContractStatuses = Company.ContractStatuses
                });
                Context.SaveChanges();
            }
            else MessageBox.Show("Enter the Company data!");
        });

        public ICommand EditCompany => new DelegateCommand(obj =>
        {
            if (SelectedCompany != null && Company.Name != null)
            {
                Context.Companies.Find(SelectedCompany.Id).Name = Company.Name;
                Context.Companies.Find(SelectedCompany.Id).ContractStatus = Company.ContractStatus;

                Context.SaveChanges();
            }
        });

        public ICommand DeleteCompany => new DelegateCommand(obj =>
        {
            if (SelectedCompany != null)
            {
                Context.Companies.Remove(SelectedCompany);
                Context.SaveChanges();
            }
            else
                MessageBox.Show("Select a Company to Delete!");
        });

        public ICommand AddUser => new DelegateCommand(obj =>
        {
            if (User.Name != null && SelectedCompany.Name != null)
            {
                Context.Users.Add(new Users
                {
                    Name = User.Name,
                    CompanyId = SelectedCompany.Id,
                    Login = User.Login,
                    Password = User.Password,
                    Companies = SelectedCompany
                });
                Context.SaveChanges();
            }
            else MessageBox.Show("Username not entered or Company not selected!");
        });

        public ICommand EditUser => new DelegateCommand(obj =>
        { 
            if (SelectedUser != null && User.Name != null)
            {
                Context.Users.Find(SelectedUser.Id).Name = User.Name;
                Context.Users.Find(SelectedUser.Id).Login = User.Login;
                Context.Users.Find(SelectedUser.Id).Password = User.Password;

                Context.SaveChanges();
            }
        });

        public ICommand DeleteUser => new DelegateCommand(obj =>
        {
            if (_selectedUser != null)
            {
                Context.Users.Remove(_selectedUser);
                Context.SaveChanges();
            }
            else
                MessageBox.Show("Select a User to Delete!");
        });

        #endregion
    } 
}