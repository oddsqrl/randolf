using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogMovement : MonoBehaviour
{
    [Header("Movement settings")]
    public float speed = 10f;
    public float runSpeed;
    //public float turnRadius;
    //public float turnSpeed;

    public float gravity = 10.0f;
    public float maxVelChange = 10.0f;
    public float groundDistance;
    public LayerMask ignoreLayer;
    public bool canJump = false;
    public float jumpHeight = 2.0f;


    [Header("Camera settings")]
    public Transform playerCam;
    public float sensitivity;
    public float maxY, maxX;
    public float addedSensitivity;

    [Header("Movement data")]
    public Vector3 moveDebugVector;

    [Header("Camera data")]
    public Vector3 camDebugVector;

    private Rigidbody rb;
    public Vector2 moveInputVec;
    public Vector2 camInputVec;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canJump)
        {
            // Calculate which direction and at what speed we'd like to move
            Vector3 targetVel = transform.TransformDirection(moveInputVec.x, 0, moveInputVec.y);
            targetVel *= speed;

            rb.AddForce(targetVel);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelChange);
        }
        
        // Add gravity onto rb
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));


        
        float mouseX = camInputVec.x * sensitivity * Time.deltaTime;
        float mouseY = camInputVec.y * sensitivity * Time.deltaTime;
        camDebugVector += new Vector3(mouseX, -mouseY, 0);
        playerCam.localRotation = Quaternion.Euler(camDebugVector.y, camDebugVector.x, 0);
    }

    public void MoveInputDetection(InputAction.CallbackContext value)
    {
        moveInputVec = value.ReadValue<Vector2>();
    }

    public void LookInputDetection(InputAction.CallbackContext value)
    {
        camInputVec = value.ReadValue<Vector2>();
        //check input device type and enable "gamepad" bool
        //var gamepad = Gamepad.current;
        //if (gamepad) { camInputVec *= addedSensitivity; Debug.Log("joyish"); }
    }
}
