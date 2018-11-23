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
		private Faction_Model Unit;
		private Faction_Model Enemy;
		private Faction_Model Friend;
		private Faction_Model Neutral;

		[SetUp]
		public void init()
		{
			Unit = new Faction_Model("Water");
			Enemy = new Faction_Model("Earth");
			Friend = new Faction_Model("Fire");
			Neutral = new Faction_Model("Äir");


		}
		
		
		[Test]
		[TestCase(Faction_Relations.hostile, true)]
		[TestCase(Faction_Relations.friendly, false)]
		[TestCase(Faction_Relations.neutral, false)]
		public void IsHostileToUnit(Faction_Relations relation, bool ExpectedResult)
		{
			Unit.AddRelationship(Enemy, relation);


			var result = Unit.IsHostileTo(Enemy);
			var result2 = Enemy.IsHostileTo(Unit);


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
		public void AddRelationshipToUnit(Faction_Relations relation, Faction_Relations RelationCheck, bool ExpectedResult)
		{
			Unit.AddRelationship(Friend, relation);

			var result = false;
			var result2 = false;

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Unit.FactionRelationships)
			{
				if (relationship.Key.Name == Friend.Name && relationship.Value == RelationCheck)
				{
					result = true;
				}
			}

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Friend.FactionRelationships)
			{
				if (relationship.Key.Name == Unit.Name && relationship.Value == RelationCheck)
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
		public void ChangeRelationshipOfUnit(Faction_Relations relation, Faction_Relations ChangedRelation, bool ExpectedResult)
		{
			Unit.AddRelationship(Friend, relation);

			var result = false;
			var result2 = false;

			Unit.ChangeRelationship(Friend, ChangedRelation);

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Unit.FactionRelationships)
			{
				if (relationship.Key.Name == Friend.Name && relationship.Value != relation)
				{
					result = true;
				}
			}

			foreach (KeyValuePair<Faction_Model, Faction_Relations> relationship in Friend.FactionRelationships)
			{
				if (relationship.Key.Name == Unit.Name && relationship.Value != relation)
				{
					result2 = true;
				}
			}

			Assert.IsTrue(result == ExpectedResult);
			Assert.IsTrue(result2 == ExpectedResult);

		}

	}
}

