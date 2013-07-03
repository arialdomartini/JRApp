using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using JR.Core;
using JR.Core.Converters;
using JR.Core.ViewModels;
using JR.Core.ViewModels.Helpers;
using JR.Core.ViewModels.AdvertLists;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using Android.Views;


namespace JR.Droid.Views.AdvertsLists
{
	[Activity(Label = "Jobrapido")]
	public class BaseAdvertListView<TViewModel, TKeyType>: MvxActivity
        where TViewModel : BaseAdvertListViewModel<TKeyType>
    {
        protected sealed override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_AdvertList);

            //Find our list and set its adapter
            var AdvertListView = FindViewById<MvxListView>(Resource.Id.AdvertList);
            AdvertListView.Adapter = new GroupedListAdapter(this, (IMvxAndroidBindingContext)BindingContext, KeyValueConverter);
			AdvertListView.Scroll += (object sender, AbsListView.ScrollEventArgs e) => {
				//Console.WriteLine(e.FirstVisibleItem-e.VisibleItemCount);
				if(ViewModel is AdvertListViewModel){
					if(e.FirstVisibleItem >= AdvertListView.Count - e.VisibleItemCount && !((AdvertListViewModel)ViewModel).Service.isFinished) {
						((AdvertListViewModel)ViewModel).GoNextPage();
					}
				}
			};
        }


        protected virtual IMvxValueConverter KeyValueConverter
        {
            get
            {
                if (typeof(TKeyType) == typeof(DateTime))
                    return new SimpleDateValueConverter();

                // default behaviour is null - which means ToString() will be used
                return null;
            }
        }

        public class GroupedListAdapter
            : MvxAdapter, ISectionIndexer
        {
            private Java.Lang.Object[] _sectionHeaders;
            private List<int> _sectionLookup;
            private List<int> _reverseSectionLookup;

            private readonly IMvxValueConverter _keyConverter;

            public GroupedListAdapter(Context context, IMvxAndroidBindingContext bindingContext, IMvxValueConverter keyConverter)
                : base(context, bindingContext)
            {
                _keyConverter = keyConverter;
            }
	
            protected override void SetItemsSource(IEnumerable list)
            {
                var groupedList = list as List<BaseAdvertListViewModel<TKeyType>.AdvertGroup>;

                if (groupedList == null)
                {
                    _sectionHeaders = null;
                    _sectionLookup = null;
                    _reverseSectionLookup = null;
                    base.SetItemsSource(null);
                    return;
                }

                var flattened = new List<object>();
                _sectionLookup = new List<int>();
                _reverseSectionLookup = new List<int>();
                var sectionHeaders = new List<string>();

                var groupsSoFar = 0;
                foreach (var group in groupedList)
                {
                    _sectionLookup.Add(flattened.Count);
                    var groupHeader = GetGroupHeader(group);
                    sectionHeaders.Add(groupHeader);
                    for (int i = 0; i <= group.Count; i++)
                        _reverseSectionLookup.Add(groupsSoFar);

                    flattened.Add(groupHeader);
                    flattened.AddRange(group.Select(x => (object)x));

                    groupsSoFar++;
                }

                _sectionHeaders = CreateJavaStringArray(sectionHeaders.Select(x => x.Length > 10 ? x.Substring(0,10): x).ToList());

                base.SetItemsSource(flattened);
            }

            private string GetGroupHeader(BaseAdvertListViewModel<TKeyType>.AdvertGroup group)
            {
                if (_keyConverter == null)
                    return group.Key.ToString();

                return (string)_keyConverter.Convert(group.Key, typeof(string), null, CultureInfo.CurrentUICulture);
            }

            public int GetPositionForSection(int section)
            {
				if (_sectionLookup == null || section <=_sectionLookup.Count)
                    return 0;

                return _sectionLookup[section];
            }

            public int GetSectionForPosition(int position)
            {
                if (_reverseSectionLookup == null)
                    return 0;

                return _reverseSectionLookup[position];
            }

            public Java.Lang.Object[] GetSections()
            {
                return _sectionHeaders;
            }

            private static Java.Lang.Object[] CreateJavaStringArray(List<string> inputList)
            {
                if (inputList == null)
                    return null;

                var toReturn = new Java.Lang.Object[inputList.Count];
                for (var i = 0; i < inputList.Count; i++)
                {
                    toReturn[i] = new Java.Lang.String(inputList[i]);
                }

                return toReturn;
            }



            protected override global::Android.Views.View GetBindableView(global::Android.Views.View convertView, object dataContext, int templateId)
            {
				if (dataContext.GetType().ToString().Contains("FavoriteAdvert"))
                    return base.GetBindableView(convertView, dataContext, Resource.Layout.ListItem_Advert);
                else
                    return base.GetBindableView(convertView, dataContext, Resource.Layout.ListItem_SeparatorToString);
            }
        }
    }
}