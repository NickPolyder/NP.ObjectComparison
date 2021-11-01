using System;

namespace ObjectComparison.Attributes
{
	/// <summary>
	/// Allows a class or a property to be skipped from analyzation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
	public class SkipAnalyzeAttribute : Attribute
	{

	}
}