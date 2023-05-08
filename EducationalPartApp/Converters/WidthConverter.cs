
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;


namespace EducationalPartApp.Converters
{
    public class WidthConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((value != null) && (parameter != null))
            {
                double dparam = double.Parse(parameter.ToString());
                double width = (double)value - dparam;
                return (width > 0) ? width : 0;
            }

            return null;
        }
    }
}
