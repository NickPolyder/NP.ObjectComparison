using System;

namespace NP.ObjectComparison.Analyzers.Settings
{
	/// <summary>
	/// Settings associated with <see cref="AnalyzerBuilder{T}"/>.
	/// </summary>
	public class AnalyzerSettings
	{
		private static readonly AnalyzerSettings _defaultSettings = new AnalyzerSettings();
		/// <summary>
		/// A factory method that is responsible to create an initial instance of <see cref="AnalyzerSettings"/>.
		/// </summary>
		public static Func<AnalyzerSettings> DefaultSettings { get; set; } = new Func<AnalyzerSettings>(() => _defaultSettings);
		/// <summary>
		/// Depth settings.
		/// </summary>
		public DepthSettings Depth { get; set; } = new DepthSettings();

		/// <summary>
		/// Skip analyze settings.
		/// </summary>
		public IgnoreSettings IgnoreSettings { get; set; } = new IgnoreSettings();
	}
}