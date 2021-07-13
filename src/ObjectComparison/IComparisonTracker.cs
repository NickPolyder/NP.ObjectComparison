namespace ObjectComparison
{
	public interface IComparisonTracker<TObject>
	{
		TObject Original { get; }

		TObject Current { get; set; }

		void Analyze();

		bool HasChanges();

		bool IsPatched();

		void Patch();

		void Reset();
	}
}