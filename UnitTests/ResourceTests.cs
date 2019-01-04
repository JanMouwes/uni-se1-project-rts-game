using System;
using System.Collections.Generic;
using System.Text;
using kbs2.Faction.CurrencyMVC;
using kbs2.Faction.Enums;
using kbs2.Resources;
using kbs2.Resources.Enums;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ResourceTests
    {

        ResourceCalculator calculator;




        [Test]
        [TestCase(1, 2, 3, 10 + 500)]
        public void ResourcesToCurrencyTest(float type1, float type2, float type3, float result)
        {
            calculator = new ResourceCalculator();
            calculator.AddResource(type1, ResourceType.Food);
            calculator.AddResource(type1, ResourceType.Stone);
            calculator.AddResource(type1, ResourceType.Wood);

            calculator.CalculateResourceWorth();
            // this test contains an error
            // Assert.AreEqual(controller.model.currency, result);
        }


    }
}

