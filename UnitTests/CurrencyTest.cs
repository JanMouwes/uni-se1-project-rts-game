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
        [TestCase(20f)]
        [TestCase(30f)]
        public void AddCurrency(float amount)
        {
            controller = new Currency_Controller(500);
            float expected = 500 + amount;
            controller.AlterCurrency(amount);

            Assert.AreEqual(expected, controller.Model.currency);
        }

        [Test]
        [TestCase(20f)]
        [TestCase(30f)]
        public void RemoveCurrency(float amount)
        {
            controller = new Currency_Controller(500);
            float expected = 500 - amount;
            controller.AlterCurrency(-amount);

            Assert.AreEqual(expected, controller.Model.currency);
        }


        [Test]
        [TestCase(20f)]
        [TestCase(30f)]
        public void AddUpkeepCost(float amount)
        {
            controller = new Currency_Controller(500);
            float expected = amount;
            controller.AlterCurrency(amount);

            Assert.AreEqual(expected, controller.Model.UpkeepCost);
        }
    }
}