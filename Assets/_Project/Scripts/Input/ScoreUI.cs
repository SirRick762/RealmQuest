using UnityEngine;

namespace Plataformer
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI scoreText;

        void Start ()
        {
            UpdateScore();
        }


        public void UpdateScore ()
        {
            StartCoroutine(UpdateScoreNextFrame());
        }

        System.Collections.IEnumerator UpdateScoreNextFrame()
        {
            yield return null;
            scoreText.text = GameManager.instance.Score.ToString();
        }
    }
}
