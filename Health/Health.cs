using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator animator;
    private bool dead;
    private Rigidbody2D rigidBody;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private float numFlashes;
    private SpriteRenderer spriteRend;
    private UIManager uiManager;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();

        uiManager = FindObjectOfType<UIManager>();
    }

    public void TakeDamage(float _damage, float direction)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // player takes damage
            animator.SetTrigger("hit");
            // iframes
            StartCoroutine(Flashing());

            // Knockback
            GetComponent<PlayerMovement>().TakeStun(0.2f, direction);
        }
        else
        {
            if (!dead)
            {
                
                // disable movement and attack
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                // player dies
                animator.SetTrigger("die");
                dead = true;
            }
            
        }
    }

    private IEnumerator Flashing()
    {
        for (int i = 0; i < numFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numFlashes*2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numFlashes*2));
        }
    }

    public void PlayerDead()
    {
        // return to menu but theres no menu so nothing becaues tired?
    }
}
