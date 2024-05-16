using UnityEngine;

namespace Plataformer
{
    public class GroundChecker : MonoBehaviour {
        
        [SerializeField] float groundDistance = 0.06f;
        [SerializeField] LayerMask groundLayers;

        public bool IsGrounded { get; private set; }

        void Update()
        {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayers);
        }
    }
}