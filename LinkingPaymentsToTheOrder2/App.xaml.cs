using LinkingPaymentsToTheOrder2.Context;
using LinkingPaymentsToTheOrder2.Services;
using LinkingPaymentsToTheOrder2.ViewModels;
using LinkingPaymentsToTheOrder2.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows;

namespace LinkingPaymentsToTheOrder2
{
    public partial class App : Application
    {
        DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        MainViewModel mainVM;
        readonly AppDbContext context;

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainViewModel, MainWindow>();
            displayRootRegistry.RegisterWindowType<LinkingViewModel, LinkingView>();
            displayRootRegistry.RegisterWindowType<ModalWindowViewModel, ModalWindowView>();

            context = new AppDbContext();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            mainVM = new MainViewModel(context, displayRootRegistry);
            displayRootRegistry.ShowPresentation(mainVM);
        }
    }
}
