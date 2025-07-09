using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootballProject
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float acceleration = 10f;
        public float deceleration = 5f;
        public float jumpForce = 5f;
        public float rotationSpeed = 10f;

        [Header("Kick")]
        public float kickForce = 10f;
        public float kickUpwardForce = 2f;

        private Rigidbody rb;
        private bool isGrounded;
        private Transform cameraTransform;
        private Vector3 moveInput;
        private Vector3 moveDirection;
        private Controls controls;

        [Header("Apparence")]
        public GameObject[] decorations, eyes, mouths, hairs, all;

        public bool hasBall = false;
        private bool isStuck = false;
        
        public bool isActive = false;

        #region Unity API
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
        }
        private void Awake()
        {
            controls = new Controls();

            controls.PlayerControls.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.PlayerControls.Move.canceled += ctx => moveInput = Vector2.zero;
            controls.PlayerControls.Jump.performed += ctx => Jump();
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
        private void OnGUI()
        {
            if (GUILayout.Button("Change Character Appearance"))
            {
                ChangeCloth();
            }
        }
        [ContextMenu(nameof(ChangeCloth))]
        private void Update()
        {
            ProcessInput();
            if (!isStuck && isActive)
            {
                RotateCharacter();
            }
        }
        private void FixedUpdate()
        {
            if (!isActive) return;
            if (!isStuck)
            {
                Move();
            }
        }
        #endregion

        #region Public Methods
        public void ChangeCloth()
        {
            foreach (var item in all)
            {
                item.SetActive(false);
            }
            decorations[Random.Range(0, decorations.Length)].SetActive(true);
            eyes[Random.Range(0, eyes.Length)].SetActive(true);
            mouths[Random.Range(0, mouths.Length)].SetActive(true);
            hairs[Random.Range(0, hairs.Length)].SetActive(true);
        }
        #endregion

        #region Private Methods
        private void ProcessInput()
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        }
        private void Move()
        {
            Vector3 targetVelocity = moveDirection * moveSpeed;
            if (!isGrounded)
            {
                targetVelocity.y = rb.velocity.y;
            }

            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, (isGrounded ? acceleration : deceleration) * Time.fixedDeltaTime);
        }
        private void RotateCharacter()
        {
            if (moveDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        private void Jump()
        {
            if (!isGrounded || rb == null) return;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        public void SetStuck(bool stuck)
        {
            if (stuck is true)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            isStuck = stuck;
        }
        #endregion

        #region Collisions
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ground ground))
            {
                isGrounded = true;
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ground ground))
            {
                isGrounded = false;
            }
            if (collision.gameObject.TryGetComponent(out Ball ball))
            {
                hasBall = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ball ball))
            {
                hasBall = true;
            }
        }
        #endregion
    }
}
