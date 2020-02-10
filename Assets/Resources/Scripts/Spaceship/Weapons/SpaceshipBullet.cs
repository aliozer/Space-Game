using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO.SpaceGame
{
    public class SpaceshipBullet : BaseBullet, IDamaging
    {
        public float Damage => 10f;
    }
}
