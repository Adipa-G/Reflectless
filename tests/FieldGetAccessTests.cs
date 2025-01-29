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
                Reflectless.Reflectless.GetFieldGetAccessor(typeof(FieldGetDto), "Unknown"));

            Assert.Equal($"The field Unknown in type {typeof(FieldGetDto).FullName} does not exists.",
                exception.Message);
        }

        [Fact]
        public void Non_Generic_Field_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetFieldGetAccessor(typeof(FieldGetDto),  nameof(FieldGetDto.IntField));

            var testObj = new FieldGetDto();
            var value = accessor.Invoke(testObj);

            Assert.Equal(10, value);
        }

        [Fact]
        public void Generic_Field_Read_When_Property_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetFieldGetAccessor<FieldGetDto, int>("Unknown"));

            Assert.Equal($"The field Unknown in type {typeof(FieldGetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Field_Read_Success()
        {
            var accessor = Reflectless.Reflectless.GetFieldGetAccessor<FieldGetDto, int>(nameof(FieldGetDto.IntField));

            var testObj = new FieldGetDto();
            var value = accessor.Invoke(testObj);

            Assert.Equal(10, value);
        }
    }
}
