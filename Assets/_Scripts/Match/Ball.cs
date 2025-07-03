namespace FootballProject
{
    using FootballProject.Audio;
    using UnityEngine;

    public class Ball : MonoBehaviour
    {
        public Transform player;
        public float kickForce = 400f;
        public float passForce = 200f;

        private Rigidbody rb;
        private PlayerController playerController;
        private Controls controls;

        #region Unity API
        private void Awake()
        {
            controls = new Controls();
            controls.PlayerControls.Kick.performed += ctx => TryKick();
            controls.PlayerControls.Pass.performed += ctx => TryPass();
        }
        private void OnEnable()
        {
            controls.Enable();
            controls.PlayerControls.Enable();
        }
        private void OnDisable()
        {
            controls.Disable();
            controls.PlayerControls.Disable();
        }
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            playerController = player.GetComponent<PlayerController>();
        }
        #endregion

        #region Private Methods
        private void TryKick()
        {
            if (!playerController.hasBall) return;

            Vector3 direction = (transform.position - player.position).normalized;
            rb.AddForce(direction * kickForce);
            MatchAudio.Current.audioManager.Play("PlayerShoot", transform.position);
        }
        private void TryPass()
        {
            if (!playerController.hasBall) return;

            Vector3 direction = (transform.position - player.position).normalized;
            rb.AddForce(direction * passForce);
            MatchAudio.Current.audioManager.Play("PlayerPass", transform.position);
        }
        #endregion

    }
}
