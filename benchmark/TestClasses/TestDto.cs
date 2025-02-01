namespace ReflectlessBenchmark.TestClasses
{
    public class TestDto
    {
        public TestDto()
        {
        }

        public TestDto(int intField)
        {
            _intField = intField;
        }

        public int IntProperty { get; set; }

        // ReSharper disable once InconsistentNaming
        public int _intField;

        public int AddInt(int addThis)
        {
            return IntProperty + addThis;
        }
    }
}
