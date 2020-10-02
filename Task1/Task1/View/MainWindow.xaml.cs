using System.Windows;

namespace Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource companiesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("companiesViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // companiesViewSource.Source = [generic data source]
        }
    }
}
