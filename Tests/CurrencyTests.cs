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
        [TestCase(1,2,3,10+500)]
        public void ResourcesToCurrencyTest(float type1, float type2, float type3, float result)
        {
            controller = new Currency_Controller();
            controller.AddResource(type1, ResourceType.Food);
            controller.AddResource(type1, ResourceType.Stone);
            controller.AddResource(type1, ResourceType.Wood);

            controller.ResourcesToCurrency();
            Assert.AreEqual(controller.model.currency, result);
        }

        [Test]
        [TestCase(20)]
        [TestCase(30)]
        public void AddCurrency(float amount)
        {
            controller = new Currency_Controller();
            float expected = 500 + amount;
            controller.AddCurrency(amount);

            Assert.IsTrue(controller.model.currency == expected);
        }

        [Test]
        [TestCase(20)]
        [TestCase(30)]
        public void RemoveCurrency(float amount)
        {
            controller = new Currency_Controller();
            float expected = 500 - amount;
            controller.RemoveCurrency(amount);

            Assert.IsTrue(controller.model.currency == expected);
        }



    }
}

