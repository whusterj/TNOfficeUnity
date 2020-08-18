using TMPro;
using UnityEngine;

namespace TNOffice.Scoring
{
    // Singleton for storing the current player's score
    public class ScoringSystem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText = null;
        [SerializeField] bool enableScoreReduction = false;
        public static int theScore;

        private float elapsed = 0f;
        private float scoreReductionInterval = 0.25f;

        public static ScoringSystem instance;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Update()
        {
            // If enabled, each player's score will be reduced over time
            // Players with higher scores will lose points a bit faster.
            if (enableScoreReduction)
            {
                // Decrement the user's score
                elapsed += Time.deltaTime;
                if (elapsed > scoreReductionInterval)
                {
                    theScore -= (int)(1f + (1f * elapsed * theScore / 1000));
                    elapsed = 0f;
                }
            }

            // Display the score in the UI
            scoreText.text = "Score: " + theScore;
        }
    }
}