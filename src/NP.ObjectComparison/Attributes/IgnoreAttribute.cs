using System;

namespace NP.ObjectComparison.Attributes
{
	/// <summary>
	/// Allows a class or a property to be ignored when analyzed.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
	public class IgnoreAttribute : Attribute
	{

	}
}