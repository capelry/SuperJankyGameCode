using UnityEngine;

public class StationaryTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage, Mathf.Sign(collision.GetComponent<Rigidbody2D>().velocity.x));
        }
    }
}
