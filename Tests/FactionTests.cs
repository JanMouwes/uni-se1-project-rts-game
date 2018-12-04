using System.Collections.Generic;
using kbs2.Faction.Enums;
using kbs2.Faction.FactionMVC;
using NUnit.Framework;


namespace Tests
{
	[TestFixture]
	public class FactionTests
	{
		private Faction_Controller Unit;
		private Faction_Controller Enemy;
		private Faction_Controller Friend;
		private Faction_Controller Neutral;
		

		[SetUp]
		public void init()
		{
			Unit = new Faction_Controller("Water");
			Enemy = new Faction_Controller("Earth");
			Friend = new Faction_Controller("Fire");
			Neutral = new Faction_Controller("Äir");
			
		}
		
		
		[Test]
		[TestCase(Faction_Relations.hostile, true)]
		[TestCase(Faction_Relations.friendly, false)]
		[TestCase(Faction_Relations.neutral, false)]
		public void IsHostileToFaction(Faction_Relations relation, bool ExpectedResult)
		{
			Unit.AddRelationship(Enemy.FactionModel, relation);


			var result = Unit.IsHostileTo(Enemy.FactionModel);
			var result2 = Enemy.IsHostileTo(Unit.FactionModel);


			Assert.IsTrue(result == ExpectedResult);
			Assert.IsTrue(result2 == ExpectedResult);

		}

		[Test]
		[TestCase(Faction_Relations.friendly, Faction_Relations.friendly, true)]
		[TestCase(Faction_Relations.hostile, Faction_Relations.hostile, true)]
		[TestCase(Faction_Relations.neutral, Faction_Relations.neutral, true)]
		[TestCase(Faction_Relations.friendly, Faction_Relations.hostile, false)]
		[TestCase(Faction_Relations.hostile, Faction_Relations.neutral, false)]
		[TestCase(Faction_Relations.neutral, Faction_Relations.friendly, false)]
		public void AddRelationshipToFaction(Faction_Relations relation, Faction_Relations RelationCheck, bool ExpectedResult)
		{
			Unit.AddRelationship(Friend.FactionModel, relation);

			var result = false;
			var result2 = false;

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Unit.FactionModel.FactionRelationships)
			{
				if (relationship.Key.Name == Friend.FactionModel.Name && relationship.Value == RelationCheck)
				{
					result = true;
				}
			}

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Friend.FactionModel.FactionRelationships)
			{
				if (relationship.Key.Name == Unit.FactionModel.Name && relationship.Value == RelationCheck)
				{
					result2 = true;
				}
			}

			Assert.IsTrue(result == ExpectedResult);
			Assert.IsTrue(result2 == ExpectedResult);
		}


		[Test]
		[TestCase(Faction_Relations.neutral, Faction_Relations.hostile, true)]
		[TestCase(Faction_Relations.friendly, Faction_Relations.neutral, true)]
		[TestCase(Faction_Relations.hostile, Faction_Relations.hostile, false)]
		public void ChangeRelationshipOfFaction(Faction_Relations relation, Faction_Relations ChangedRelation, bool ExpectedResult)
		{
			Unit.AddRelationship(Friend.FactionModel, relation);

			var result = false;
			var result2 = false;

			Unit.ChangeRelationship(Friend.FactionModel, ChangedRelation);

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Unit.FactionModel.FactionRelationships)
			{
				if (relationship.Key.Name == Friend.FactionModel.Name && relationship.Value != relation)
				{
					result = true;
				}
			}

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Friend.FactionModel.FactionRelationships)
			{
				if (relationship.Key.Name == Unit.FactionModel.Name && relationship.Value != relation)
				{
					result2 = true;
				}
			}

			Assert.IsTrue(result == ExpectedResult); 
			Assert.IsTrue(result2 == ExpectedResult);

		}

	}
}

