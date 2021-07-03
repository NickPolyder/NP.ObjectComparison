namespace ObjectComparison
{
	public delegate bool EqualsPredicate<in TObject>(TObject original, TObject target);
}