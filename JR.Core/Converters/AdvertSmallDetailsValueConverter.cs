using System;
using JR.DL.Model.Entities;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Localization;
using JR.SL.Service;

namespace JR.Core.Converters
{
    public class AdvertSmallDetailsValueConverter
        : MvxValueConverter
          
    {
        private IMvxTextProvider _textProvider;
        private IMvxTextProvider TextProvider
        {
            get
            {
                if (_textProvider == null)
                {
                    _textProvider = Mvx.Resolve<IMvxTextProvider>();
                }
                return _textProvider;
            }
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Advert = (Advert) value;
			if (Advert.Title == "") {
				return "";
			}
            var format = TextProvider.GetText(Constants.GeneralNamespace, Constants.Shared, (string)parameter);
            return string.Format(format, Advert.Type, Advert.Level, Advert.Where, Advert.When);
        }
    }
}