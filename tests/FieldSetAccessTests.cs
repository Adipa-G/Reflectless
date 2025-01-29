using ReflectlessTests.TestClasses;
using Xunit;

namespace ReflectlessTests
{
    public class FieldSetAccessTests
    {
        [Fact]
        public void Non_Generic_Field_Write_When_Field_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetFieldSetAccessor(typeof(FieldSetDto), "Unknown"));

            Assert.Equal($"The field Unknown in type {typeof(FieldSetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Non_Generic_Field_Write_Success()
        {
            var accessor = Reflectless.Reflectless.GetFieldSetAccessor(typeof(FieldSetDto), nameof(FieldSetDto.StrField));

            var testObj = new FieldSetDto();
            accessor.Invoke(testObj, "Updated Value");

            Assert.Equal("Updated Value", testObj.StrField);
        }

        [Fact]
        public void Generic_Field_Write_When_Field_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetFieldSetAccessor<FieldSetDto, string>("Unknown"));

            Assert.Equal($"The field Unknown in type {typeof(FieldSetDto).FullName} does not exists.", exception.Message);
        }

        [Fact]
        public void Generic_Field_Write_Success()
        {
            var accessor = Reflectless.Reflectless.GetFieldSetAccessor<FieldSetDto, string>(nameof(FieldSetDto.StrField));

            var testObj = new FieldSetDto();
            accessor.Invoke(testObj, "Updated Value");

            Assert.Equal("Updated Value", testObj.StrField);
        }
    }
}
