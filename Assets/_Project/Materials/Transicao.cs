using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plataformer
{
    [ExecuteInEditMode]
    public class Transicao : MonoBehaviour
    {
        public Shader shader;
        private Material material;

        [Range(0, 1)]
        public float cutoff = 0.5f;

        void Start()
        {
            if (shader == null)
            {
                Debug.LogError("Shader not set!");
                enabled = false;
                return;
            }

            material = new Material(shader);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (material != null)
            {
                material.SetFloat("_Cutoff", cutoff);
                Graphics.Blit(src, dest, material);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }
    }
}