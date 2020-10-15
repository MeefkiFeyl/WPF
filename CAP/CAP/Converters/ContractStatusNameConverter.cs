using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace CAP.Converters
{
    class ContractStatusNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = "Not defined";
            CAPContext context = new CAPContext();

            if (value != null)
                switch ((int)value)
                {
                    case 1:
                        name = context.ContractStatuses.Find(1).ContractStatus;
                        break;
                    case 2:
                        name = context.ContractStatuses.Find(2).ContractStatus;
                        break;
                    case 3:
                        name = context.ContractStatuses.Find(3).ContractStatus;
                        break;
                }

            return name;
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = new int();
            CAPContext context = new CAPContext();

            if (value != null)
                switch ((string)value)
                {
                    case "Заключён":
                        id = 1;
                        break;
                    case "Ещё не заключён":
                        id = 2;
                        break;
                    case "Расторгнут":
                        id = 3;
                        break;
                }

            return id;
        }
    }
}
