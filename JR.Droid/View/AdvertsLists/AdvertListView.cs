using System;
using Android.App;
using JR.Core.ViewModels.AdvertLists;

namespace JR.Droid.Views.AdvertsLists
{
	[Activity(Label = "Jobrapido")]
    public class AdvertListView 
        : BaseAdvertListView<AdvertListViewModel, DateTime>
    {
    }
}