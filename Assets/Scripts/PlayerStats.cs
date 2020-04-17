using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    float level = 1;
    [SerializeField]
    float experience = 0;
    [Range(5, 5000)]
    public float expToLevel = 100;
    public float maxHealth = 100;
    [SerializeField]
    float curHp;
    public float maxStamina = 100;
    public float staminaRegenRate = 1f;
    [SerializeField]
    float curStam;
    [SerializeField]
    float atk = 5;
    [SerializeField]
    float def = 1;

    private void Awake()
    {
        SetStartingStats();
    }

    void SetStartingStats()
    {
        curHp = maxHealth;
        curStam = maxStamina;
    }

    void TakeDamage(float amt)
    {
        float dmg = amt - def;
        if (dmg > 0) curHp -= dmg;
        else curHp -= 1;
    }

    void GainExp(float amt)
    {
        experience += amt;
        if (experience > expToLevel)
        {
            level++;
            GetStat();
            experience -= expToLevel;
        }
    }

    void GetStat()
    {
        int num = Random.Range(0, 4);

        if (num == 0)
        {
            maxHealth += 5;
            Debug.Log("Increase max health");
        }
        else if (num == 1)
        {
            maxStamina += 5;
            Debug.Log("Increase max stamina");
        }
        else if (num == 2)
        {
            atk += 1;
            Debug.Log("Increase attack");
        }
        else if (num == 3)
        {
            def += 1;
            Debug.Log("Increase defense");
        }
    }

    private void Update()
    {
        if (curStam < maxStamina) curStam += Time.deltaTime * staminaRegenRate;

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.O))
            {
                curHp -= 1;
            }
            if (Input.GetKey(KeyCode.P))
            {
                curStam -= 5F * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.L))
            {
                GainExp(1);
            }
        }
    }
}
