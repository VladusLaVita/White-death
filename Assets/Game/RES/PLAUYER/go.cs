
using UnityEngine;
using UnityEngine.InputSystem;
// using Cinemachine;

public class Go : MonoBehaviour
{
    public InputActionAsset inputActions;
    public float jump_speed = 5f;
    private InputAction a_move;
    private InputAction a_look;
    private InputAction a_jump;

    private Vector2 move;
    private Vector2 look;
    private Vector3 moveDir;
    private Animator animator;
    public float speedDifference = 2;
    public float walkspeed = 5;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float rotationSpeed = 5;
    public Camera cam;
    public CharacterController controller;
    private bool isGrounded;
    private Vector3 velocity;
    private Vector3 desiredMoveDirection = Vector3.zero;
    private Rigidbody rb;

    private void Awake()
    {
        a_move = InputSystem.actions.FindAction("Move");
        a_look = InputSystem.actions.FindAction("Look");
        a_jump = InputSystem.actions.FindAction("Jump");

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;

        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        isGrounded = controller.isGrounded;
        move = a_move.ReadValue<Vector2>();
        look = a_look.ReadValue<Vector2>();

        if (a_jump.WasPressedThisFrame() && isGrounded)
        {
            Jump();

        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded) {
            Run(true);
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Run(false);
        }
    }


    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        animator.SetTrigger("jump");
        

        controller.Move(moveDir * Time.deltaTime * jump_speed);
        rb.AddForce(moveDir * jump_speed);
    }
    private void FixedUpdate()
    {
        Walking();
        Rotating();
        Gravity();
    }
    private void Gravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // небольшой "прижим" к земле
        }
        velocity.y += gravity * UnityEngine.Time.deltaTime;
        controller.Move(velocity * UnityEngine.Time.deltaTime);
    }
    private void Walking()
    {
        // animator.SetFloat("forward", move.y);
        // animator.SetFloat("right", move.x);
        // Vector3 move2 = (transform.forward * move.y + transform.right * move.x).normalized * walkspeed;
        // controller.Move(move2 * Time.deltaTime);
        float inputMagnitude = new Vector2(move.x, move.y).magnitude;
        inputMagnitude = Mathf.Clamp01(inputMagnitude); // ограничиваем до [0, 1]

        animator.SetFloat("forward", inputMagnitude);
        animator.SetFloat("right", 0f);

        moveDir = desiredMoveDirection * inputMagnitude * walkspeed;
        controller.Move(moveDir * UnityEngine.Time.deltaTime);
    }

    //     private void Rotating()
    //     {
    //         if (move.x != 0 || move.y != 0)
    //         {
    //             Vector3 targetPos = cam.transform.position + cam.transform.forward * 100;
    //             transform.LookAt(new Vector3(targetPos.x, targetPos.y, targetPos.z));
    //             transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
    //         }
    // }
 
    private void Rotating()
    {
        if (move.x != 0 || move.y != 0)
        {
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = forward * move.y + right * move.x;
            moveDir.y = 0f;
            moveDir.Normalize();

            desiredMoveDirection = moveDir;

            // Плавный поворот к этому направлению
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * UnityEngine.Time.deltaTime
            );
        }
        else
        {
            desiredMoveDirection = Vector3.zero;
        }
    }

    private void Run(bool flag) {
        animator.SetBool("runnning", flag);
        float tempSpeed = walkspeed;
        if (flag && isGrounded) walkspeed += speedDifference;
        else if (isGrounded) walkspeed -= speedDifference;
    }
}
