using System;
using System.Collections.Generic;
using System.Text;
using kbs2.World.Structs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class PathfinderTests
    {
        private Pathfinder pathfinder = new Pathfinder(null, 500);


        
            List<FloatCoords>[] input = new List<FloatCoords>[]
            {
             new List<FloatCoords>{
            new FloatCoords {x=0,y=0}
            ,new FloatCoords {x=1,y=0}
            ,new FloatCoords {x=2,y=0}
            ,new FloatCoords {x=2,y=1}
            ,new FloatCoords {x=2,y=2}
            ,new FloatCoords {x=3,y=2}
            ,new FloatCoords {x=4,y=2} }
            };

            public List<FloatCoords>[] output = new List<FloatCoords>[]
            {
            new List<FloatCoords>{
            new FloatCoords {x=0,y=0}
            ,new FloatCoords {x=2,y=0}
            ,new FloatCoords {x=2,y=2}
            ,new FloatCoords {x=4,y=2} }
            };
        

        [Test]
        [TestCase(0)]
        public void MinimizeWaypointTest(int index)
        {
            List < FloatCoords > x = pathfinder.MinimizeWaypoints(input[index]);

            for(int i =0; i<output[index].Count ; i++)
            {
                Assert.IsTrue(x[i] == output[index][i]);
            }
        }
    }
}
