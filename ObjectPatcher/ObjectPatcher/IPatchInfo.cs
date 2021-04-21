namespace ObjectPatcher
{
	public interface IPatchInfo : IPatchInfo<object>
	{
	}

	public interface IPatchInfo<in TObject>
	{
		bool Patch(TObject originalInstance, TObject targetInstance);
	}
}