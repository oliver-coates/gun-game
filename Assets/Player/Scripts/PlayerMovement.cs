using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Image blackout;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [Range(1.0f,10.0f)]
    public float playerSpeed = 8.0f;
    [Range(0f,5f)]
    public float jumpHeight = 1.0f;
    [Range(-20f, -1f)]
    public float gravityValue = -9.81f;

    public float sprintSpeedMultiplier;
    public bool isSprinting;

    private Vector3 move;

    //the camera pivot and mouse movements stuff
    public Transform camPivot;
    [Range(0.01f, 10f)]
    public float mouseSensitivity = 1f;
    float pitch = 0f;

    public float footstepMoveThreshold = 0.4f;
    private float footstepMoveTimer;

    public List<AudioClip> footstepSounds;
    public AudioSource audioSource;

    private void Start()
    {
        blackout.enabled = true;
        blackout.CrossFadeAlpha(0, 1f, true);

        controller =  GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        bool playerMoving = (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f);

        if (Input.GetKey(KeyCode.LeftShift) && playerMoving)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        footstepMoveTimer -= Time.deltaTime;
        if (isSprinting)
        {
            footstepMoveTimer -= Time.deltaTime * 0.5f;            
        }


        if (playerMoving)
        {
            if (footstepMoveTimer < 0)
            {
                // Play footstep sound

                AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
                audioSource.PlayOneShot(clip);

                footstepMoveTimer = footstepMoveThreshold;
            }
        }

        move = Vector3.Lerp(move, transform.TransformVector( new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))),0.1f);
        
        float speedMultiplier = 0f;
        if (isSprinting)
        {
            speedMultiplier = playerSpeed * sprintSpeedMultiplier;
        }
        else
        {
            speedMultiplier = playerSpeed;
        }
        
        controller.Move(move * Time.deltaTime * speedMultiplier);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);


        //the look rotation stuff
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);

        //pitch now
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        camPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);


    }
}
