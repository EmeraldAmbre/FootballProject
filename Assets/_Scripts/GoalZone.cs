using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace FootballProject
{
    public class GoalZone : MonoBehaviour
    {
        public Transform ballSpawnPoint;
        public Rigidbody ball;
        public string teamName;

        public TextMeshProUGUI scoreText;

        private int score = 0;

        private void Awake()
        {
            score = 0;
            scoreText.text = teamName + " : " + score;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                score++;
                UpdateScore();
                ResetBall();
            }
        }

        void UpdateScore()
        {
            if (scoreText != null)
            {
                scoreText.text = teamName + " : " + score;
            }
        }

        void ResetBall()
        {
            ball.velocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
            ball.transform.position = ballSpawnPoint.position;
        }
    }
}
