using UnityEngine;
using System.Collections;

namespace AO
{

    [RequireComponent(typeof(ParticleSystem))]
    public class DestructParticle : MonoBehaviour
    {

        void OnEnable()
        {
            StartCoroutine(CheckIfAlive());
        }

        IEnumerator CheckIfAlive()
        {
            ParticleSystem ps = this.GetComponent<ParticleSystem>();

            while (true && ps != null)
            {
                yield return new WaitForSeconds(0.5f);
                if (!ps.IsAlive(true))
                {

                    GameObject.Destroy(this.gameObject);
                    break;
                }
            }
        }
    }

}