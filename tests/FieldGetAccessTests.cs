using ReflectlessTests.TestClasses;
using Xunit;

namespace ReflectlessTests
{
    public class FieldGetAccessTests
    {
        [Fact]
        public void Non_Generic_Field_Read_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetFieldGetAccessor(typeof(FieldGetTest), "Unknown"));

            Assert.Equal("The field Unknown in type ReflectlessTests.TestClasses.FieldGetTest does not exists.", exception.Message);
        }

        [Fact]
        public void Non_Generic_Field_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetFieldGetAccessor(typeof(FieldGetTest), "IntField");

            var testObj = new FieldGetTest();
            var value = accessor.Invoke(testObj);

            Assert.Equal(10, value);
        }

        [Fact]
        public void Generic_Field_Read_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetFieldGetAccessor<FieldGetTest, int>("Unknown"));

            Assert.Equal("The field Unknown in type ReflectlessTests.TestClasses.FieldGetTest does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Field_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetFieldGetAccessor<FieldGetTest, int>("IntField");

            var testObj = new FieldGetTest();
            var value = accessor.Invoke(testObj);

            Assert.Equal(10, value);
        }
    }
}
