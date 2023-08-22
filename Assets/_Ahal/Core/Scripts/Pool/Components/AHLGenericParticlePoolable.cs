using UnityEngine;

namespace AHL.Core.Pool.Components
{
    public class AHLGenericParticlePoolable : AHLPoolableMonoBehaviour
    {
        private ParticleSystem particleSystem;

        private void Start()
        {
            // particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnDisable()
        {
            if (particleSystem != null)
            {
                particleSystem.Stop(); // Stop emission
                particleSystem.Clear(); // Clear existing particles    
            }
        }

        public override void OnReturnedToPool()
        { 
            // gameObject.SetActive(false);
        }
    }
}