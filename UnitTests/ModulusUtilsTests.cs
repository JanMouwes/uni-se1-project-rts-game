using kbs2.utils;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ModulusUtilsTests
    {
        [TestCase(28, 20, 8)]
        [TestCase(-28, 20, 12)]
        [TestCase(-18, 20, 2)]
        public void Test_ShouldOutputCorrectNumber_WhenGivenInput(int inputInt, int modulus, int expectedResult)
        {
            //    Arrange
            //    Act
            int actual = ModulusUtils.mod(inputInt, modulus);
            
            //    Assert
            Assert.AreEqual(expectedResult, actual);
        }
    }
}