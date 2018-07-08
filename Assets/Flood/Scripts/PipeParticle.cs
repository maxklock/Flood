using UnityEngine;

namespace Flood
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PipeParticle : MonoBehaviour
    {
        public PipeEnd End;

        [HideInInspector]
        public ParticleSystem Particle;

        // Use this for initialization
        private void Start()
        {
            Particle = GetComponent<ParticleSystem>();
            Particle.Stop();
            if (GetComponentInParent<StartPipe>() != null)
            {
                Particle.Play();
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}