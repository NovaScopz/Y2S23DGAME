using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //player variables
    public float moveSpeed = 5f; //movement
    public float rotationSpeed = 100f; // rotation
    public float jumpForce = 15f; // jump force
    public Transform cameraTransform; // camera reference for relative movement
    public bool pickupItem = false; //holding item
    private Rigidbody rb;
    private bool isGrounded = false;
    public int inventoryItems = 0;
    public int jumpsRemaining = 2; // double jump variable
    


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
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpsRemaining > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsRemaining--; // Decrease jumps remaining for double jump
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
        if (other.CompareTag("item") && !pickupItem && Keyboard.current.eKey.wasPressedThisFrame && inventoryItems < 3) //pickup requirements of trigger, button and inventory cap 3
        {
            pickupItem = true;
            Destroy(other.gameObject); 
            Debug.Log("Item picked up!");
            inventoryItems++;

        }
    }   

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded.");
            jumpsRemaining = 2; // reset jumps when grounded
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            jumpsRemaining = 1; // disable double jump by changing jumps on leaving ground
            Debug.Log("Player is not grounded.");
        }
    }
}
