using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private InputActionAsset input;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private string mapName = "Player1";

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction emote1;
    private InputAction emote2;

    private Rigidbody rb;
    [SerializeField] private bool isGrounded = false;

    private Animator animator;

    void Awake()
    {
        InputActionMap map = input.FindActionMap(mapName);
        moveAction   = map.FindAction("Move");
        jumpAction   = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
        emote1 = map.FindAction("Emote1");
        emote2 = map.FindAction("Emote2");
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()  { input.FindActionMap(mapName).Enable(); }
    void OnDisable() { input.FindActionMap(mapName).Disable(); }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        float speed = walkSpeed * moveInput.y;

        if (sprintAction.IsPressed())
            speed *= 2f;

        Vector3 movement = transform.forward * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        float angle = moveInput.x * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, angle, 0f, Space.World);

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            Debug.Log("Jump pressed");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("JumpTrigger");
        }

        animator.SetFloat("Speed", speed);
        animator.SetBool("Grounded", isGrounded);

        if(emote1.WasPressedThisFrame())
        {
            animator.SetTrigger("emote1trigger");
        }
        if(emote2.WasPressedThisFrame())
        {
            animator.SetTrigger("emote2trigger");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}