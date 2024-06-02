using UnityEngine;

namespace Plataformer
{
    public class PlatformCollisionHandler : MonoBehaviour
    {
        Transform platform;
        Vector3 offset;
        bool onPlatform = false;

        void Update()
        {
            if (onPlatform && platform != null)
            {
                // Keep the player at the same offset from the platform
                transform.position = platform.position + offset;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                ContactPoint contact = other.GetContact(0);
                if (contact.normal.y < 0.5f) return;

                platform = other.transform;
                offset = transform.position - platform.position;
                onPlatform = true;
            }
        }

        void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                platform = null;
                onPlatform = false;
            }
        }
    }
}