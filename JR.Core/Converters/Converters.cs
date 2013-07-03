using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.Visibility;

namespace JR.Core.Converters
{
    public class Converters
    {
        public readonly TimeAgoValueConverter TimeAgo = new TimeAgoValueConverter();
        public readonly AdvertSmallDetailsValueConverter AdvertSmallDetails = new AdvertSmallDetailsValueConverter();
        public readonly SimpleDateValueConverter SimpleDate = new SimpleDateValueConverter();
        public readonly MvxVisibilityValueConverter Visibility = new MvxVisibilityValueConverter();
        public readonly MvxLanguageConverter Language = new MvxLanguageConverter();
    }
}