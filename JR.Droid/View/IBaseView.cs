using JR.Core.ViewModels;
using Cirrious.MvvmCross.Views;

namespace JR.Droid.Views
{
    public interface IBaseView<TViewModel>
        : IMvxView
        where TViewModel : BaseViewModel
    {
        // just a marker interface
    }
}