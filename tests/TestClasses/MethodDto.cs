namespace ReflectlessTests.TestClasses
{
    public class MethodDto
    {
        public bool MethodCalled { get; set; }

        public void MethodWithNoParametersNoReturns()
        {
            MethodCalled = true;
        }

        public int MethodWithNoParametersHasReturns()
        {
            MethodCalled = true;
            return 10;
        }

        public int MethodWithParametersAndReturns(string value)
        {
            MethodCalled = true;
            return value.Length;
        }
    }
}
