using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private AudioClip hitSound;
    private float direction;
    private bool hit;
    private float movementSpeed;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        damage = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        movementSpeed = speed * direction * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if(lifetime > 5)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        animator.SetTrigger("hit");
        SoundManager.Instance.PlaySound(hitSound);

        // Handle hitting player
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage, direction);
        }
    }

    public void SetDirection(float _direction)
    {
        hit = false;
        direction = _direction;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
        lifetime = 0f;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
