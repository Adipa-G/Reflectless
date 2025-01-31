using ReflectlessTests.TestClasses;
using Xunit;

namespace ReflectlessTests
{
    public class ConstructorAccessTests
    {
        [Fact]
        public void Non_Generic_Default_Constructor_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetDefaultConstructorAccessor(typeof(ConstructorWithParametersDto)));

            Assert.Equal($"The default constructor in type {typeof(ConstructorWithParametersDto).FullName} does not exists.",
                exception.Message);
        }

        [Fact]
        public void Non_Generic_Default_Constructor_Initialise()
        {
            var accessor = Reflectless.Reflectless.GetDefaultConstructorAccessor(typeof(DefaultConstructorDto));

            var value = accessor() as DefaultConstructorDto;

            Assert.NotNull(value);
            Assert.Equal("X", value.Name);
        }

        [Fact]
        public void Non_Generic_Parameterised_Constructor_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetConstructorAccessor<Func<object,object>>(typeof(ConstructorWithParametersDto), typeof(int)));

            Assert.Equal($"The constructor with parameters [Int32] in type {typeof(ConstructorWithParametersDto).FullName} does not exists.",
                exception.Message);
        }

        [Fact]
        public void Non_Generic_Parameterised_Constructor_Initialise()
        {
            var accessor =
                Reflectless.Reflectless.GetConstructorAccessor<Func<object,object>>(typeof(ConstructorWithParametersDto), typeof(string));

            var value = accessor("A") as ConstructorWithParametersDto;

            Assert.NotNull(value);
            Assert.Equal("A", value.Name1);
        }

        [Fact]
        public void Non_Generic_Multi_Parameterised_Constructor_Initialise()
        {
            var accessor =
                Reflectless.Reflectless.GetConstructorAccessor<Func<object, object, object>>(
                    typeof(ConstructorWithParametersDto), typeof(string), typeof(string));

            var value = accessor("A","B") as ConstructorWithParametersDto;

            Assert.NotNull(value);
            Assert.Equal("A", value.Name1);
            Assert.Equal("B", value.Name2);
        }

        [Fact]
        public void Generic_Default_Constructor_Initialise()
        {
            var accessor = Reflectless.Reflectless.GetDefaultConstructorAccessor<DefaultConstructorDto>();

            var value = accessor();

            Assert.NotNull(value);
            Assert.Equal("X", value.Name);
        }

        [Fact]
        public void Generic_Parameterised_Constructor_Initialise()
        {
            var accessor =
                Reflectless.Reflectless.GetConstructorAccessor<Func<string,ConstructorWithParametersDto>>();

            var value = accessor("A");

            Assert.NotNull(value);
            Assert.Equal("A", value.Name1);
        }

        [Fact]
        public void Generic_Multi_Parameterised_Constructor_Initialise()
        {
            var accessor =
                Reflectless.Reflectless.GetConstructorAccessor<Func<string, string, ConstructorWithParametersDto>>();

            var value = accessor("A", "B");

            Assert.NotNull(value);
            Assert.Equal("A", value.Name1);
            Assert.Equal("B", value.Name2);
        }
    }
}
