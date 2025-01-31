using ReflectlessTests.TestClasses;
using Xunit;

namespace ReflectlessTests
{
    public class PropertyGetAccessTests
    {
        [Fact]
        public void Non_Generic_Property_Read_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetPropertyGetAccessor(typeof(PropertyGetDto), "Unknown"));

            Assert.Equal($"The property Unknown in type {typeof(PropertyGetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Non_Generic_Property_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertyGetAccessor(typeof(PropertyGetDto), nameof(PropertyGetDto.IntProperty));

            var testObj = new PropertyGetDto();
            var value = accessor(testObj);

            Assert.Equal(10, value);
        }

        [Fact]
        public void Generic_Property_Read_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetPropertyGetAccessor<PropertyGetDto,int>("Unknown"));

            Assert.Equal($"The property Unknown in type {typeof(PropertyGetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Property_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertyGetAccessor<PropertyGetDto, int>(nameof(PropertyGetDto.IntProperty));

            var testObj = new PropertyGetDto();
            var value = accessor(testObj);

            Assert.Equal(10, value);
        }
    }
}
