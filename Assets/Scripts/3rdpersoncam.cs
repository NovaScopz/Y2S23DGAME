using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;

    public Rigidbody rbody;

    public float rotationspeed;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){
        //calc view direction
        Vector3 viewdirect = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewdirect.normalized;

        //
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputdirect = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if(inputdirect != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputdirect.normalized, Time.deltaTime * rotationspeed);
        }

    }
}
