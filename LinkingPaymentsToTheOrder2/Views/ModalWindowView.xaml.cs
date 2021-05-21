using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LinkingPaymentsToTheOrder2.Views
{
    public partial class ModalWindowView : Window
    {
        public ModalWindowView()
        {
            InitializeComponent();
        }

        void OnYes(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public bool? BindableDialogResult
        {
            get { return (bool?)GetValue(BindableDialogResultProperty); }
            set { SetValue(BindableDialogResultProperty, value); }
        }

        public static readonly DependencyProperty BindableDialogResultProperty =
            DependencyProperty.Register("BindableDialogResult", typeof(bool?), typeof(ModalWindowView),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
