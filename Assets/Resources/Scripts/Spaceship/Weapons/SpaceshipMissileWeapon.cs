using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO.SpaceGame
{
    public class SpaceshipMissileWeapon : BaseWeapon
    {
        private float _force = 10000f;
        public override float Force { get => _force; set => _force = value; }
    }
}
