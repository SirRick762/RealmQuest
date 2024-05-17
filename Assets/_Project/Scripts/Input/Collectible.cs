using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plataformer
{
    public class Collectible : Entity
    {
        [SerializeField] int score = 10;
        [SerializeField] IntEventChannel scoreChannel;


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                scoreChannel.Invoke(score);
                Destroy(gameObject);
            }
        }
    }
}
