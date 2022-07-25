using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject roundOverScreen;

    public void RoundOver()
    {
        roundOverScreen.SetActive(true);
    }
}
