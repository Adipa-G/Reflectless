using System.Reflection;
using BenchmarkDotNet.Attributes;
using ReflectlessBenchmark.TestClasses;

public class BenchmarkFieldGet
{
    private IList<TestDto> _testDtoList;
    
    private FieldInfo FieldInfo { get; set; }
    private Func<object,object> FieldGetAccessor { get; set; }

    public BenchmarkFieldGet()
    {
        FieldInfo = typeof(TestDto).GetField(nameof(TestDto._intField))!;
        FieldGetAccessor = Reflectless.Reflectless.GetFieldGetAccessor(typeof(TestDto), nameof(TestDto._intField));

        _testDtoList = new List<TestDto>();
    }

    [Params(1000,10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < N; i++)
        {
            _testDtoList.Add(new TestDto() { _intField = i });
        }
    }

    [Benchmark]
    public void Reflection_GetField()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            var result = FieldInfo.GetValue(testDto);
            if ((int)(result ?? 0) != index)
                throw new InvalidOperationException("Unknown error");
        }
    }

    [Benchmark]
    public void Reflectless_GetField()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            var result = FieldGetAccessor(testDto);
            if ((int)(result ?? 0) != index)
                throw new InvalidOperationException("Unknown error");
        }
    }
}