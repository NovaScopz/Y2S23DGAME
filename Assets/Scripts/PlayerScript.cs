using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    public bool pickupItem = false; //holding item
    private Rigidbody rb;

    public int inventoryItems = 0;
    public int itemsLeft = 5; // for win condition
    // public int jumpsRemaining = 2; // double jump variable *MOVED TO PLAYER ARM SETUP*

    [SerializeField] private TextMeshProUGUI inventoryDisplay;
    [SerializeField] private TextMeshProUGUI itemsLeftDisplay;
    [SerializeField] private TextMeshProUGUI instructions;
    [SerializeField] private TextMeshProUGUI interactionText;
    


    void Start()
    {

        rb = GetComponent<Rigidbody>();

        // mouselock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Player started");

    }


    void OnTriggerStay(Collider other)
    {
        Debug.Log("Item in range");
        // pickup starter
        if (other.CompareTag("item") && inventoryItems < 2) //pickup requirements of trigger, button and inventory cap 2
        {
            pickupItem = true;
            Destroy(other.gameObject); 
            Debug.Log("Item picked up");
            inventoryItems++;
            inventoryDisplayItemAdded();

        } else if (other.CompareTag("item") && inventoryItems >= 2)
        {
            Debug.Log("Inventory full Cannot pick up more items");
        } else if (other.CompareTag("bin") && inventoryItems > 0) //pickup requirements of trigger, button and already holding item
        {

            pickupItem = false;
            itemsLeft = itemsLeft - inventoryItems; // decrease items left by how many were in inventory
            inventoryItems = 0;
            inventoryDisplayItemAdded();
            Debug.Log("Items deposited");
        }
    }   



    public void inventoryDisplayItemAdded()
    {
        inventoryDisplay.text = inventoryItems + "/2";
        itemsLeftDisplay.text = "Items Left: " + itemsLeft;
        instructions.enabled = false;
    }

}
