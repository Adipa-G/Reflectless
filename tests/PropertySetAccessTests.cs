using ReflectlessTests.TestClasses;
using Xunit;

namespace ReflectlessTests
{
    public class PropertySetAccessTests
    {
        [Fact]
        public void Non_Generic_Property_Write_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetPropertySetAccessor(typeof(PropertySetDto), "Unknown"));

            Assert.Equal($"The property Unknown in type {typeof(PropertySetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Non_Generic_Property_Write_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertySetAccessor(typeof(PropertySetDto), nameof(PropertySetDto.StrProperty));

            var testObj = new PropertySetDto();
            accessor.Invoke(testObj, "Updated Value");

            Assert.Equal("Updated Value", testObj.StrProperty);
        }

        [Fact]
        public void Generic_Property_Write_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetPropertySetAccessor<PropertySetDto, string>("Unknown"));

            Assert.Equal($"The property Unknown in type {typeof(PropertySetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Property_Write_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertySetAccessor<PropertySetDto, string>(nameof(PropertySetDto.StrProperty));

            var testObj = new PropertySetDto();
            accessor.Invoke(testObj, "Updated Value");

            Assert.Equal("Updated Value", testObj.StrProperty);
        }
    }
}
