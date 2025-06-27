namespace FootballProject
{
    using UnityEngine;

    public class Ball : MonoBehaviour
    {
        public Transform player;
        public float kickForce = 400f;
        public float passForce = 200f;

        private Rigidbody rb;
        private PlayerController playerController;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            playerController = player.GetComponent<PlayerController>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && playerController.hasBall)
            {
                Vector3 direction = (transform.position - player.position).normalized;
                rb.AddForce(direction * kickForce);
            }

            else if (Input.GetMouseButtonDown(1) && playerController.hasBall)
            {
                Vector3 direction = (transform.position - player.position).normalized;
                rb.AddForce(direction * passForce);
            }
        }
    }
}
