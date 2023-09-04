using UnityEngine;
using Photon.Pun;

public class ProjectileController : MonoBehaviourPunCallbacks
{
    public BulletData bulletData;

    // The PhotonView ID of the player who fired the projectile
    private int ownerViewId;
    private bool isEntered = false;
    private void Start()
    {
        Destroy(gameObject, 3f);
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
        if (collision.TryGetComponent(out IDamageable damageable) && !isEntered)
        {
            Debug.Log("Player " + " damage dealt " + bulletData.damage);
            isEntered = true;
            damageable.TakeDamage(bulletData.damage);
            Destroy(gameObject);
        }

        //if (collision.TryGetComponent(out PlayerController playerController))
        ////    PlayerController player = collision.GetComponent<PlayerController>();
        ////if (player != null)
        //{
        //    // Ensure that the player hit is not the owner of the projectile
        //    if (playerController.photonView.ViewID != ownerViewId)
        //    {
        //        playerController.TakeDamage(bulletData.damage);
        //        Debug.Log("Player " + playerController.photonView.ViewID + " damage dealt " + bulletData.damage);
        //        Destroy(gameObject);
        //    }
        //}
        else
        {
            Destroy(gameObject);
        }
    }
}
