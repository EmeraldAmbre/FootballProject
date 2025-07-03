using UnityEngine;

namespace FootballProject
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform player;
        public float smoothSpeed = 10f;

        private Vector3 offset;
        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            offset = transform.position - player.position;
        }

        private void FixedUpdate()
        {
            if (player == null) return;

            Vector3 targetPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.05f);
        }
        public void SetTarget(Transform newTarget)
        {
            player = newTarget;
            offset = transform.position - player.position;
        }
    }
}
