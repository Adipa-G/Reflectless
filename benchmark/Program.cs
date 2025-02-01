using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

var config = ManualConfig.Create(DefaultConfig.Instance)
    .WithOption(ConfigOptions.JoinSummary, true)
    .WithOption(ConfigOptions.DisableLogFile, true)
    .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Alphabetical));

config.AddJob(Job.Default.WithInvocationCount(4096).WithIterationCount(16));
config.AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig()));

BenchmarkRunner.Run(new[]{
    BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkPropertyGet), config),
    BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkPropertySet), config),
    BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkFieldGet), config),
    BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkFieldSet), config),
    BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkConstructor), config),
    BenchmarkConverter.TypeToBenchmarks( typeof(BenchmarkMethod), config),
});