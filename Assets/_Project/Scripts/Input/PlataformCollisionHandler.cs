using UnityEngine;

namespace Plataformer
{
    public class PlataformCollisionHandler : MonoBehaviour {

        Transform plataform;

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlataform"))
            {
                ContactPoint contact = other.GetContact(index: 0);
                if (contact.normal.y < 0.5f) return;

                plataform = other.transform;
                transform.SetParent(plataform);

            }
        }
        void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlataform"))
            {
                transform.SetParent(null);
                plataform = null;

            }
        }

    }
}