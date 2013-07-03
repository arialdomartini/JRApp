using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using JR.DL.Model.Entities;
using JR.SL.Service.Interfaces;
using Cirrious.MvvmCross.Plugins.Messenger;
using JR.SL.Service.Messages;
using Cirrious.MvvmCross.Platform;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.File;
using JR.SL.DTO.Raw;

namespace JR.SL.Service.Impl
{
    public class JRService 
        : IJRService
        
    {
        private readonly FavoritesSaver _favoritesSaver = new FavoritesSaver();
		private string _where;
		private string _what;
		private int _page;
        // is loading setup
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set 
            { 
                _isLoading = value;
                FireLoadingChanged();
            }
        }

        private void FireLoadingChanged()
        {
			FireMessage(new LoadingChangedMessage(this));
        }
 
        // the basic lists
        public IDictionary<string, FavoriteAdvert> Adverts { get; private set; }
		private string CookieToUse { get; set;}
		public List<Advert> Items { get;  set; }
		public bool isFinished { get; private set; }



        // a hashtable of favorites
        private IDictionary<string, FavoriteAdvert> _favoriteAdverts;
        public IDictionary<string,FavoriteAdvert> GetCopyOfFavoriteAdverts()
        {
            lock (this)
            {
				if (_favoriteAdverts == null)
					return new Dictionary<string, FavoriteAdvert>();
				
                var toReturn = new Dictionary<string, FavoriteAdvert>(_favoriteAdverts);
                return toReturn;
            }
        }
		 
        private void FireFavoriteAdvertsChanged()
        {
			FireMessage(new FavoritesChangedMessage(this));
        }

		private void FireMessage(MvxMessage message)
		{
			var messenger = Mvx.Resolve<IMvxMessenger>();
			messenger.Publish(message);
		}

        public void BeginAsyncLoad()
        {
            IsLoading = true;
            MvxAsyncDispatcher.BeginAsync(Load);
        }

		public void BeginAsyncLoadAdvert()
		{
			IsLoading = true;
			MvxAsyncDispatcher.BeginAsync(LoadAdvert);
		}
		
		public void DoSyncLoad()
        {
            IsLoading = true;
            Load();
        }

		public void DoSyncLoadAdvert(string what, string where, int page)
		{
			//IsLoading = true;
			Load(what, where, page);
		}

		private void Load(string what, string where, int page)
		{
		
			if (_what != what || _where != where) {
				Items = null;
				Adverts = null;
			}
			_what = what;
			_where = where;
			_page = page;
			LoadAdverts (what, where, page);

			//IsLoading = false;
		}	

		public void LoadAdvert(){
			_page = _page+1;
			LoadAdverts (_what, _where, _page);
		}


        private void Load()
        {
            //LoadAdverts();
            LoadFavorites();
            IsLoading = false;
        }		

        private void LoadFavorites()
        {
            lock (this)
            {
                _favoriteAdverts = new Dictionary<string, FavoriteAdvert>();
            }
            FireFavoriteAdvertsChanged();

            var files = Mvx.Resolve<IMvxFileStore>();
            string json;
            if (!files.TryReadTextFile(Constants.FavoritesFileName, out json))
                return;

            var jsonConvert = Mvx.Resolve<IMvxJsonConverter>();
            var parsedKeys = jsonConvert.DeserializeObject<List<string>>(json);
            if (parsedKeys != null && Adverts!=null)
            {
                foreach (var key in parsedKeys)
                {
                    FavoriteAdvert Advert;
                    if (Adverts.TryGetValue(key, out Advert))
                    Advert.IsFavorite = true;
                }
            }
        }


		private void LoadAdverts(string whatFor, string whereFor, int page)
        {
			string content="";
	
			if (page == 1) {
				Items = new List<Advert> ();
			}
			if (whatFor == "") {
				whatFor ="_empty";
			}
			
			if (whereFor == "") {
				whereFor ="_empty";
			}
			var jsonConvert = Mvx.Resolve<IMvxJsonConverter>();
			//page++;

			string address = "http://it.jobrapido.com/fo/api/v1/search/"+whatFor+"/"+whereFor+"/_empty/" + Convert.ToString(page);

			if (CookieToUse == null) {
				GenerateCookie ();
			}

			HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;  
			request.Method = "GET";
			request.Accept = "application/json";

			request.AllowAutoRedirect =true;
			request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0";
			request.Headers.Add(HttpRequestHeader.Cookie, CookieToUse);

			//request.Headers.Add(HttpRequestHeader.Cookie, @"__gads=ID=01fa5343a3bd25cd:T=1368986455:S=ALNI_MaG-DG1dYHtNEUz6sKsFL5ThOHlPQ; __utma=258773842.1530730123.1355866582.1370784966.1371808885.25; __utmb=258773842.1.10.1371808885; __utmc=258773842; __utmz=258773842.1358888724.6.2.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided); __utmv=258773842.|5=%3B=_=1; JE=""02*RHtNsCk46YMSzFFQ4Yaq/okH7psoVqLLP6zW+R7/wkScCxQVHeiLjwvkkzZ7pwCElR0rAg6M2DBEjYLQ2thglpa2tU71KkT8n13HeKbSpL6rX+WDd5noUk2eBAXWduFblIehGuwsiSBEE0nNQoXlTXft4X06jiXIoEv9+xW8h2fOgMm0CVJ9cBWnzPRY2hNy3HiY7kz1mUEsyzd4fErqyJpm7dJOLU+jARu90qSkHdMCPOP+h/j5cPcMtV1zjuMGjZlk4Vo++a/jVVBcD1rEEmMMVAxs5Yn291Q85oCHi1Pw/Qkdmq0z4mm1HPw4g4+iznVIq5gz5g96RrFFOwTCQ9rPnAmYGOWoYm5fGZrD3jmoe99FQYq14E3tUQXp/YoFgYqQGyqsMTyVbvT9id8kOwB+bQErWsZ33kDBMbYmP+W+iTTWbtK2dtavZfZwFxvukFtl6iHoAW/2+24TY+ZI7eUj6Vvc5fgRmOCiGmIe0tkUPENd6fq44BLN3QFBp77DlSDDW02yYdgzNXwVafKmG3Ne8Oyw9Owq5do8Kl73BEUj96O4BYY7kg7qtUjPQKKi""");

			// Get response  

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {  
				// Get the response stream  
				StreamReader reader = new StreamReader (response.GetResponseStream());  
				content = reader.ReadToEnd();

			}

			SearchItem itemssr = jsonConvert.DeserializeObject<SearchItem>(content);


			if (itemssr != null && itemssr.searchResults != null) {
	
				foreach (SearchResult item in  itemssr.searchResults) {
					Advert ss = new Advert ();
					//id spesso era uguale perciò ho salvato questa concatenazione
					ss.Key = Convert.ToString (item.id) + "|" +item.title; // RandomDay ().ToString ();
					ss.Description = item.openAdvertUrl.Contains ("http") ? item.openAdvertUrl : "http://it.jobrapido.com" + item.openAdvertUrl;
					ss.Level = item.showNewMarker ? "*new*" : "";

					ss.SpeakerKey = item.company;
					ss.Type = item.salary;

					ss.Title = item.title;
					ss.Where = item.location;
					DateTime dt = DateTime.Today;
					if (!item.date.Contains ("sponsorizzato")) {
				
						string[] words = item.date.Split (' ');
						if (words.Count () > 0) { 
							// che merda
							dt = new DateTime (Convert.ToInt32(words [2]), FindMonth (words[1]), Convert.ToInt32 (words [0]));

						}
					} else {
						ss.Level = "*sponsorizzato*";
					}
					ss.When = dt;
					TimeSpan span = DateTime.Today.Subtract (dt);
					Items.Add (ss);
			
				}

			
			} else {
				isFinished = true;
			}


			Adverts = Items.Select(x => new FavoriteAdvert()
                                                  {
                                                      Advert = x,
                                                      IsFavorite = false
                                                  })
                .ToDictionary(x => x.Advert.Key, x => x);

            foreach (var FavoriteAdvert in Adverts.Values)
            {
                FavoriteAdvert.PropertyChanged += AdvertWithFavoriteFlagOnPropertyChanged;            
            }
			LoadFavorites ();
        }

        private void AdvertWithFavoriteFlagOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "IsFavorite")
                return;

            var Advert = (FavoriteAdvert)sender;
            lock (this)
            {
                if (_favoriteAdverts == null)
                    return;

                if (Advert.IsFavorite)
                {
                    _favoriteAdverts[Advert.Advert.Key] = Advert;
                }
                else
                {
                    if (_favoriteAdverts.ContainsKey(Advert.Advert.Key))
                        _favoriteAdverts.Remove(Advert.Advert.Key);
                }

				_favoritesSaver.RequestAsyncSave(_favoriteAdverts);
            }

            FireFavoriteAdvertsChanged();
        }


		public void GenerateCookie(){
			string homepage = "http://it.jobrapido.com";
			HttpWebRequest requests = WebRequest.Create(homepage) as HttpWebRequest;  
			requests.Method = "GET";
			requests.Accept = "application/json";
			requests.AllowAutoRedirect = false;
			requests.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0";
			using (HttpWebResponse response = requests.GetResponse() as HttpWebResponse) {  
				// Get the response stream  
				CookieToUse = response.Headers["Set-Cookie"];

			}
		}

		private int  FindMonth(string mese){
		
			int iValue =0;
			switch (mese)
			{
				case "gennaio":
					iValue = 1;
					break;
				case "febbraio":
					iValue = 2;
					break;			
				case "marzo":
					iValue = 3;
					break;
				case "aprile":
					iValue = 4;
					break;
				case "maggio":
					iValue = 5;
					break;
				case "giugno":
					iValue = 6;
					break;
				case "luglio":
					iValue = 7;
					break;
				case "agosto":
					iValue = 8;
					break;
				case "settembre":
					iValue = 9;
					break;
				case "ottobre":
					iValue = 10;
					break;
				case "novembre":
					iValue = 11;
					break;
				case "dicembre":
					iValue = 12;
					break;

			}
			return iValue;
		}
    }
}
