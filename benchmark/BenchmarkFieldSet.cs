using System.Reflection;
using BenchmarkDotNet.Attributes;
using ReflectlessBenchmark.TestClasses;

public class BenchmarkFieldSet
{
    private IList<TestDto> _testDtoList;
    
    private FieldInfo FieldInfo { get; set; }
    private Action<object,object> FieldSetAccessor { get; set; }

    public BenchmarkFieldSet()
    {
        FieldInfo = typeof(TestDto).GetField(nameof(TestDto._intField))!;
        FieldSetAccessor = Reflectless.Reflectless.GetFieldSetAccessor(typeof(TestDto), nameof(TestDto._intField));

        _testDtoList = new List<TestDto>();
    }

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < N; i++)
        {
            _testDtoList.Add(new TestDto() { _intField = 0 });
        }
    }

    [Benchmark]
    public void Reflection_SetField()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            FieldInfo.SetValue(testDto, index);
            if (testDto._intField != index)
                throw new InvalidOperationException("Unknown error");
        }
    }

    [Benchmark]
    public void Reflectless_SetField()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            FieldSetAccessor(testDto, index);
            if (testDto._intField != index)
                throw new InvalidOperationException("Unknown error");
        }
    }
}