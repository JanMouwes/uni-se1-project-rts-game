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
        [TestCase(1f, 2f, 3f, 10f + 500f)]
        public void ResourcesToCurrencyTest(float type1, float type2, float type3, float result)
        {
            calculator = new ResourceCalculator();
            calculator.AddResource(type1, ResourceType.Food);
            calculator.AddResource(type2, ResourceType.Stone);
            calculator.AddResource(type3, ResourceType.Wood);

            float actual = calculator.CalculateResourceWorth();
            
            // this test contains an error
            Assert.AreEqual(result, actual);
        }
    }
}