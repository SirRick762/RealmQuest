using UnityEngine;

namespace Plataformer
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;
    }
}
