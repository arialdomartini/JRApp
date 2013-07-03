using System;
using System.Collections.Generic;

namespace JR.SL.DTO.Raw
{
	public class TrackingItem
	{
		public string domEventType { get; set; }
		public List<string> pathElements { get; set; }
		public string targetSelector { get; set; }
	}
}