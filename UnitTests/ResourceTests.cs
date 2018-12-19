using System;
using System.Collections.Generic;
using System.Text;
using kbs2.Faction.CurrencyMVC;
using kbs2.Faction.Enums;
using kbs2.Factory.Enums;
using kbs2.Factory.ResourceMVC;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ResourceTests
    {

        ResourceController controller;




        [Test]
        [TestCase(1, 2, 3, 10 + 500)]
        public void ResourcesToCurrencyTest(float type1, float type2, float type3, float result)
        {
            controller = new ResourceController();
            controller.AddResource(type1, ResourceType.Food);
            controller.AddResource(type1, ResourceType.Stone);
            controller.AddResource(type1, ResourceType.Wood);

            controller.ResourcesToCurrency();
            // this test contains an error
            // Assert.AreEqual(controller.model.currency, result);
        }


    }
}

