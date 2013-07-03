namespace JR.Core.ViewModels.AdvertLists
{
    public abstract class BaseReloadingAdvertListViewModel<T>
        : BaseAdvertListViewModel<T>
    {
        public override void Start()
        {
            LoadAdverts();
            base.Start();
        }

        protected override void RepositoryOnLoadingChanged()
        {
            LoadAdverts();
            base.RepositoryOnLoadingChanged();
        }

        protected abstract void LoadAdverts();
		public abstract void GoNextPage();


    }
}