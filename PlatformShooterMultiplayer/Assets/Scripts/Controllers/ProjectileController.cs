using UnityEngine;
using Photon.Pun;

public class ProjectileController : MonoBehaviourPunCallbacks
{
    public float speed = 10f;
    public int damage = 10;

    private int ownerViewId; // The PhotonView ID of the player who fired the projectile

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    public void SetOwner(int viewId)
    {
        ownerViewId = viewId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            // Ensure that the player hit is not the owner of the projectile
            if (player.photonView.ViewID != ownerViewId)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
