using System.Reflection;
using BenchmarkDotNet.Attributes;
using ReflectlessBenchmark.TestClasses;

public class BenchmarkPropertyGet
{
    private IList<TestDto> _testDtoList;
    
    private MethodInfo PropertyGetMethod { get; set; }
    private Func<object,object> PropertyGetAccessor { get; set; }

    public BenchmarkPropertyGet()
    {
        var property = typeof(TestDto).GetProperty(nameof(TestDto.IntProperty))!;
        PropertyGetMethod = property.GetMethod!;
        PropertyGetAccessor = Reflectless.Reflectless.GetPropertyGetAccessor(typeof(TestDto), nameof(TestDto.IntProperty));

        _testDtoList = new List<TestDto>();
    }

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < N; i++)
        {
            _testDtoList.Add(new TestDto() { IntProperty = i });
        }
    }

    [Benchmark]
    public void Reflection_GetProperty()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            var result = PropertyGetMethod.Invoke(testDto, null);
            if ((int)(result ?? 0) != index)
                throw new InvalidOperationException("Unknown error");
        }
    }

    [Benchmark]
    public void Reflectless_GetProperty()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            var result = PropertyGetAccessor(testDto);
            if ((int)(result ?? 0) != index)
                throw new InvalidOperationException("Unknown error");
        }
    }
}