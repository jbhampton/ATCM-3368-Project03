using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rbPlayerMovement : MonoBehaviour
{
    

    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private CapsuleCollider PlayerCollider;
    [Space]

    #region MOVE VARS
    //move vars
    private Vector3 PlayerMovementInput;
    [SerializeField] public float moveSpeed;
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;
    public bool isMoving;
    public bool isSprinting;
    #endregion

    #region JUMP VARS
    //jump vars
    [SerializeField] private float jumpForce;
    private Vector3 jumpDirection;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask obstacleMask;
    #endregion

    #region CROUCH VARS
    //crouch vars
    private bool Crouching = false;
    private float crouchHeight;
    private float standHeight = 2f;
    public float currentHeight;
    private float minStandHeight = 1f;
    private float maxStandHeight = 2f;
    #endregion

    #region STAMINA VARS
    //stamina vars
    public float stamina = 100f;
    public float fatigue = 10f;
    public float adrenaline = 5f;
    #endregion

    //UI vars
    [SerializeField] rbUIManager rbUIManager;
    [SerializeField] Camera playerCamera;
    public CameraShake cameraShake;
    

    private void Awake()
    {
        Crouching = false;
        standHeight = 2f;
        currentHeight = standHeight;
    }

    void Update()
    {
        //==PLAYER MOVEMENT==
        #region PLAYER MOVEMENT
        //adjust move speed
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
        if (Input.GetKey(KeyCode.LeftControl) == false)
        {
            moveSpeed += Input.GetAxis("Mouse ScrollWheel");
        }
        //move player
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
        if(PlayerBody.velocity.magnitude > 0)
        {
            isMoving = true;
        }
        #endregion

        //==PLAYER JUMPING==
        #region PLAYER JUMPING
        //check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            Jump();
            Stand();
        }
        #endregion

        //==CROUCHING==
        #region CROUCHING
        //crouch
        if (Input.GetKeyDown(KeyCode.C) && Crouching == false)
        {
            Crouch();
        } else if (Input.GetKeyDown(KeyCode.C) && Crouching == true)
        {
            Stand();
        }
        //adjust crouchHeight
        standHeight = Mathf.Clamp(standHeight, minStandHeight, maxStandHeight);
        if (Input.GetKey(KeyCode.LeftControl)) {
            standHeight += Input.GetAxis("Mouse ScrollWheel");
            PlayerCollider.height = standHeight;
            currentHeight = standHeight;
        }
        if (PlayerCollider.height < maxStandHeight)
        {
            Crouching = true;
        } else
        {
            Crouching = false;
        }
        if (Crouching == true)
        {
            maxMoveSpeed = 2;
        } else
        {
            maxMoveSpeed = 4;
        }
        #endregion

        //==SPRINT==
        #region SPRINT
        if (Input.GetKey(KeyCode.LeftShift) && isMoving == true)
        {
            //sprinting = true
            isSprinting = true;
            //increase movespeed
            maxMoveSpeed = 7;
            moveSpeed = 7;
            //drain stamina
            DecreaseStamina();
            Stand();

        } else
        {
            maxMoveSpeed = 4;
            isSprinting = false;
            isMoving = false;
        }
        if (stamina < 100f && isSprinting == false)
        {
            //regen stamina
            StartCoroutine(IncreaseStamina(2));
        }
        if (stamina <= 1)
        {
            OutOfStamina();
        }
        #endregion
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * moveSpeed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
        //StartCoroutine(cameraShake.Shake(5f, 0.4f));
        //limit strafe if sprinting
        if(isSprinting == true)
        {
            PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z/2);
        }
    }

    private void Jump()
    {
        jumpDirection = new Vector3(PlayerMovementInput.x, 1f, PlayerMovementInput.z);
        PlayerBody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
    }

    private void Crouch()
    {
        PlayerCollider.height = crouchHeight;
        Crouching = true;
        currentHeight = 1;
    }

    private void Stand()
    {
        PlayerCollider.height = maxStandHeight;
        standHeight = maxStandHeight;
        currentHeight = standHeight;
        Crouching = false;
    }

    //STAMINA REGEN AND FATIGUE EFFECTS
    public void DecreaseStamina()
    {
        stamina = Mathf.Clamp(stamina, 0, 100);
        stamina -= Time.deltaTime * (fatigue);
        //Debug.Log(stamina);
    }

    public IEnumerator IncreaseStamina(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        stamina = Mathf.Clamp(stamina, 0, 100);
        stamina += Time.deltaTime * adrenaline;
        //Debug.Log(stamina);
    }

    public void OutOfStamina()
    {
        isSprinting = false;
        maxMoveSpeed = 4;
        
    }
}
