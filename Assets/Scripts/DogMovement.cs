using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogMovement : MonoBehaviour
{
    [Header("Movement settings")]
    public float walkSpeed = 10f;
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
    public Transform camPlace;
    public float sensitivity;
    public float maxY, maxX;

    [Header("Movement data")]
    public float curSpeed;
    public bool Running;
    public Vector3 moveDebugVector;
    public float useVelChange;

    [Header("Camera data")]
    public Vector2 camRotation;
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
        // Calculate which direction and at what speed we'd like to move
        Vector3 targetVel = transform.TransformDirection(moveInputVec.x, 0, moveInputVec.y);
        targetVel *= curSpeed;
        rb.AddForce(targetVel);
        float gravityBeforeClamp = rb.velocity.y;

        // Clamp the speed
        Vector3 vel = rb.velocity;
        Vector3 clampedMovement = new Vector3(vel.x, 0, vel.z);
        clampedMovement = Vector3.ClampMagnitude(clampedMovement, useVelChange);
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, useVelChange);
        rb.velocity = new Vector3(clampedMovement.x, vel.y, clampedMovement.x);


        // Add gravity onto rb
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

        // Handle camera control
        float mouseX = camInputVec.x * sensitivity * Time.deltaTime;
        float mouseY = camInputVec.y * sensitivity * Time.deltaTime;
        camRotation += new Vector2(mouseX, -mouseY);
        camRotation.y = Mathf.Clamp(camRotation.y, -maxY, maxY);
        //camRotation.x = Mathf.Clamp(camRotation.x, transform.rotation.x - maxX, transform.rotation.x + maxX);
        playerCam.rotation = Quaternion.Euler(camRotation.y, camRotation.x, 0);

        // Rotate playerbod and set cam position
        transform.eulerAngles = new Vector3(0, playerCam.eulerAngles.y, 0);
        playerCam.position = camPlace.position;
    }

    public void RunInputDetection(InputAction.CallbackContext value)
    {
        Running = value.ReadValue<float>() > 0 ? true : false;
        if (Running) { curSpeed = runSpeed; useVelChange = maxVelChange * runSpeed / walkSpeed; }
        else { curSpeed = walkSpeed; useVelChange = maxVelChange; }
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
