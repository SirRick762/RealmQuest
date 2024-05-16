using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Plataformer
{
    public class LoadCharacter : MonoBehaviour
    {
        public GameObject[] characterPrefabs;
        public Transform spawnPoint;
        //public TMP_Text label;

        void Start()
        {
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            GameObject prefab = characterPrefabs[selectedCharacter];
            GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            // Assign the "Player" tag to the instantiated character clone
            clone.tag = "Player";

            //label.text = prefab.name;
        }
    }
}
