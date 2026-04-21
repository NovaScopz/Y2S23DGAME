using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] private GameObject blackScreen;

    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private AudioSource depositSound;
    [SerializeField] private AudioSource victorySound;



    void Start()
    {

        rb = GetComponent<Rigidbody>();

        // mouselock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Player started");
        blackScreen.SetActive(false);

    }


    void OnTriggerStay(Collider other)
    {
        if (pickupItem) return;

        Debug.Log("Item in range");

        // pickup starter adn ignor funct
        if (other.CompareTag("item") && inventoryItems < 2 && !pickupItem) //pickup requirements of trigger, button and inventory cap 2
        {
            interactionText.text = "[E]";
            if(Keyboard.current.eKey.wasPressedThisFrame)
            {
                pickupItem = true;
                Invoke("pickupCool", 1); //cooldown using my boolean
                if(pickupSound != null) pickupSound.Play();
                Destroy(other.gameObject); 
                Debug.Log("Item picked up");
                inventoryItems++;
                inventoryDisplayItemAdded();
                interactionText.text = "";
                
            }
            

        } else if (other.CompareTag("item") && inventoryItems >= 2)
        {
            Debug.Log("Inventory full Cannot pick up more items");
            interactionText.text = "FULL! GO DEPOSIT";
        } else if (other.CompareTag("bin") && inventoryItems > 0) //pickup requirements of trigger, button and already holding item
        {
            interactionText.text = "[E]";
            if(Keyboard.current.eKey.wasPressedThisFrame){
                
                pickupItem = true;
                itemsLeft = itemsLeft - inventoryItems; // decrease items left by how many were in inventory
                
                inventoryItems = 0;
                inventoryDisplayItemAdded();
                if(itemsLeft == 0)
                {
                    gameEndSt();
                }
                Debug.Log("Items deposited");
                if(depositSound != null) depositSound.Play();
                interactionText.text = "DEPOSITED";
                Invoke("pickupCool", 2);
                
            }
        }else if (other.CompareTag("bin") && inventoryItems > 0) //pickup requirements of trigger, button and already holding item
        {
            interactionText.text = "GO GET SOME RUBBISH";
        }   
        
    }

    public void OnTriggerExit(Collider other)
    {
        interactionText.text = "";
    }



    public void inventoryDisplayItemAdded()
    {
        inventoryDisplay.text = inventoryItems + "/2";
        itemsLeftDisplay.text = "" + itemsLeft;
        instructions.enabled = false;
    }
    void pickupCool()
    {
        pickupItem=false;
    }

    public void gameEndSt()
    {
        blackScreen.SetActive(true);
        interactionText.enabled = false;
        instructions.text = "CONGRATULATIONS! By cleaning up the rubbish, you have contributed to Sustainable Development Goal 15: Life on land!";
        instructions.enabled = true;
        if(victorySound != null) victorySound.Play();
    }

}
