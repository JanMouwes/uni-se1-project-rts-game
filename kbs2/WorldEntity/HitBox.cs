using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.WorldEntity
{
	class HitBox : CircleHitBox, RectHitBox
	{
		public double Radius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public double[,] Dimension { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public HitBox()
		{

		}

		public bool CollideWith(HitBox hitbox)
		{
			return true;
		}
		public void CollideWith(Coords coords)
		{

		}
	}
}
