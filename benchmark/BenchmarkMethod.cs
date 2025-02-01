using System.Reflection;
using BenchmarkDotNet.Attributes;
using ReflectlessBenchmark.TestClasses;

public class BenchmarkMethod
{
    private IList<TestDto> _testDtoList;
    
    private MethodInfo MethodInfo { get; set; }
    private Func<object, object, object> MethodAccessor { get; set; }

    public BenchmarkMethod()
    {
        MethodInfo = typeof(TestDto).GetMethod(nameof(TestDto.AddInt), new Type[] { typeof(int) });
        MethodAccessor = Reflectless.Reflectless.GetMethodAccessor<Func<object, object, object>>(typeof(TestDto),
            nameof(TestDto.AddInt), typeof(int));

        _testDtoList = new List<TestDto>();
    }

    [Params(1000,10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        for(int i = 0; i < N; i++)
        {
            _testDtoList.Add(new TestDto() { IntProperty = i });
        }
    }

    [Benchmark]
    public void Reflection_Method()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var result = (int)MethodInfo.Invoke(_testDtoList[index], new object[] { index });
            if (result != 2 * index)
            {
                throw new InvalidOperationException("Unknown error");
            }
        }
    }

    [Benchmark]
    public void Reflectless_Method()
    {
        for (var index = 0; index < _testDtoList.Count; index++)
        {
            var result = (int)MethodAccessor(_testDtoList[index], index);
            if (result != 2 * index)
            {
                throw new InvalidOperationException("Unknown error");
            }
        }
    }
}