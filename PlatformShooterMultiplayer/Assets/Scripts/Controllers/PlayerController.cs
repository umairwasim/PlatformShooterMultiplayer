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

    private Rigidbody2D rb;
    private Transform spriteTransform;
    private PhotonView photonView;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        spriteTransform = GetComponent<Transform>();

        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
            Jump();
            Shoot();
        }
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
            if (spriteTransform.localScale.x > 1)
            {
                spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
            }
            facingRight = false;
            //rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        }
        //Right
        else if (Input.GetAxisRaw(HORIZONTAL) < 0)
        {
            if (spriteTransform.localScale.x < 1)
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
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            rb.velocity = transform.right * projectileController.bulletData.speed;

            // Set owner of the projectile
            projectileController.SetOwner(photonView.ViewID);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
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