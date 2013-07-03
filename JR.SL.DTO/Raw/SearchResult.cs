using System.Collections.Generic;

namespace JR.SL.DTO.Raw
{
	public class SearchResult
    {
		public string company { get; set; }
		public string date { get; set; }
		public int id { get; set; }
        public string location { get; set; }
		public string openAdvertUrl { get; set; }
		public string salary { get; set; }
		public bool showNewMarker { get; set; }
		public string title { get; set; }
		public int trackClickAd { get; set; }
		public string trackGAJobClickEventSnippet { get; set; }
		public string url { get; set; }
		public string website { get; set; }
	}
}