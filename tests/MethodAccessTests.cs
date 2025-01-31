using ReflectlessTests.TestClasses;
using Xunit;

namespace ReflectlessTests
{
    public class MethodAccessTests
    {
        [Fact]
        public void Non_Generic_Method_Not_Exists_Throws()
        {
            var exception = Assert.Throws<Exception>(() =>
                Reflectless.Reflectless.GetMethodAccessor<Action<object>>(typeof(MethodDto), "Unknown"));

            Assert.Equal($"The method with name Unknown in type {typeof(MethodDto).FullName} does not exists.",
                exception.Message);
        }

        [Fact]
        public void Non_Generic_Method_With_No_Parameters_Or_Return_Type_Calls()
        {
            var action = Reflectless.Reflectless.GetMethodAccessor<Action<object>>(typeof(MethodDto), nameof(MethodDto.MethodWithNoParametersNoReturns));

            var testObj = new MethodDto();
            action(testObj);

            Assert.True(testObj.MethodCalled);
        }

        [Fact]
        public void Non_Generic_Method_With_No_Parameters_With_Return_Type_Calls()
        {
            var action = Reflectless.Reflectless.GetMethodAccessor<Func<object,object>>(typeof(MethodDto), nameof(MethodDto.MethodWithNoParametersHasReturns));

            var testObj = new MethodDto();
            var result = action(testObj);

            Assert.True(testObj.MethodCalled);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Non_Generic_Method_With_Parameters_And_With_Return_Type_Calls()
        {
            var action = Reflectless.Reflectless.GetMethodAccessor<Func<object, object, object>>(typeof(MethodDto), nameof(MethodDto.MethodWithParametersAndReturns));

            var testObj = new MethodDto();
            var result = action(testObj, "xyz");

            Assert.True(testObj.MethodCalled);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Generic_Method_With_No_Parameters_Or_Return_Type_Calls()
        {
            var action = Reflectless.Reflectless.GetMethodAccessor<Action<MethodDto>>(nameof(MethodDto.MethodWithNoParametersNoReturns));

            var testObj = new MethodDto();
            action(testObj);

            Assert.True(testObj.MethodCalled);
        }

        [Fact]
        public void Generic_Method_With_No_Parameters_With_Return_Type_Calls()
        {
            var action = Reflectless.Reflectless.GetMethodAccessor<Func<MethodDto, int>>(nameof(MethodDto.MethodWithNoParametersHasReturns));

            var testObj = new MethodDto();
            var result = action(testObj);

            Assert.True(testObj.MethodCalled);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Generic_Method_With_Parameters_And_With_Return_Type_Calls()
        {
            var action = Reflectless.Reflectless.GetMethodAccessor<Func<MethodDto, string, int>>(nameof(MethodDto.MethodWithParametersAndReturns));

            var testObj = new MethodDto();
            var result = action(testObj, "xyz");

            Assert.True(testObj.MethodCalled);
            Assert.Equal(3, result);
        }
    }
}
