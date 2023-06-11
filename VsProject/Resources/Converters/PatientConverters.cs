using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VsProject.Resources.Converters
{
    public class IsNewUserToPatientEditTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isNewUser)
            {
                if(isNewUser)
                {
                    return "Nouveau Patient";
                }
                else
                {
                    return "Modifier Patient";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class IsNewUserToPatientRecordEditTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isInHistory)
            {
                if(isInHistory)
                {
                    return "Historique du Patient";
                }
                else
                {
                    return "Bilan du Patient";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
