using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO.SpaceGame
{
    public abstract class SpaceshipAssaultWeapon : BaseWeapon
    {
        private float _force = 12000f;
        public override float Force { get => _force; set => _force = value; }

    }
}
