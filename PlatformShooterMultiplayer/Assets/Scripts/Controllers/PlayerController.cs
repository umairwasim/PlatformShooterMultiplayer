using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    public static bool facingRight;

    [Header("Health")]
    public GameObject healthCanvas;
    public Image healthBarImage;
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Shoot")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    private int currentHealth;
    private bool isGrounded;

    private const string GROUND = "Ground";
    private const string FIRE = "Fire1";
    private const string HORIZONTAL = "Horizontal";
    private const string PREFABS = "Prefabs";
    private const string BULLET = "SimpleBullet";

    private Transform spriteTransform;
    private Rigidbody2D rb;
    private PhotonView pv;
    private PlayerManager playerManager;
    //private Vector3 direction;

    private void Awake()
    {
        spriteTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        //Prevent applying Physics to other client
        if (!pv.IsMine)
        {
           // Destroy(rb);
            //Destroy(healthCanvas);
        }

        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        Move();
        Jump();
        Shoot();
        OutOfMapDeath();
    }

    #region Move/Jump/Shoot
    private void Move()
    {
        float horizontalInput = Input.GetAxis(HORIZONTAL);

        //Left
        if (horizontalInput > 0 && facingRight)
            FlipDirection();

        //Right
        else if (horizontalInput < 0 && !facingRight)
            FlipDirection();

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        //if (spriteTransform.localScale.x < 1)
        //{
        //spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
        //}
        //facingRight = false;
        //SetDir(Vector3.left);
        //if (spriteTransform.localScale.x > 1)
        //{
        //spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
        //}
        //facingRight = true;
        //SetDir(Vector3.right);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        if (Input.GetButtonDown(FIRE))
        {
            // Instantiate and synchronize projectiles
            GameObject projectile = PhotonNetwork.Instantiate(Path.Combine(PREFABS, BULLET),
                firePoint.position, firePoint.rotation);

            if (projectile.TryGetComponent(out ProjectileController projectileController))
                projectileController.SetOwner(pv.ViewID);

            // ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            //rb.velocity = transform.right * projectileController.bulletData.speed;

            //if (Input.GetAxisRaw(HORIZONTAL) > 0)
            //    rb.velocity = transform.right * projectileController.bulletData.speed;
            //else if (Input.GetAxisRaw(HORIZONTAL) < 0)
            //    rb.velocity = -transform.right * projectileController.bulletData.speed;

            // Set owner of the projectile
            //projectileController.SetBulletDirection(direction);
            //}
        }
    }

    //Call Die if player falls out of map and 
    private void OutOfMapDeath()
    {
        if (transform.position.y < -10f)
            Die();
    }

    private void FlipDirection()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    #endregion


    #region Take Damage

    //Send a RPC call of Take Damage
    public void TakeDamage(int damage)
    {
        pv.RPC(nameof(RPC_TakeDamage), pv.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage, PhotonMessageInfo info)
    {
        currentHealth -= damage;

        Debug.Log("Current Health: " + currentHealth + " ViewID: " + photonView.ViewID);

        healthBarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }

    #endregion

    private void Die()
    {
        playerManager.Die();
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

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}