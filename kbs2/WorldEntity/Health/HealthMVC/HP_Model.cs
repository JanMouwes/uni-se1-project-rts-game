﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.WorldEntity.Health
{
	public class HP_Model
	{
		public int CurrentHP { get; set; }
		public int MaxHP { get; set; }

        public HP_Model(int currentHP, int maxHP)
        {
            CurrentHP = currentHP;
            MaxHP = maxHP;
        }
	}
}
