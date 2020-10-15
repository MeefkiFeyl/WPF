using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Linq;

namespace CAP.Converters
{
    public class CompaniesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ObservableCollection<Companies>)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
