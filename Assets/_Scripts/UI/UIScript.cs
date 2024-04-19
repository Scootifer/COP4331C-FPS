using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{
    public Image healthBar;
    public float currentHP;
    public TMP_Text ammoText;
    public float totalAmmo;
    public TMP_Text magazine;
    public float magAmmo;
    public TMP_Text grenadeText;
    public float currentGrenade;
    public GameObject reloadText;
    public GameObject Controls;
    public TMP_Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        reloadText.SetActive(false);
        
    }
    public void Reload()
    {
        //activate "Reload" for 2 seconds
        reloadText.SetActive(true);
        StartCoroutine(reloadWait());
    }
    public IEnumerator reloadWait()
    {
        yield return new WaitForSeconds(2);
        reloadText.SetActive(false);
    }

    public void TakeDamage(float currentHP)
    {
        healthBar.fillAmount = currentHP / 100f;
    }
    public void Grenade(float currentGrenade)
    {
        grenadeText.SetText(currentGrenade.ToString());
    }
    public void Ammo(float totalAmmo, string magAmmo)
    {
        magazine.SetText(magAmmo);
        ammoText.SetText(totalAmmo.ToString());
    }
    public void SetScoreText(int score, int curBases, int maxBases)
    {
        ScoreText.SetText($"Destroy the enemy bases: {curBases}/{maxBases}\nScore: {score}");
    }
    public void ToggleControls()
    {
        if (Controls.activeSelf)
            Controls.SetActive(false);
        else if (!Controls.activeSelf)
            Controls.SetActive(true);
    }

}
