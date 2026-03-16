using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //player variables
    public float moveSpeed = 5f; //movement
    public float rotationSpeed = 100f; // rotation
    public float jumpForce = 5f; // jump force
    public Transform cameraTransform; // camera reference for relative movement
    public bool pickupItem = false; //holding item
    private Rigidbody rb;
    private bool isGrounded = false;
    


    void Start()
    {

        rb = GetComponent<Rigidbody>();

        // mouselock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Player started");

    }

    void Update()
    {
        // movement relative to camera direction
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        Vector3 movement = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) movement += forward;
        if (Keyboard.current.sKey.isPressed) movement -= forward;
        if (Keyboard.current.aKey.isPressed) movement -= right;
        if (Keyboard.current.dKey.isPressed) movement += right;




        if (movement != Vector3.zero)
        {
            movement = movement.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z); // Preserve Y velocity for gravity
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // Stop horizontal movement
        }
        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //capsule upright by preserving yaw only
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        //mouseunlock on ESC
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // pickup starter
        if (other.CompareTag("item") && !pickupItem)
        {
            pickupItem = true;
            Destroy(other.gameObject); //change for throw and carry mechanics later
            Debug.Log("Item picked up!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded.");

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            Debug.Log("Player is not grounded.");
        }
    }
}
