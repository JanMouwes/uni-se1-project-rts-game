using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Health
{
	public class HP_Controller
	{
        public HP_Model HPModel;

		public HP_Controller(int curHP, int maxHP)
		{
            HPModel = new HP_Model(curHP, maxHP);
		}

		public void AddHP()
		{
			
		}
		public void RemoveHP()
		{
			
		}
		private void AffectHP()
		{

		}
	}
}
