using UnityEngine;
using Photon.Pun;

public class ProjectileController : MonoBehaviourPunCallbacks
{
    public BulletData bulletData;

    public Vector3 currentDir;

    // The PhotonView ID of the player who fired the projectile
    private int ownerViewId;

    private void Start()
    {
        Destroy(gameObject, 3f);
        currentDir = Vector3.right;
    }

    private void Update()
    {
       // if (PlayerController.facingRight)
            transform.Translate(bulletData.speed * Time.deltaTime * Vector3.right);
        //else if (!PlayerController.facingRight)
        //    transform.Translate(bulletData.speed * Time.deltaTime * Vector3.left);
    }

    //public void SetBulletDirection(Vector3 dir)
    //{
    //    currentDir = dir;
    //}

    public void SetOwner(int viewId)
    {
        ownerViewId = viewId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent(out PlayerController player))
        //    PlayerController player = collision.GetComponent<PlayerController>();
        //if (player != null)
        {
            // Ensure that the player hit is not the owner of the projectile
            if (player.photonView.ViewID != ownerViewId)
            {
                player.TakeDamage(bulletData.damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
