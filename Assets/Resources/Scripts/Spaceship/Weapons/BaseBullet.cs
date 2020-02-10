
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseBullet : MonoBehaviour
    {
        protected Rigidbody _rigidbody;
        private bool _isShoot;

        private float _destroySecond = 3.0f;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            if (_isShoot && _rigidbody.velocity.magnitude < 10f)
                Destroy(gameObject);
        }

        public void AddForce(Vector3 vector3)
        {
            _isShoot = true;
            if (_rigidbody)
            {
                _rigidbody.AddForce(vector3);
                StartCoroutine(Destroy());
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(_destroySecond);
            Destroy(gameObject);
        }
    }
}
