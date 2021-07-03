using System;
using System.Collections.Generic;

namespace ObjectComparison
{
	public static class Constants
	{
		public static readonly Type DictionaryInterfaceType = typeof(IDictionary<,>);


		public const int MaxReferenceLoopDepth = 5;
	}
}