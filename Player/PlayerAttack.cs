using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    [SerializeField] private AudioClip shootArrowSound;
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private string shoot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // initialize attacking instantly
        cooldownTimer = attackCooldown + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(shoot) && cooldownTimer > attackCooldown && playerMovement.CanAttack())
        {
            Attack();
        }
        // limit max value
        if (cooldownTimer < attackCooldown + 100f)
        {
            cooldownTimer += Time.deltaTime;
        }
        
    }

    private void Attack()
    {
        SoundManager.Instance.PlaySound(shootArrowSound);
        animator.SetTrigger("attack");
        cooldownTimer = 0;

        // pool object (used for lots of objects for better performance)
        arrows[FindArrow()].transform.position = attackOrigin.position;
        arrows[FindArrow()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
