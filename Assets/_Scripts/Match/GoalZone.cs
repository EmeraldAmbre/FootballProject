using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace FootballProject
{
    public class GoalZone : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        private int score = 0;

        private MatchManager matchManager;

        [SerializeField] Transform ballSpawnPoint;
        [SerializeField] Rigidbody ball;

        private void Awake()
        {
            score = 0;
            scoreText.text = "" + score;
        }
        private void Start()
        {
            matchManager = FindObjectOfType<MatchManager>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                score++;
                UpdateScore();
                ResetBall();
                matchManager.OnGoalScored();
            }
        }
        private void UpdateScore()
        {
            if (scoreText != null)
            {
                scoreText.text = "" + score;
            }
        }
        private void ResetBall()
        {
            ball.velocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
            ball.transform.position = ballSpawnPoint.position;
        }

    }
}
