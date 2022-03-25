using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using NP.ObjectComparison.Analyzers;
using NP.ObjectComparison.Analyzers.Settings;

namespace NP.ObjectComparison.Benchmarks;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
public class BenchmarkAnalyzerBuilder
{
	private static readonly AnalyzerSettings _analyzerOptions;

	static BenchmarkAnalyzerBuilder()
	{
		_analyzerOptions =  AnalyzerSettings.DefaultSettings.Invoke()
		                    ?? new AnalyzerSettings();
	}

	[Benchmark(Baseline = true)] // mark this is as baseline method.
	public void FirstBuild()
	{
		AnalyzerBuilder<BenchmarkObject>.Build().ToArray();
	}

	[Benchmark]
	public void WithAnalyzerOptions()
	{
		AnalyzerBuilder<BenchmarkObject>.Build(_analyzerOptions).ToArray();
	}

	[GlobalSetup(Targets = new[] { nameof(WhenAlreadyInitializedOnce), nameof(WhenAlreadyInitializedOnceAndOptions) })]
	public void WhenAlreadyInitializedOnceSetup()
	{
		AnalyzerBuilder<BenchmarkObject>.Build().ToArray();
	}
	
	[Benchmark]
	public void WhenAlreadyInitializedOnce()
	{
		AnalyzerBuilder<BenchmarkObject>.Build().ToArray();
	}

	[Benchmark]
	public void WhenAlreadyInitializedOnceAndOptions()
	{
		AnalyzerBuilder<BenchmarkObject>.Build(_analyzerOptions).ToArray();
	}
}