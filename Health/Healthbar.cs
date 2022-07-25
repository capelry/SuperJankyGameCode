using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthbarOutline;
    [SerializeField] private Image healthBar;

    private void Start()
    {
        healthBar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = playerHealth.currentHealth / 100;
    }
}
