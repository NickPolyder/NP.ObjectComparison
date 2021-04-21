namespace ObjectPatcher
{
	public interface IPatchInfo<in TInstance>
	{
		bool Patch(TInstance originalInstance, TInstance targetInstance);
	}
}