using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Blazor.WebAssembly
{
	public class SiteSet : ICloneable
	{
		public string SavedName { get; set; }
		public List<string> Sites { get; set; } = new List<string>();
		public bool Public { get; set; }
		public string CreatedUser { get; set; }

		public object Clone()
		{
			var forRet = (SiteSet)this.MemberwiseClone();
			forRet.Sites = Sites.Select(x => x).ToList();
			return forRet;
		}
	}
}