using System;
using System.Collections.Generic;
using System.Text;
using kbs2.Faction.CurrencyMVC;
using kbs2.Faction.Enums;
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
        [TestCase(20)]
        [TestCase(30)]
        public void AddCurrency(float amount)
        {
            controller = new Currency_Controller();
            float expected = 500 + amount;
            controller.AlterCurrency(amount);

            Assert.IsTrue(controller.Model.currency == expected);
        }

        [Test]
        [TestCase(20)]
        [TestCase(30)]
        public void RemoveCurrency(float amount)
        {
            controller = new Currency_Controller();
            float expected = 500 - amount;
            controller.RemoveCurrency(amount);

            Assert.IsTrue(controller.Model.currency == expected);
        }


        [Test]
        [TestCase(20)]
        [TestCase(30)]
        public void AddUpkeepCost(float amount)
        {
            controller = new Currency_Controller();
            float expected = amount;
            controller.AddUpkeepCost(amount);

            Assert.IsTrue(controller.Model.UpkeepCost == expected);
        }



    }
}

