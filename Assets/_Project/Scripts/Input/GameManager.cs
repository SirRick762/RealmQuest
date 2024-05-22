using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Plataformer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance {  get; private set; }



        public bool topscore;
        private void Start()
        {
            topscore = false;
        }

        
        public int Score { get; private set; }


        public void checkscore()
        {
            if(Score >= 100)
            {
                topscore = true;
            }
        }
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
