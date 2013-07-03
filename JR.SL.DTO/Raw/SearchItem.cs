using System.Collections.Generic;

namespace JR.SL.DTO.Raw
{
    public class SearchItem
    {
		public string what { get; set; }
		public string location { get; set; }
		public string radius { get; set; }
		public string page { get; set; }
		public int itemsCount  { get; set; }
		public int totalItemsFound { get; set; }
		public int totalPagesFound { get; set; }
		public List<SearchResult> searchResults { get; set; }
	
		public bool isErrorPage { get; set; }
		public string hasNextPage { get; set; }
		public string trackGAPageViewSnippet { get; set; }
		public List<TrackingItem> trackingBindings { get; set; }

        public override string ToString()
        {
			return searchResults.Count.ToString();
        }
    }
}