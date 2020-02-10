using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseAirplaneController : MonoBehaviour
    {
        private const float POUNDS_TO_KILOS = 0.453592f;
        private const float MPS_TO_MPH = 2.23694f;

        [Header("Control Settgins")]
        [SerializeField]
        protected float _pitchSpeed = 4000f;
        [SerializeField]
        protected float _rollSpeed = 2000f;
        [SerializeField]
        private float _yawSpeed = 2000f;

        [Header("Airplane Properties")]
        [SerializeField]
        protected Transform _centerOfGravity;
        [SerializeField]
        protected float _weight = 800f;
        [SerializeField]
        protected float _maxMph = 110f;

        [Header("Engines")]
        public List<BaseAirplaneEngine> Engines = new List<BaseAirplaneEngine>();

        protected float _maxMPS = 0.0f;
        protected Rigidbody _rigidbody;
        protected float _angleOfAttack;
        private float _forwardSpeed;
        

        public float MPH { get; protected set; }

        protected virtual void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.mass = _weight * POUNDS_TO_KILOS;

            if (_centerOfGravity)
                _rigidbody.centerOfMass = _centerOfGravity.localPosition;

            _maxMPS = _maxMph / MPS_TO_MPH;
        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (_rigidbody != null)
            {

                foreach (var engine in Engines)
                {
                    _rigidbody.AddForce(engine.CalculateForce(GetThrottle()));
                }

                CalculateForwardSpeed();
                CalculateLift();
                UpdateRigidbody();
                HandlePitch();
                HandleRoll();
                HandleYaw();
            }

        }

        private void HandleYaw()
        {
            Vector3 yawTorque = GetYaw() * _yawSpeed * transform.up;
            _rigidbody.AddTorque(yawTorque);
        }

        private void HandleRoll()
        {
            Vector3 flatRight = transform.right;
            flatRight.y = 0f;
            flatRight = flatRight.normalized;

            Vector3 rollTorque = GetRoll() * _rollSpeed * transform.forward;
            _rigidbody.AddTorque(rollTorque);
        }

        private void HandlePitch()
        {
            Vector3 flatForward = transform.forward;
            flatForward.y = 0f;
            flatForward = flatForward.normalized;

            Vector3 pitchTorque = GetPitch() * _pitchSpeed * transform.right;
            _rigidbody.AddTorque(pitchTorque);
        }

        private void UpdateRigidbody()
        {
            if (_rigidbody.velocity.magnitude > 1.0f)
            {
                Vector3 updateVelocity = Vector3.Lerp(_rigidbody.velocity, transform.forward * _forwardSpeed, _forwardSpeed * _angleOfAttack * Time.deltaTime);
                _rigidbody.velocity = updateVelocity;

                Quaternion updateQuaternion = Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(_rigidbody.velocity.normalized, transform.up), Time.deltaTime);
                _rigidbody.MoveRotation(updateQuaternion);
            }
        }

        private void CalculateLift()
        {
            _angleOfAttack = Vector3.Dot(_rigidbody.velocity.normalized, transform.forward);
            _angleOfAttack *= _angleOfAttack;
        }

        private void CalculateForwardSpeed()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
            _forwardSpeed = Mathf.Max(0.0f, localVelocity.z);
            _forwardSpeed = Mathf.Clamp(_forwardSpeed, 0f, _maxMPS);

            MPH = _forwardSpeed * MPS_TO_MPH;
            MPH = Mathf.Clamp(MPH, 0f, _maxMph);
        }

        protected abstract float GetThrottle();
        protected abstract float GetPitch();
        protected abstract float GetRoll();
        protected abstract float GetYaw();
    }
}
