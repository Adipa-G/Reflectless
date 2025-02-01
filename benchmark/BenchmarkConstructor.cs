using System.Reflection;
using BenchmarkDotNet.Attributes;
using ReflectlessBenchmark.TestClasses;

public class BenchmarkConstructor
{
    private IList<TestDto> _testDtoList;
    
    private ConstructorInfo ConstructorInfo { get; set; }
    private Func<object, object> ConstructorAccessor { get; set; }

    public BenchmarkConstructor()
    {
        ConstructorInfo = typeof(TestDto).GetConstructor(new[] { typeof(int) });
        ConstructorAccessor = Reflectless.Reflectless.GetConstructorAccessor<Func<object,object>>(typeof(TestDto), new []{typeof(int)});
    }

    [Params(1000,10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _testDtoList = new List<TestDto>(N);
    }

    [Benchmark]
    public void Reflection_Construct()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = (TestDto)ConstructorInfo.Invoke(new object?[] { index });
            if (testDto == null)
            {
                throw new InvalidOperationException("Unknown error");
            }
            _testDtoList.Add(testDto);
        }
    }

    [Benchmark]
    public void Reflectless_Construct()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var testDto = (TestDto)ConstructorAccessor(index);
            if (testDto == null)
            {
                throw new InvalidOperationException("Unknown error");
            }
            _testDtoList.Add(testDto);
        }
    }
}