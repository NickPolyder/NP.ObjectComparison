namespace ObjectPatcher
{
	public class PatchComposite<TInstance> : IPatchInfo<TInstance>
	{
		private readonly IPatchInfo<TInstance>[] _patchInfos;

		public PatchComposite(params IPatchInfo<TInstance>[] patchInfos)
		{
			_patchInfos = patchInfos;
		}

		public bool Patch(TInstance originalInstance, TInstance targetInstance)
		{
			var hasChanged = false;
			foreach (var patchInfo in _patchInfos)
			{
				hasChanged = patchInfo.Patch(originalInstance, targetInstance);
			}

			return hasChanged;
		}
	}
}