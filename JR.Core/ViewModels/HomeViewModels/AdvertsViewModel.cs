using System.Windows.Input;
using JR.Core.ViewModels.AdvertLists;
using Cirrious.MvvmCross.ViewModels;

namespace JR.Core.ViewModels.HomeViewModels
{
    public class AdvertsViewModel
        : BaseJRViewModel
    {
        public ICommand ShowDayCommand
        {
            get { return new MvxCommand<string>((day) => ShowViewModel<AdvertListViewModel>(new {dayOfMonth = int.Parse(day)})); }
        }

        private ICommand MakeDayCommand(int whichDayOfMonth)
        {
            return new MvxCommand(() => ShowViewModel<AdvertListViewModel>(new { dayOfMonth = whichDayOfMonth }));
        }
		private string _searchTerm;
		public string SearchTerm
		{
			get { return _searchTerm; }
			set { _searchTerm = value; RaisePropertyChanged(() => SearchTerm);;
			}
		}

		private string _searchWhereTerm;
		public string SearchWhereTerm
		{
			get { return _searchWhereTerm; }
			set { _searchWhereTerm = value; RaisePropertyChanged(() => SearchWhereTerm);
			}
		}


		public ICommand GoParameterizedCommand
		{
			get
			{

				return
					new MvxCommand(
						() => ShowViewModel<AdvertListViewModel>(new AdvertListViewModel.Parameters(SearchTerm, SearchWhereTerm )));
						}
		}

    }
}