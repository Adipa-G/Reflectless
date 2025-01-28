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
                Reflectless.Reflectless.GetPropertySetAccessor(typeof(PropertySetTest), "Unknown"));

            Assert.Equal("The property Unknown in type ReflectlessTests.TestClasses.PropertySetTest does not exists.", exception.Message);
        }

        [Fact]
        public void Non_Generic_Property_Write_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertySetAccessor(typeof(PropertySetTest), "IntProperty");

            var testObj = new PropertySetTest();
            accessor.Invoke(testObj, 20);

            Assert.Equal(20, testObj.IntProperty);
        }

        [Fact]
        public void Generic_Property_Write_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetPropertySetAccessor<PropertySetTest, int>("Unknown"));

            Assert.Equal("The property Unknown in type ReflectlessTests.TestClasses.PropertySetTest does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Property_Write_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertySetAccessor<PropertySetTest, int>("IntProperty");

            var testObj = new PropertySetTest();
            accessor.Invoke(testObj, 20);

            Assert.Equal(20, testObj.IntProperty);
        }
    }
}
