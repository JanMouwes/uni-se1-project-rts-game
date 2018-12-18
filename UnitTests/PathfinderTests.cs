using System;
using System.Collections.Generic;
using System.Text;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class PathfinderTests
    {
        public WeightDictionarys obstacles;
        private Pathfinder pathfinder;
        List<FloatCoords>[] input;
        public List<FloatCoords>[] output;
        Coords one;
        Coords[] two;
        bool[] blocked;

        [SetUp]
        public void setup()
        {
            obstacles = new WeightDictionarys();
            obstacles.ObstacleList = new List<Coords>();
            obstacles.ObstacleList.Add(new Coords { x = 0, y = 1 });
            obstacles.ObstacleList.Add(new Coords { x = 1, y = 0 });

            pathfinder = new Pathfinder(null, 500);

            input = new List<FloatCoords>[]
            {
             new List<FloatCoords>{
            new FloatCoords {x=0,y=0}
            ,new FloatCoords {x=1,y=0}
            ,new FloatCoords {x=2,y=0}
            ,new FloatCoords {x=2,y=1}
            ,new FloatCoords {x=2,y=2}
            ,new FloatCoords {x=3,y=2}
            ,new FloatCoords {x=4,y=2} }
             ,
             new List<FloatCoords>{
            new FloatCoords {x=0,y=0}
            ,new FloatCoords {x=1,y=0}
            ,new FloatCoords {x=1,y=1}
            ,new FloatCoords {x=2,y=1}
            ,new FloatCoords {x=2,y=2}
            ,new FloatCoords {x=3,y=2}
            ,new FloatCoords {x=3,y=3} }
            };
            output = new List<FloatCoords>[]
            {
            new List<FloatCoords>{
            new FloatCoords {x=0,y=0}
            ,new FloatCoords {x=2,y=0}
            ,new FloatCoords {x=2,y=2}
            ,new FloatCoords {x=4,y=2} }
            ,
            new List<FloatCoords>{
            new FloatCoords {x=0,y=0}
            ,new FloatCoords {x=3,y=3} }
            };


            one = new Coords { x = 0, y = 0 };
            two = new Coords[]
            {
            new Coords{x=1,y=1},
            new Coords{x=-1,y=0},
            new Coords{x=-1,y=-1},
            new Coords{x=1,y=-1},
            new Coords{x=-1,y=1}
            };
            blocked = new bool[]
            {
            false,
            true,
            true,
            true,
            true
            };
        }



        //        [Test]
        //        [TestCase(0)]
        //        [TestCase(1)]
        //        public void MinimizeWaypointTest(int index)
        //        {
        //            List < FloatCoords > x = pathfinder.MinimizeWaypoints(input[index]);
        //
        //            for(int i =0; i<output[index].Count ; i++)
        //            {
        //                Assert.IsTrue(x[i] == output[index][i]);
        //            }
        //        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void CheckDiagonalsBlockedTest(int index)
        {
            Assert.IsTrue(pathfinder.CheckDiagonalsBlocked(one, two[index], obstacles) == blocked[index]);
        }
    }
}
