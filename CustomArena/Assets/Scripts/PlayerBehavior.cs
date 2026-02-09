using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBehavior : MonoBehaviour 
{
    public GameBehavior GameManager;

    public float MoveSpeed; // Movement Speed
    public float RotateSpeed = 75f; // Rotation Speed
    private float _vInput; // Vertical Input

    private float _hInput; // Horizontal Input
    private Rigidbody _rb; // Rigidbody Component
    
    public float JumpVelocity = 5f; // Jump Velocity
    private bool _isJumping; // Jumping Action

    public float DistanceToGround = 0.1f; // Distance to Ground
    public LayerMask GroundLayer; // Ground Layer
    private CapsuleCollider _col; // Capsule Collider

    public GameObject Bullet; // Bullet Prefab
    public float BulletSpeed = 100f; // Bullet Speed
     
    private bool _isShooting; // Shooting Action

    private GameBehavior _gameManager; // Reference to Game Manager


    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Get Rigidbody Component

        _col = GetComponent<CapsuleCollider>(); // Get Capsule Collider Component
    
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>(); // Get Game Manager Component
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            _gameManager.HP -= 1;
        }
    }

    void Update()
    {
        MoveSpeed = GameManager._playerSpeed; // Update Move Speed from Game Manager

        _vInput = Input.GetAxis("Vertical") * MoveSpeed; // Vertical Move Speed
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed; // Horizontal Rotate Speed
        /*
        this.transform.Translate(Vector3.forward * _vInput * 
        Time.deltaTime);
        this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);
        */

        _isJumping |= Input.GetKeyDown(KeyCode.J); // Jump Action

        _isShooting |=  Input.GetKeyDown(KeyCode.Space); // Shooting Action

    }


    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation *
            Time.fixedDeltaTime); // Rotation

        _rb.MovePosition(this.transform.position +
            this.transform.forward * _vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot); // Movement

        if(IsGrounded() && _isJumping)
        {
            _rb.AddForce(Vector3.up * JumpVelocity,
            ForceMode.Impulse); // Jump Force
        }
        _isJumping = false; // Reset Jump Action

        if(_isJumping)
        {
        _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse); // Jump Force
        }
        _isJumping = false; // Reset Jump Action

        if (_isShooting)
        {
            Vector3 spawnPos = transform.position + 
                                   transform.forward * 1f; // Bullet Spawn Position
            GameObject newBullet = Instantiate(Bullet, spawnPos, 
                                       this.transform.rotation); // Create Bullet
            Rigidbody bulletRB = 
                newBullet.GetComponent<Rigidbody>(); // Get Bullet Rigidbody
            bulletRB.linearVelocity = this.transform.forward * 
                                          BulletSpeed; // Set Bullet Velocity
        }
        _isShooting = false; // Reset Shooting Action

    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x,
            _col.bounds.min.y, _col.bounds.center.z); // Bottom of Capsule
                
        bool grounded = Physics.CheckCapsule(_col.bounds.center,
            capsuleBottom, DistanceToGround, GroundLayer,
                QueryTriggerInteraction.Ignore); // Check if Grounded
                
        return grounded;
    }

} 