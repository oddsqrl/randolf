using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DogMovement : MonoBehaviour
{
    public GameData gameData;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI objectiveText;
    public bool hasMoved = false;

    [Header("Movement settings")]
    public float walkSpeed = 10f;
    public float runSpeed;
    //public float turnRadius;
    //public float turnSpeed;

    public float gravity = 10.0f;
    public float maxVelChange = 10.0f;
    public float jumpHeight = 2.0f;
    public float groundDistance;
    public float sphereCastLength;
    public float sphereRadius;

    [Header("Run data")]
    public Image fillImage;
    public float stamina;
    public float staminaRegen;
    public float staminaDrain;

    [Header("Camera settings")]
    public Transform playerCam;
    public Transform camPlace;
    public float sensitivity;
    public float maxY, maxX;

    [Header("Movement data")]
    public bool isSprinting;
    public float curSpeed;
    public Vector3 moveDebugVector;
    public float curStamina;
    public float useVelChange;
    public bool canJump = false;
    public bool jumping = false;
    public float curGroundDist;
    

    [Header("Camera data")]
    public Vector2 camRotation;
    private Rigidbody rb;
    public Vector2 moveInputVec;
    public Vector2 camInputVec;

    void Start()
    {
        gameData.ResetTime();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        curSpeed = walkSpeed;
        useVelChange = maxVelChange;

        curStamina = stamina;
    }

    private void FixedUpdate()
    {
        // Set direction and correct speed towards that direction
        Vector3 targetVel = transform.TransformDirection(moveInputVec.x, 0, moveInputVec.y) * curSpeed;
        rb.AddForce(targetVel);
        //rb.velocity.Set(targetVel.x, rb.velocity.y, targetVel.z);
        //rb.velocity *= 0.99f;
        //Clamp magnitude didn't want to gravity itself lol
        // Add gravity onto rb
        float curGravity = rb.velocity.y * 0.9f + gravity;
        rb.AddForce(-transform.up * curGravity);

        if (jumping)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            jumping = false;
        }

    }

    void Update()
    {
        if (hasMoved)
        {
            gameData.StartTimer(Time.time);
            objectiveText.enabled = false;
            gameData.Timer(Time.time);
            timeText.text = gameData.TimeFormat(gameData.curTime);
        }

        // Handle camera control
        sensitivity = gameData.sensitivity;
        playerCam.GetComponent<Camera>().fieldOfView = gameData.FOV;
        float mouseX = camInputVec.x * sensitivity * Time.deltaTime;
        float mouseY = camInputVec.y * sensitivity * Time.deltaTime;
        camRotation += new Vector2(mouseX, -mouseY);
        camRotation.y = Mathf.Clamp(camRotation.y, -maxY, maxY);
        playerCam.rotation = Quaternion.Euler(camRotation.y, camRotation.x, 0);

        // Rotate playerbod and set cam position
        transform.eulerAngles = new Vector3(0, playerCam.eulerAngles.y, 0);
        playerCam.position = camPlace.position;

        canJump = groundCheck();
        RunHandle();
    }

    public void RunHandle()
    {
        if (isSprinting && moveInputVec.magnitude > 0)
        { curStamina -= staminaDrain * Time.deltaTime; }
        else { curStamina += staminaRegen * Time.deltaTime; }
        curStamina = Mathf.Clamp(curStamina, 0, stamina);

        fillImage.fillAmount = curStamina / stamina;
    }


    public bool groundCheck()
    {
        bool temp = false;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereRadius, -transform.up, out hit, sphereCastLength))
        {
            temp = hit.distance < groundDistance ? true : false;
            curGroundDist = hit.distance;
            
        }
        return temp;
    }

    public void JumpInputDetection(InputAction.CallbackContext value)
    {
        bool jump = value.ReadValue<float>() > 0 ? true : false;
        if(jump && canJump) jumping = true;
    }

    public void RunInputDetection(InputAction.CallbackContext value)
    {
        isSprinting = value.ReadValue<float>() > 0 ? true : false;
        if (isSprinting) 
        { 
            curSpeed = runSpeed; useVelChange = maxVelChange * runSpeed / walkSpeed; 
        }
        else 
        { 
            curSpeed = walkSpeed; useVelChange = maxVelChange; 
        }
    }

    public void MoveInputDetection(InputAction.CallbackContext value)
    {
        moveInputVec = value.ReadValue<Vector2>();
        hasMoved = true;
    }

    public void LookInputDetection(InputAction.CallbackContext value)
    {
        camInputVec = value.ReadValue<Vector2>();
        //check input device type and enable "gamepad" bool
        //var gamepad = Gamepad.current;
        //if (gamepad) { camInputVec *= addedSensitivity; Debug.Log("joyish"); }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower() == "vacuum")
        {
            Debug.Log("Collided");
            gameData.EndTimer();
            gameData.SSGameOver();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower() == "finish")
        {
            Debug.Log("Triggered");
            gameData.EndTimer(true);
            gameData.SSLevelComplete();
        }
    }
}
