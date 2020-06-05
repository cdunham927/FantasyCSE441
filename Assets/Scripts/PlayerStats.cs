using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float curStam;
    [SerializeField]
    float atk = 5;
    [SerializeField]
    float def = 1;
    float gold = 0;

    public float dmgDealt;

    //UI bars variables
    public GameObject healthCanvas;
    public Image healthImage;
    public Image staminaImage;
    public Image expImage;
    public Text statsText;
    public float lerpSpd = 5;

    //Death and respawning
    public bool alive = true;
    public string respawnSceneName;
    public GameObject deathUI;

    private void Awake()
    {
        if (healthCanvas != null) DontDestroyOnLoad(healthCanvas);
        SetStartingStats();
        deathUI = GameObject.FindGameObjectWithTag("DeathUI");
    }

    void SetStartingStats()
    {
        curHp = maxHealth;
        curStam = maxStamina;
        dmgDealt = atk;
    }

    public void TakeDamage(float amt)
    {
        if (alive)
        {
            float dmg = amt - def;
            if (dmg > 0) curHp -= dmg;
            else curHp -= 1;

            if (curHp <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        alive = false;
    }

    public void Respawn()
    {
        SetStartingStats();
        alive = true;
        SceneManager.LoadScene(respawnSceneName);
        //transform.position = GameObject.FindGameObjectWithTag("Spawnpoint").transform.position;
        transform.position = Vector3.zero;
    }

    public void GainExp(float amt)
    {
        experience += amt;
        if (experience >= expToLevel)
        {
            curHp = maxHealth;
            curStam = maxStamina;
            level++;
            GetStat();
            experience -= expToLevel;
        }
    }

    public void GainGold(float amt)
    {
        gold += amt;
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
            dmgDealt++;
            Debug.Log("Increase attack");
        }
        else if (num == 3)
        {
            def += 1;
            Debug.Log("Increase defense");
        }
    }

    public void ConsumeStamina(float amt)
    {
        curStam -= amt * Time.deltaTime;
    }

    private void Update()
    {
        if (deathUI != null) deathUI.SetActive(!alive);

        if (healthCanvas == null)
        {
            healthCanvas = GameObject.FindGameObjectWithTag("HealthUI");
            DontDestroyOnLoad(healthCanvas);
            healthImage = healthCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            staminaImage = healthCanvas.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
            expImage = healthCanvas.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();
        }

        if (healthCanvas != null)
        {
            //healthbar lerp fill
            healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, (curHp / maxHealth), Time.deltaTime * lerpSpd);
            //stamina lerp fill
            staminaImage.fillAmount = Mathf.Lerp(staminaImage.fillAmount, (curStam / maxStamina), Time.deltaTime * lerpSpd);
            //expbar lerp fill
            expImage.fillAmount = Mathf.Lerp(expImage.fillAmount, (experience / expToLevel), Time.deltaTime * lerpSpd);
        }

        if (statsText != null)
        {
            //stats text
            statsText.text = "Hp:" + curHp.ToString() + "/" + maxHealth.ToString() + "\nSp:" + Mathf.RoundToInt(curStam).ToString() + "/" + Mathf.RoundToInt(maxStamina).ToString() + "\nExp:" + experience.ToString() + "/" + expToLevel.ToString();
        }
        else statsText = GameObject.FindGameObjectWithTag("TextUI").GetComponent<Text>();

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
