using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    //move vars
    public float moveSpeed = 4;
    public float minMoveSpeed = 1f;
    public float maxMoveSpeed = 7;
    
    //stamina vars
    public float stamina = 100f;
    public float fatigue = 10f;
    public int adrenaline = 5;
    
    //jump vars
    public float gravity = -15f;
    public float jumpHeight = 1f;
    
    //crouch vars
    public float crouchHeight;
    public float minCrouchHeight = 1f;
    public float standHeight = 2f;

    [SerializeField] UIManager UIManager;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask obstacleMask;

    Vector3 velocity;
    bool isGrounded;
    public bool sprinting = false;

    private void Awake()
    {
        moveSpeed = 4;
        stamina = 100f;
        UIManager.UpdateMoveSpeedSlider();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController> ();
    }

    // Update is called once per frame
    void Update()
    {
        //===GROUNDED CHECKS===
        //check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //reset velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //===MOVEMENT===
        //movement WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //adjust move speed
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
        moveSpeed += Input.GetAxis("Mouse ScrollWheel");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);


        //===SPRINT===
        if (sprinting == true)
        {
            //deplete stamina
            DecreaseStamina();

            //restrict weapon ready


            //limit strafe speed
            //dont allow backwards movement
        }
        else if (stamina < 100f && sprinting == false)
        {
            //regen stamina
            StartCoroutine(IncreaseStamina(fatigue - adrenaline));
        }
        else
        {
            //unlock weapon ready bool
            

            //unlock strafe


            //unlock backwwards movement

        }

        if (stamina <= 1)
        {
            OutOfStamina();
        }
        else
        {
            maxMoveSpeed = 7;
        }


        //===JUMPING===
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            if (stamina < 1)
            {
                velocity.y = Mathf.Sqrt((jumpHeight / 2) * -2f * gravity);
            }
            else
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            stamina -= (fatigue * 2);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


        //===CROUCHING/PRONING===
        //crouch
        if (Input.GetKey(KeyCode.C))
        {
            Crouch(1);
        }
        else
        {
            //get component and resize to stand height
            controller.height = standHeight;
            //set max movement speed
            maxMoveSpeed = 7;
        }
        //adjust crouch height with scroll wheel
        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouchHeight = Mathf.Clamp(crouchHeight, minCrouchHeight, standHeight);
            crouchHeight += Input.GetAxis("Mouse ScrollWheel");
            Crouch(crouchHeight);
        }

        //prone
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //prone
        }
    }

    //DELETE may be unecessary
    public void changeMoveSpeed()
    {
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);

        moveSpeed += Input.GetAxis("Mouse ScrollWheel");
    }

    //CROUCH FUNCTION
    public void Crouch(float crouchHeight)
    {
        //get component and resize to crouch height
        controller.height = crouchHeight;
        //set max movement speed
        maxMoveSpeed = 4;
    }
    
    //STAMINA REGEN AND FATIGUE EFFECTS
    public void DecreaseStamina()
    {
        stamina = Mathf.Clamp(stamina, 0, 100);
        stamina -= Time.deltaTime * (fatigue * (moveSpeed/2));
        Debug.Log(stamina);
    }

    public IEnumerator IncreaseStamina(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        stamina = Mathf.Clamp(stamina, 0, 100);
        stamina += Time.deltaTime * adrenaline;
        Debug.Log(stamina);
    }

    public void OutOfStamina()
    {
        maxMoveSpeed = 4;
        if(moveSpeed > maxMoveSpeed)
        {
            moveSpeed = 4;
        }
    }

    
}
