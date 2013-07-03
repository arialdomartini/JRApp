using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JR.Core.Converters;
using JR.Core.ViewModels.Helpers;
using JR.Core.ViewModels.AdvertLists;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Drawing;
using BigTed;



namespace JR.UI.Touch.Views.AdvertLists
{
    public class BaseAdvertListView<TViewModel, TKey>
		: MvxTableViewController
        where TViewModel : BaseReloadingAdvertListViewModel<TKey>
    {
        private UIActivityIndicatorView _activityView;

		public new TViewModel ViewModel {
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			var converter = typeof (TKey) == typeof (DateTime)
				? new ShortDateValueConverter()
					: null;
			var source = new TableSource(converter, TableView, ViewModel, TabBarController, NavigationController);




			//TabBarController.TabBar.Hidden = true;
			//NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);

           
            this.AddBindings(new Dictionary<object, string>()
		                         {
		                             {source, "ItemsSource GroupedList"},
		                         });

			TableView.AutosizesSubviews = true;

            TableView.BackgroundColor = UIColor.White;
            TableView.RowHeight = 126;
            TableView.Source = source;
			//TableView.Frame = new System.Drawing.RectangleF (0, 100, View.Frame.Size.Width, View.Frame.Size.Height-100);
			/*
			System.Drawing.RectangleF tableFrame;
			tableFrame.X = 0;
			tableFrame.Y = 0;
			tableFrame.Size.Width = View.Frame.Size.Width;
			tableFrame.Size.Height = View.Frame.Size.Height - 0;
			TableView.Frame = tableFrame;
			*/

            TableView.ReloadData();
        }

		public override void ViewDidDisappear (bool animated)
		{	if (NavigationController != null) {
				NavigationController.SetNavigationBarHidden (false, true);
			}
			if (TabBarController != null) {
				TabBarController.TabBar.Hidden = false;
			}
			base.ViewDidDisappear (animated);
		}


        private class TableSource : MvxBaseTableViewSource
        {
			TViewModel _tvm;
			private float _position;
            private readonly IMvxValueConverter _keyConverter;
			private UITabBarController _tabbarc;
			private UINavigationController _uinav;
			private bool _hiddenTabBar;
            public TableSource(IMvxValueConverter keyConverter, UITableView tableView, TViewModel tvm, UITabBarController tabbarc,UINavigationController uinav)
                : base(tableView)
            {
                _keyConverter = keyConverter;
				_tvm =  tvm;
				_tabbarc = tabbarc;
				_uinav = uinav;
            }

            private IList<BaseAdvertListViewModel<TKey>.AdvertGroup> _AdvertGroups;
            public IList<BaseAdvertListViewModel<TKey>.AdvertGroup> ItemsSource
            {
                get
                {
                    return _AdvertGroups;
                }
                set 
                { 
                    _AdvertGroups = value;
                    ReloadTableData();

                }
            }
			
			public override string TitleForHeader(UITableView tableView, int section)
			{
		       if (_AdvertGroups == null)
                    return string.Empty;

               return KeyToString(_AdvertGroups[section].Key);
         	}

            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {

                return 126;
            }
			public override void Scrolled (UIScrollView scrollView)
			{
				if (_position +5< scrollView.ContentOffset.Y && !_hiddenTabBar) {
					ResizeView (true);
				} else if(_position > scrollView.ContentOffset.Y+5 && _hiddenTabBar) {
					ResizeView (false);
				}

				_position = scrollView.ContentOffset.Y;
			}




            public override int NumberOfSections(UITableView tableView)
            {
                if (_AdvertGroups == null)
                    return 0;

                return _AdvertGroups.Count;
            }

            public override int RowsInSection(UITableView tableview, int section)
            {
                if (_AdvertGroups == null)
                    return 0;

                return _AdvertGroups[section].Count;
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
				var reuse = tableView.DequeueReusableCell(AdvertCell2.Identifier);
				if (reuse != null)
					return reuse;

				var cell = AdvertCell2.LoadFromNib(tableView);

				cell._firstColor = new UIColor(85.0F / 255.0F , 213.0F / 255.0F,80.0F / 255.0F, 1.0F);
				cell._secondColor = new UIColor(232.0F / 255.0F, 61.0F / 255.0F,14.0F / 255.0F,1.0F);
				cell._thirdColor = new UIColor(254.0F / 255.0F , 217.0F / 255.0F , 56.0F / 255.0F ,1.0F);
				cell._fourthColor = new UIColor(206.0F / 255.0F , 149.0F / 255.0F,98.0F / 255.0F, 1.0F);

				return cell;


				/*
				 * 
				 * 

                var reuse = tableView.DequeueReusableCell(AdvertCell2.Identifier);
                if (reuse != null)
                    return reuse;

                var cell = AdvertCell2.LoadFromNib(tableView);
                return cell;
                
				 * 
				 * 
				var cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];

				if (!cell) {
					cell = [[MCSwipeTableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:CellIdentifier];
				}

				[cell setDelegate:self];
				[cell setFirstStateIconName:@"check.png"
				 firstColor:[UIColor colorWithRed:85.0 / 255.0 green:213.0 / 255.0 blue:80.0 / 255.0 alpha:1.0]
				 secondStateIconName:@"cross.png"
				 secondColor:[UIColor colorWithRed:232.0 / 255.0 green:61.0 / 255.0 blue:14.0 / 255.0 alpha:1.0]
				 thirdIconName:@"clock.png"
				 thirdColor:[UIColor colorWithRed:254.0 / 255.0 green:217.0 / 255.0 blue:56.0 / 255.0 alpha:1.0]
				 fourthIconName:@"list.png"
				 fourthColor:[UIColor colorWithRed:206.0 / 255.0 green:149.0 / 255.0 blue:98.0 / 255.0 alpha:1.0]];

				[cell.contentView setBackgroundColor:[UIColor whiteColor]];
				[cell setSelectionStyle:UITableViewCellSelectionStyleGray];

				if (indexPath.row % 2) {
					[cell.textLabel setText:@"Switch Mode Cell"];
					[cell.detailTextLabel setText:@"Swipe to switch"];
					[cell setMode:MCSwipeTableViewCellModeSwitch];
				}
				else {
					[cell.textLabel setText:@"Exit Mode Cell"];
					[cell.detailTextLabel setText:@"Swipe to delete"];
					[cell setMode:MCSwipeTableViewCellModeExit];
				}

				return cell;

				*/


            }

            public override string[] SectionIndexTitles(UITableView tableView)
            {
                if (_AdvertGroups == null)
                    return base.SectionIndexTitles(tableView);

                return _AdvertGroups.Select(x => KeyToString(x.Key, 10)).ToArray();
            }

            private string KeyToString(TKey key, int maxLength)
            {
				var candidate = KeyToString(key);
				if (candidate.Length > maxLength)
				{
					candidate = candidate.Substring(0,maxLength);
				}
				return candidate;
            }
			
			private string KeyToString(TKey key)
            {
                if (_keyConverter == null)
                    return key.ToString();

                return (string) _keyConverter.Convert(key, typeof (string), null, CultureInfo.CurrentUICulture);
            }

            protected override object GetItemAt(NSIndexPath indexPath)
            {
                if (_AdvertGroups == null)
                    return null;

                return _AdvertGroups[indexPath.Section][indexPath.Row];
            }
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (BTProgressHUD.IsVisible) {
					BTProgressHUD.ShowSuccessWithStatus("Trovati!");

					BTProgressHUD.Dismiss();

				}
				if (!_tvm.Service.isFinished && !_tvm.GetType().ToString().Contains("Favorites") && tableView.ContentOffset.Y>0) {
					float actualPosition = tableView.ContentOffset.Y;
					float contentHeight = tableView.ContentSize.Height - 700;
					if (actualPosition >= contentHeight) {
						BTProgressHUD.Show("Sto cercando altri lavori per te...");
						_tvm.GoNextPage ();
						tableView.ReloadData ();
					

					}
				}
				return base.GetCell (tableView, indexPath);
			}


			public void ResizeView(bool toResize){
	
				RectangleF newFrameTab = _tabbarc.View.Frame;
				RectangleF newFrameNav = _uinav.View.Frame;


				newFrameTab.Height = toResize ? newFrameTab.Height + 49 : newFrameTab.Height - 49;
				newFrameNav.Height = toResize ? newFrameNav.Height + 44 : newFrameNav.Height - 44;

				_tabbarc.TabBar.Hidden = toResize;
				_uinav.SetNavigationBarHidden (toResize, true);
	
				foreach (UIView view in _tabbarc.View.Subviews) {

						if (view.GetType ().ToString ().Equals ("MonoTouch.UIKit.UITabBar")) {
							//vecchio iphonz
							//view.Frame = toResize ? new RectangleF(0,0,0,0) : new RectangleF(0,431,320,49);
							//view.Frame = toResize ? new RectangleF(0,0,0,0) : new RectangleF(0,607,320,49);

						} else {
							view.Frame= newFrameTab;
						}
					}    
				foreach (UIView view in _uinav.View.Subviews) {
					if (view.GetType ().ToString ().Equals ("MonoTouch.UIKit.UINavigationBar")) {

						view.Frame = toResize ? new RectangleF(0,0,0,0) : new RectangleF(0,20,320,44);
					} else {
						view.Frame = newFrameNav;
					}
				}  
				_hiddenTabBar = toResize;

			}
        }
    }
}