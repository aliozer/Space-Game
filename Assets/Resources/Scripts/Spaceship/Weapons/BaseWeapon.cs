using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public enum WeaponAttackType
    {
        Single,
        Brust
    }

    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField]
        protected BaseBullet _bulletPrefab;
        [SerializeField]
        protected List<Transform> _emitters;
        [SerializeField]
        protected float _fireRate = 2f;
        public float FireRate { get => _fireRate; set => _fireRate = value; }

        [SerializeField]
        private WeaponAttackType _attackType = WeaponAttackType.Single;
        public WeaponAttackType AttackType { get => _attackType; set => _attackType = value; }

        private float _nextFire = 0;

        public bool IsReady { get; protected set; } = true;
        public float SpeedFactor { get; set; } = 1.0f;
        public abstract float Force { get; set; }

        protected virtual void Update()
        {
            if (!IsReady && Time.time > _nextFire)
                IsReady = true;
        }

        public void Shoot()
        {
            if (IsReady)
            {
                _nextFire = Time.time + _fireRate;
                IsReady = false;

                if (_bulletPrefab)
                {
                    foreach (var emitter in _emitters)
                    {
                        BaseBullet bullet = Instantiate(_bulletPrefab, emitter.position, emitter.rotation);
                        StartCoroutine(ShootBullet(bullet, emitter.forward * Force * SpeedFactor));

                        if (AttackType == WeaponAttackType.Brust)
                        {
                            bullet = Instantiate(_bulletPrefab, emitter.position, emitter.rotation);
                            StartCoroutine(ShootBullet(bullet, emitter.forward * Force * SpeedFactor, 0.1f));
                        }
                    }
                }

            }
        }

        private IEnumerator ShootBullet(BaseBullet bullet, Vector3 force, float delay = 0.0f)
        {
            yield return new WaitForSeconds(delay);
            bullet.AddForce(force);
        }
    }
}
