
using AO.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame
{

    public class AstroidController : MonoBehaviour
    {
        [Header("General settings:")]
        [SerializeField]
        private List<BaseAstroid> _prefabs;
        [SerializeField]
        private int _asteroidCount = 50;
        [SerializeField]
        private float _range = 1000.0f;
        [SerializeField]
        private Vector2 _scaleRange = new Vector2(1.0f, 3.0f);

        [Header("Rigidbody settings:")]
        [SerializeField]
        private float _velocity = 0.0f;
        [SerializeField]
        private float _angularVelocity = 0.0f;
        [SerializeField]
        private float _massFactor = 20.0f;


        public void Create()
        {
            if (_prefabs.Count > 0)
            {
                for (int i = 0; i < _asteroidCount; i++)
                    CreateAsteroid();
            }
        }

        public void Clear()
        {
            transform.Clear();
        }


        private void CreateAsteroid()
        {
            Vector3 spawnPos = Random.insideUnitSphere * _range;
            spawnPos += transform.position;

            Quaternion spawnRot = Random.rotation;
            var prefab = _prefabs[Random.Range(0, _prefabs.Count)];
            BaseAstroid astroid = Instantiate(prefab, spawnPos, spawnRot);
            astroid.transform.SetParent(transform);

            float scale = Random.Range(_scaleRange.x, _scaleRange.y);
            astroid.transform.localScale = Vector3.one * scale;

            Rigidbody rigidbody = astroid.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.mass *= scale * scale * scale * _massFactor;
                rigidbody.AddRelativeForce(Random.insideUnitSphere * _velocity, ForceMode.VelocityChange);
                rigidbody.AddRelativeTorque(Random.insideUnitSphere * _angularVelocity * Mathf.Deg2Rad, ForceMode.VelocityChange);
            }
        }
    }
}
