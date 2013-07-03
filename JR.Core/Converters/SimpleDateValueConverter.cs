using System;
using System.Net;
using Cirrious.CrossCore.Converters;

namespace JR.Core.Converters
{
    public class SimpleDateValueConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is DateTime))
                return value;

            var format = parameter ?? "dd-MM-yyyy";

            var dateValue = (DateTime) value;
            return dateValue.ToLocalTime().ToString((string)format);
        }
    }
}
