using System;

namespace ObjectComparison.Exceptions
{
	[Serializable]
	public class ObjectComparisonException: Exception
	{
		public ObjectComparisonException() : base() { }
		public ObjectComparisonException(string message) : base(message) { }
		public ObjectComparisonException(string message, Exception inner) : base(message, inner) { }

		// A constructor is needed for serialization when an
		// exception propagates from a remoting server to the client.
		protected ObjectComparisonException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}