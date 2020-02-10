using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame
{
    public class SpaceshipThruster : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _flameParticle;

        public void AddForce(float force)
        {
            if (force != 0)
            {
                if (!_flameParticle.isPlaying)
                    _flameParticle.Play();

                _flameParticle.startLifetime = force * 2;
            }
            else
            {
                _flameParticle.Stop();
            }

        }
    }
}
