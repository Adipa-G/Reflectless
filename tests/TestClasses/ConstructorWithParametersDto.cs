namespace ReflectlessTests.TestClasses
{
    public class ConstructorWithParametersDto
    {
        public string Name1 { get; }

        public string Name2 { get; }
        
        public ConstructorWithParametersDto(string name1)
        {
            Name1 = name1;
            Name2 = "P";
        }

        public ConstructorWithParametersDto(string name1, string name2)
        {
            Name1 = name1;
            Name2 = name2;
        }
    }
}
