using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    Rigidbody rb;
    Vector3 movement_Key_Input;
    GameMaster gm;

    [SerializeField] private float player_MaxSpeed = 1f;
    [SerializeField] private float player_Acceleration = 1f;
    [SerializeField] private float player_Deceleration = 0.8f;
    

    [SerializeField] private float jump_Speed = 0.8f;
    [SerializeField] private float gravity_Coeff = 0.8f;
     private bool isAttemptingToJump = false;
    private float lastGroundedTime; // Time since last grounded
    private float lastJumpAttemptTime; // Time since jump action was last triggered

    // Coyote Time and Jump Buffer Times
    public float coyoteTime = 0.2f; // Time in seconds player can still jump after leaving the ground
    public float jumpBufferTime = 0.2f; // Time in seconds before hitting the ground that the jump action is remembered


    public float groundCheckDistance = 0.1f; // Distance below the player to check for ground
    [SerializeField] LayerMask groundLayer; // Layer mask to identify the ground layer
    private bool surface_Angle;

    [SerializeField] private Animator foxanim;
    [SerializeField] private float rotation_Speed = 5f;
    [SerializeField] private bool cantJump = false;


    public AudioSource footstepAudioSource; // Assign this in the inspector
    public float stepInterval = 0.5f; // Time between steps
    private float stepTimer;


    public AudioSource jumpAudioSource;


    private void Awake() 
    {
        gm = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            if(gm.LastCheckpointPosition != Vector3.zero)
            {
                transform.position = gm.LastCheckpointPosition;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationBools();
        TurnPlayerBasedOnVelocity();
    }

    void FixedUpdate()
    {

        PlayerMovement();
        Jump();

    }


    void OnMove(InputValue value)
    {   
        //WASD Input
        Vector2 input;
        input = value.Get<Vector2>();  

        //Movement Coefficient
        movement_Key_Input = new Vector3(input.x, 0, input.y);

    }


    void PlayerMovement()
    {
        //Acceleration Calculation
        float x_acceleration = movement_Key_Input.x * player_Acceleration * Time.deltaTime;
        float z_acceleration = movement_Key_Input.z * player_Acceleration * Time.deltaTime;

        //Velocity Calculation(Acceleration)
        rb.velocity += new Vector3(x_acceleration, 0, z_acceleration);

        //Velocity Adjustment(Max Min Speed)
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -player_MaxSpeed, player_MaxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -player_MaxSpeed, player_MaxSpeed));

        //Velocity Deceleration When No Input
        if(movement_Key_Input.x == 0)
        {
            rb.velocity = new Vector3(rb.velocity.x * player_Deceleration, rb.velocity.y, rb.velocity.z);
        }
        if(movement_Key_Input.z == 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * player_Deceleration);
        }

        Debug.DrawRay(transform.position, -Vector3.up * groundCheckDistance, Color.red);

    }



    void OnJump()
    {
        if(!cantJump)
        {
            isAttemptingToJump = true;
            lastJumpAttemptTime = Time.time;
        }
    }

    void Jump()
    {
        Physics.gravity = new Vector3(0, -9.81f * gravity_Coeff, 0);

        if (IsGrounded())
        {
            Physics.gravity = new Vector3(0, 0, 0);
            lastGroundedTime = Time.time;
            if (isAttemptingToJump && Time.time - lastJumpAttemptTime <= jumpBufferTime)
            {
                rb.velocity = new Vector3(rb.velocity.x, jump_Speed, rb.velocity.z);
                JumpAudio();
                isAttemptingToJump = false; // Reset jump attempt flag
                lastGroundedTime = 0;
            }
        }
        else if (isAttemptingToJump && Time.time - lastGroundedTime <= coyoteTime)
        {
            rb.velocity = new Vector3(rb.velocity.x, jump_Speed, rb.velocity.z);
            JumpAudio();
            isAttemptingToJump = false; // Reset jump attempt flag
            lastGroundedTime = 0;
        }
    }

    private bool IsGrounded()
    {
        if(rb.velocity.y <= 0.01f)
        {
            RaycastHit hit;
            // Cast a ray straight downwards.
            bool hitGround = Physics.Raycast(transform.position, -Vector3.up, out hit, groundCheckDistance, groundLayer);

            return hitGround;
        }
        else
        {
            return false;
        }
    }



    void AnimationBools()
    {
        if(IsGrounded())
        {
            foxanim.SetBool("OnGround", true);
        }
        else
        {
            foxanim.SetBool("OnGround", false);
        }

        if(rb.velocity.y > 0)
        {
            foxanim.SetBool("GoingUp", true);
        }
        else
        {
            foxanim.SetBool("GoingUp", false);
        }

        if(movement_Key_Input.x != 0 || movement_Key_Input.z != 0)
        {
            foxanim.SetBool("Running", true);
        }
        else
        {
            foxanim.SetBool("Running", false);
        }
    }


    private void TurnPlayerBasedOnVelocity()
    {
        Vector3 velocityXZ = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Check if the velocity on the X and Z axis is significant enough to warrant a rotation
        if (velocityXZ.magnitude > 0.1f)
        {
            // Determine the direction to turn towards, based on the velocity of the Rigidbody
            Vector3 directionToTurn = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;

            // Calculate the rotation needed to face the new direction
            Quaternion targetRotation = Quaternion.LookRotation(directionToTurn);

            // Smoothly rotate the player towards the target direction
            // You can adjust the rotation speed with the third parameter of Quaternion.Lerp
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotation_Speed);

            FootstepAudio();
        }
        else
        {
            footstepAudioSource.Stop();
            stepTimer = 0f;
        }
    }



    void FootstepAudio()
    {
        if (!footstepAudioSource.isPlaying && IsGrounded())
        {
            footstepAudioSource.pitch = Random.Range(-1.5f, -1.45f);
            footstepAudioSource.Play();
        }
        if(footstepAudioSource.isPlaying && !IsGrounded())
        {
            footstepAudioSource.Stop();
        }
    }

    void JumpAudio()
    {
        jumpAudioSource.pitch = Random.Range(1.2f, 1.3f);
        jumpAudioSource.Play();
    }

}
