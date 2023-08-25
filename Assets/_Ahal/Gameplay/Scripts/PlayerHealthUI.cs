using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject[] healthUIParts;

    public void SetHealth(int healthAmount)
    {
        for (var i = 0; i < healthUIParts.Length; i++)
        {
            healthUIParts[i].SetActive(i < healthAmount);
        }
    }
}