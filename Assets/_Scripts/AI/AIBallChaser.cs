using FootballProject.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FootballProject.AI
{
    public class AIBallChaser : MonoBehaviour
    {
        public Transform ball;
        public Transform goal;
        public float kickForce = 300f;
        private NavMeshAgent agent;

        private bool isEnabled = false;

        public void SetActiveAI(bool active)
        {
            isEnabled = active;
            GetComponent<NavMeshAgent>().enabled = active;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            if (!isEnabled) return;

            if (ball != null)
                agent.SetDestination(ball.position);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                TryKickBall();
            }
        }
        private void TryKickBall()
        {
            if (Random.value <= 0.6f)
            {
                Vector3 direction = (goal.position - ball.position).normalized;
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();
                ballRb.AddForce(direction * kickForce, ForceMode.Impulse);
                MatchAudio.Current.audioManager.Play("PlayerShoot", transform.position);
            }
        }
    }
}
