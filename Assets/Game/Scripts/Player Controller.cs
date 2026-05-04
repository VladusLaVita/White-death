using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed = 5f;
    public float maxSpeed = 10f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float rotationSmooth = 10f;

    [Header("Pickup")]
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private float inputMagnitude;
    private bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;
    }

    void Update()
    {
        HandleMovement();
        HandlePickup();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
            velocity.y = -5f;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        inputMagnitude = Mathf.Clamp01(new Vector2(inputX, inputZ).magnitude);

        Vector3 move = Vector3.zero;

        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            move = (right * inputX + forward * inputZ).normalized;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float speed = isRunning ? maxSpeed : playerSpeed;

        velocity.y += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        Vector3 motion = move * speed + Vector3.up * velocity.y;
        controller.Move(motion * Time.deltaTime);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
        }
    }

    private void HandlePickup()
    {
        if (!Input.GetKeyDown(pickupKey))
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, pickupLayer);
        if (hits.Length == 0)
            return;

        Collider closest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit;
            }
        }

        if (closest == null)
            return;

        IPickup item = closest.GetComponent<IPickup>();
        if (item == null)
            return;

        animator.SetTrigger("pickup");
        item.OnPickup();
    }

    private void HandleAnimation()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        animator.SetBool("running", isRunning && inputMagnitude > 0f);
        animator.SetFloat("forward", Mathf.Ceil(inputMagnitude));
    }
public AudioSource footstepSource;
public AudioClip[] footstepSounds;

[Header("Настройки вариативности")]
public float minPitch = 0.85f;
public float maxPitch = 1.15f;
public float minVolume = 0.4f;
public float maxVolume = 0.6f;

void Step() 
{
    if (footstepSounds.Length > 0)
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];

        footstepSource.pitch = Random.Range(minPitch, maxPitch);

        float randomVolume = Random.Range(minVolume, maxVolume);
        footstepSource.PlayOneShot(clip, randomVolume);
    }
}

public interface IPickup
{
    void OnPickup();
}