using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plataformer
{
    public class Collectible1 : Entity
    {
        [SerializeField] float health = 10;
        [SerializeField] FloatEventChannel healthChannel;


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                healthChannel.Invoke(health);
                Destroy(gameObject);
            }
        }
    }
}
