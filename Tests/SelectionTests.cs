using System;
using System.Drawing;
using kbs2.GamePackage;
using NUnit.Framework;
namespace Tests
{
	[TestFixture]
	public class SelectionTests
	{
		private Selection_Controller select;
		

		[SetUp]
		public void init()
		{
			select = new Selection_Controller("PurpleLine");

		}

		// Ik kan hier geen Tests voor schrijven omdat die shit monogame.framework nodig heeft voor de verschillenden methods
		// Dus weet iemand hoe je die monogame.Framework toevoegd als reference aan die tests file?
		
	}
}
