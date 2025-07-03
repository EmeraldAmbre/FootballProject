using FootballProject.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FootballProject.AI
{
    public class AISupport : MonoBehaviour
    {
        [Header("Navigation")]
        public Vector3 zoneMin;
        public Vector3 zoneMax;
        public float patrolInterval = 3f;

        [Header("Ball & Kick")]
        public Transform ball;
        public Transform chaser;
        public float passForce = 100f;

        private NavMeshAgent agent;
        private float nextPatrolTime = 0f;

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

            if (Time.time >= nextPatrolTime)
            {
                Vector3 target = GetRandomPointInZone();
                agent.SetDestination(target);
                nextPatrolTime = Time.time + patrolInterval;
            }
        }

        private Vector3 GetRandomPointInZone()
        {
            float x = Random.Range(zoneMin.x, zoneMax.x);
            float z = Random.Range(zoneMin.z, zoneMax.z);
            float y = transform.position.y;
            return new Vector3(x, y, z);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                TryPassToChaser();
            }
        }

        private void TryPassToChaser()
        {
            if (Random.value <= 0.6f)
            {
                Vector3 direction = (chaser.position - ball.position).normalized;
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();
                ballRb.AddForce(direction * passForce, ForceMode.Impulse);
                MatchAudio.Current.audioManager.Play("PlayerPass", transform.position);
            }
        }
    }
}
