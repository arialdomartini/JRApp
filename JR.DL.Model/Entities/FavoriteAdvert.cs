using Cirrious.MvvmCross.ViewModels;

namespace JR.DL.Model.Entities
{
    public class FavoriteAdvert
        : MvxNotifyPropertyChanged
    {
        public Advert Advert { get; set; }
        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                if (_isFavorite == value)
                    return;
                _isFavorite = value;
                RaisePropertyChanged("IsFavorite");
            }
        }
    }
}