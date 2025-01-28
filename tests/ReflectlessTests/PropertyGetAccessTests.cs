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
                Reflectless.Reflectless.GetPropertyGetAccessor(typeof(PropertyGetTest), "Unknown"));

            Assert.Equal("The property Unknown in type ReflectlessTests.TestClasses.PropertyGetTest does not exists.", exception.Message);
        }

        [Fact]
        public void Non_Generic_Property_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertyGetAccessor(typeof(PropertyGetTest), "IntProperty");

            var testObj = new PropertyGetTest();
            var value = accessor.Invoke(testObj);

            Assert.Equal(10, value);
        }

        [Fact]
        public void Generic_Property_Read_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetPropertyGetAccessor<PropertyGetTest,int>("Unknown"));

            Assert.Equal("The property Unknown in type ReflectlessTests.TestClasses.PropertyGetTest does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Property_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetPropertyGetAccessor<PropertyGetTest, int>("IntProperty");

            var testObj = new PropertyGetTest();
            var value = accessor.Invoke(testObj);

            Assert.Equal(10, value);
        }
    }
}
