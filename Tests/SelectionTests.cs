using System;
using System.Drawing;
using kbs2.GamePackage;
using NUnit.Framework;
namespace Tests
{
	[TestFixture]
	public class SelectionTests
	{

		[SetUp]
		public void init()
		{
			Selection_Controller selection = new Selection_Controller("PurpleLine");

		}

		[Test]
		public void CheckClickedBox_CheckForClick(bool clicked, bool result)
		{

		}
	}
}
