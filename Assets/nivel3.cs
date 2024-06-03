using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plataformer
{
    public class nivel3 : MonoBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene("Nivel 3");
            }
        }
    }
}
