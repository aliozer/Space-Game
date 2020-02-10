using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame
{

    public abstract class BaseAstroid : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private GameObject _hitParticlePrefab;
        [SerializeField]
        private GameObject _explosionParticlePrefab;

        private float _currentHealth;
        public abstract float Health { get; }

        protected virtual void Awake()
        {
            _currentHealth = Health;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0f)
            {
                if (_explosionParticlePrefab != null)
                    CreatePrefab(_explosionParticlePrefab, transform.position);

                Destroy(gameObject);
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (_hitParticlePrefab != null) 
                CreatePrefab(_hitParticlePrefab, collision.contacts[0].point);

            IDamaging damaging = collision.collider.GetComponent<IDamaging>();

            if (damaging != null)
                TakeDamage(damaging.Damage);
        }

        private void CreatePrefab(GameObject prefab, Vector3 position)
        {
            Instantiate(prefab, position, Quaternion.identity);

        }
    }
}
