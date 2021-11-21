using System;
using System.Collections.Generic;

namespace ObjectComparison
{
	/// <summary>
	/// Constants related to this project.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// The Dictionary interface type.
		/// </summary>
		public static readonly Type DictionaryInterfaceType = typeof(IDictionary<,>);


		/// <summary>
		/// Maximum Reference Depth for objects.
		/// </summary>
		public const int MaxReferenceLoopDepth = 5;
	}
}