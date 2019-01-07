using kbs2.Faction.CurrencyMVC;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CurrencyTests
    {
        public CurrencyTests()
        {
        }

        Currency_Controller controller;


        [Test]
        [TestCase(20f)]
        [TestCase(30f)]
        public void AddCurrency(float amount)
        {
            controller = new Currency_Controller(500);
            float expected = 500 + amount;
            controller.AlterCurrency(amount);

            Assert.AreEqual(expected, controller.Model.Currency);
        }

        [Test]
        [TestCase(20f)]
        [TestCase(30f)]
        public void RemoveCurrency(float amount)
        {
            controller = new Currency_Controller(500);
            float expected = 500 - amount;
            controller.AlterCurrency(-amount);

            Assert.AreEqual(expected, controller.Model.Currency);
        }
    }
}