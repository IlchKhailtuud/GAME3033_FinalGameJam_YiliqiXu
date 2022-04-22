using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovementComponent : MonoBehaviour
{
    //movement variables
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float runSpeed = 20;
    [SerializeField] private float jumpForce = 5;

    //components
    private PlayerController playerController;
    private Rigidbody rigidbody;
    private Animator playeranimator;
    public GameObject followTransform;

    //movement references
    private Vector2 inputVector = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 lookInput = Vector2.zero;
    private float moveSpeedPara = 1.0f;

    public float MoveSpeedPara
    {
        get => moveSpeedPara;
        set => moveSpeedPara = value;
    }

    public float aimSensitivity = 1.0f;
    
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");

    public void Awake()
    {
        playeranimator = GetComponent<Animator>();
        //playerController = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //x-axis ratation
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);
        //
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        if (angle>180 && angle<300)
        {
            angles.x = 300;
        }
        else if (angle<180 && angle>70)
        {
            angles.x = 70;
        }

        followTransform.transform.localEulerAngles = angles;
        
        //rotate the play along with aim
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        if (playerController.isJumping) return;
        if (!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;

        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime * moveSpeedPara);

        //rigidbody.velocity = 
        transform.position += movementDirection;
        //Debug.Log($"{movementDirection.magnitude}");
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        playeranimator.SetFloat(movementXHash, inputVector.x);
        playeranimator.SetFloat(movementYHash, inputVector.y);
    }
    
    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        playeranimator.SetBool(isRunningHash, playerController.isRunning);
    }
    
    public void OnJump(InputValue value)
    {
        if (playerController.isJumping)
        {
            return;
        }
        playerController.isJumping = true;
        rigidbody.AddForce((transform.up + moveDirection)*jumpForce,ForceMode.Impulse);
        
        playeranimator.SetBool(isJumpingHash, playerController.isJumping);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground") && !playerController.isJumping || rigidbody.velocity.y > 0) return;

        if (IsGroundCollision(other.contacts))
        {
            playerController.isJumping = false;
            playeranimator.SetBool(isJumpingHash,false);
        }
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground") && !playerController.isJumping || rigidbody.velocity.y > 0) return;

        if (IsGroundCollision(other.contacts))
        {
            playerController.isJumping = false;
            playeranimator.SetBool(isJumpingHash,false);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground"))
            return;
        
        if (!IsGroundCollision(other.contacts))
        {
            playerController.isJumping = true;
            playeranimator.SetBool(isJumpingHash,true);
        }
    }

    bool IsGroundCollision(ContactPoint[] contacts)
    {
        for (int i = 0; i < contacts.Length; i++)
        {
            if (1-contacts[i].normal.y<0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Deathplane")
        {
            FloorTileManager.Instance.EndGame(false);
            Destroy(gameObject);
        }
    }
}
