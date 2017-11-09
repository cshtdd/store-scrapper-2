using System;
using Xunit;

namespace store_screapper_2_Tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(2, 2 - 1 + 1);
        }

        [Fact]
        public void FaillingTest(){
            Assert.Equal(3, 2 + 1);
        }
    }
}
