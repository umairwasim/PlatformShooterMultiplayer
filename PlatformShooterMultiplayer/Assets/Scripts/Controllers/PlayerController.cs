using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    public static bool facingRight;

    public int maxHealth = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform firePoint;
    public GameObject projectilePrefab;

    private int currentHealth;
    private bool isGrounded;

    private const string GROUND = "Ground";
    private const string FIRE = "Fire1";
    private const string HORIZONTAL = "Horizontal";

    private Transform spriteTransform;
    private Rigidbody2D rb;
    private PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        spriteTransform = GetComponent<Transform>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        //Prevent applying Physics to other client
        if (!pv.IsMine)
            Destroy(rb);
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        Move();
        Jump();
        Shoot();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis(HORIZONTAL);

        //Left
        if (Input.GetAxisRaw(HORIZONTAL) > 0)
        {
            if (spriteTransform.localScale.x < 1)
            {
                spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
            }
            facingRight = false;
        }
        //Right
        else if (Input.GetAxisRaw(HORIZONTAL) < 0)
        {
            if (spriteTransform.localScale.x > 1)
            {
                spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
            }
            facingRight = true;
        }
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void Shoot()
    {
        if (Input.GetButtonDown(FIRE))
        {
            // Instantiate and synchronize projectiles
            GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, firePoint.position, firePoint.rotation);

            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            Rigidbody2D rb = projectile?.GetComponent<Rigidbody2D>();

            if (projectileController)
            {
                rb.velocity = transform.right * projectileController.bulletData.speed;

                // Set owner of the projectile
                projectileController.SetOwner(pv.ViewID);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!pv.IsMine)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();

    }

    private void Die()
    {
        // death logic
    }

    #region Grounded Check

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND))
        {
            isGrounded = false;
        }
    }
    #endregion
}