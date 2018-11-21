using System.Collections.Generic;
using kbs2.Faction.Enums;
using kbs2.Faction.FactionMVC;
using kbs2.Faction.Interfaces;
using NUnit.Framework;


namespace Tests
{
	[TestFixture]
	public class FactionTests
	{
		private Faction_Model FactionModel;
		private Faction_Model FactionModelEnemy;

		[SetUp]
		public void init()
		{
			FactionModel = new Faction_Model("Vuur");
			FactionModelEnemy = new Faction_Model("Water");

			FactionModel.AddRelationship(FactionModelEnemy, Faction_Relations.hostile);
		}
		

		[Test]
		public void ReturnTrue()
		{

			var result = FactionModel.IsHostileTo(FactionModelEnemy);

			Assert.IsTrue(result, "They are Hostile to each other");
			
		}
	}
}

