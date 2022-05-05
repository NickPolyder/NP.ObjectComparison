using System;
using System.Diagnostics.CodeAnalysis;

namespace NP.ObjectComparison.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	[ExcludeFromCodeCoverage]
	public class ObjectComparisonException: Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public ObjectComparisonException() : base() { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ObjectComparisonException(string message) : base(message) { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ObjectComparisonException(string message, Exception inner) : base(message, inner) { }

		// A constructor is needed for serialization when an
		// exception propagates from a remoting server to the client.
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ObjectComparisonException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}