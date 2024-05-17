using UnityEngine;

namespace Plataformer
{
    public class GameManager : MonoBehaviour
    {
            public static GameManager instance {  get; private set; }
            
        public int Score { get; private set; }

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddScore(int score)
        { Score += score; }
    }
}
