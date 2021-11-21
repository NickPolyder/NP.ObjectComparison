namespace ObjectComparison
{
	/// <summary>
	/// A signature for Equality helpers.
	/// </summary>
	/// <typeparam name="TObject"></typeparam>
	/// <param name="original"></param>
	/// <param name="target"></param>
	/// <returns>true when <paramref name="original" /> and  <paramref name="target" /> are equal.</returns>
	public delegate bool EqualsPredicate<in TObject>(TObject original, TObject target);
}