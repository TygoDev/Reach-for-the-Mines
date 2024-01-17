using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 0.05f;
    [SerializeField] private float sprintMultiplier = 1.2f;
    [SerializeField] private float movementSpeed = 0.05f;
    [SerializeField] private float rotationSpeedHorizontal = 50f;
    [SerializeField] private float rotationSpeedVertical = 10f;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private GameObject followTarget = null;
    [SerializeField] private Transform groundedCheck = null;

    private Systems systems = default;
    private Vector3 movementInput = Vector3.zero;
    private Vector2 cameraAngle = Vector2.zero;
    private Rigidbody rigidBody = null;
    private Vector3 oldMovementInput = new Vector3();

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        systems = Systems.Instance;
        transform.position = systems.spawnpoint;        
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.moveEvent += OnMove;
        systems.inputManager.mouseRotateEvent += OnCameraRotate;
        systems.inputManager.jumpEvent += OnJump;
        systems.inputManager.sprintEvent += OnSprint;
        systems.inputManager.sprintCanceledEvent += OnSprintCanceled;

        EventBus<UnStuckEvent>.OnEvent += ResetPosition;
    }

    private void OnDisable()
    {
        systems.inputManager.moveEvent -= OnMove;
        systems.inputManager.mouseRotateEvent -= OnCameraRotate;
        systems.inputManager.jumpEvent -= OnJump;
        systems.inputManager.sprintEvent -= OnSprint;
        systems.inputManager.sprintCanceledEvent -= OnSprintCanceled;

        EventBus<UnStuckEvent>.OnEvent -= ResetPosition;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void FixedUpdate()
    {
        CalculateCustomGravity();
        CalculatePlayerMovement();
        CalculateFollowTargetPosition();
    }

    private void CalculatePlayerMovement()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 newPosition = new Vector3();

        if (IsGrounded())
        {
            newPosition = transform.position +
            (forwardDirection * movementInput.z * movementSpeed) +
            (transform.right * movementInput.x * movementSpeed);
            oldMovementInput = movementInput;
        }
        else
        {
            newPosition = transform.position +
            (forwardDirection * oldMovementInput.z * movementSpeed) +
            (transform.right * oldMovementInput.x * movementSpeed);
        }

        rigidBody.MovePosition(newPosition);
    }

    private void CalculateCustomGravity()
    {
        rigidBody.AddForce(Vector3.down * Physics.gravity.magnitude * 5, ForceMode.Acceleration);
    }

    private void CalculateFollowTargetPosition()
    {
        followTarget.transform.position = transform.position;
    }

    private void RotateCamera()
    {
        // Rotate camera by rotating the follow target
        followTarget.transform.Rotate(Vector3.up * cameraAngle.x * rotationSpeedHorizontal * Time.deltaTime);
        followTarget.transform.Rotate(Vector3.right * cameraAngle.y * rotationSpeedVertical * Time.deltaTime);

        // Clamp camera rotation angle
        Vector3 angles = followTarget.transform.localEulerAngles;
        angles.z = 0f;
        float angle = followTarget.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTarget.transform.localEulerAngles = angles;

        // Allow camera to rotate around character when the character is stood still
        if (movementInput != Vector3.zero)
            transform.rotation = Quaternion.Euler(0, followTarget.transform.eulerAngles.y, 0);
    }
    private bool grounded = false;
    private bool IsGrounded()
    {
        Vector3 raycastOrigin = groundedCheck.transform.position;
        Vector3 raycastDirection = Vector3.down;
        float raycastDistance = 0.1f;
        grounded = Physics.Raycast(raycastOrigin, raycastDirection, raycastDistance);
        if (grounded)
            return true;
        else
            return false;
    }

    // EVENT LISTENERS

    private void ResetPosition(UnStuckEvent myEvent)
    {
        transform.position = systems.spawnpoint;
    }

    private void OnMove(Vector2 action)
    {
        movementInput = new Vector3(action.x, 0, action.y);
    }

    private void OnCameraRotate(Vector2 action)
    {
        cameraAngle = action;
    }

    private void OnJump()
    {
        if (IsGrounded())
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnSprint()
    {
        if (IsGrounded())
            movementSpeed *= sprintMultiplier;
        else
            movementSpeed = defaultSpeed;
    }

    private void OnSprintCanceled()
    {
        movementSpeed = defaultSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f);
    }
}
