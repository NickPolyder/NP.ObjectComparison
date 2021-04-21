namespace ObjectPatcher
{
	public delegate bool EqualsPredicate<in TObject>(TObject original, TObject target);
}