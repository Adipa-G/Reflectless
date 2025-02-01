using System.Reflection;
using BenchmarkDotNet.Attributes;
using ReflectlessBenchmark.TestClasses;

public class BenchmarkPropertySet
{
    private IList<TestDto> _testDtoList;
    
    private MethodInfo PropertySetMethod { get; set; }
    private Action<object,object> PropertySetAccessor { get; set; }

    public BenchmarkPropertySet()
    {
        var property = typeof(TestDto).GetProperty(nameof(TestDto.IntProperty))!;
        PropertySetMethod = property.SetMethod!;
        PropertySetAccessor = Reflectless.Reflectless.GetPropertySetAccessor(typeof(TestDto), nameof(TestDto.IntProperty));

        _testDtoList = new List<TestDto>();
    }

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < N; i++)
        {
            _testDtoList.Add(new TestDto() { IntProperty = 0 });
        }
    }

    [Benchmark]
    public void Reflection_SetProperty()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            PropertySetMethod.Invoke(testDto, new object[] { index });
            if (testDto.IntProperty != index)
                throw new InvalidOperationException("Unknown error");
        }
    }

    [Benchmark]
    public void Reflectless_SetProperty()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = _testDtoList[index];
            PropertySetAccessor(testDto, index);
            if (testDto.IntProperty != index)
                throw new InvalidOperationException("Unknown error");
        }
    }
}