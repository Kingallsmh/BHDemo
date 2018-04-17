using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public bool isHit = false;
    float hitTime = 1.5f;
    public int HPMax, HPCurrent;
    EntityInterpret interpret;
    public SpriteRenderer sr;

    private void Start()
    {
        interpret = GetComponent<EntityInterpret>();
    }

    public void InitStats()
    {
        HPCurrent = HPMax;
    }

    /// <summary>
    /// Take damage by a flat amount
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        if (isHit)
        {
            return;
        }
        HPCurrent -= dmg;
        if(HPCurrent <= 0)
        {
            HPCurrent = 0;
        }
        else
        {
            StartCoroutine(interpret.Hit());
            StartCoroutine(WaitForHit());
        }
    }

    /// <summary>
    /// Restore health to 100%
    /// </summary>
    public void HealHP()
    {
        HPCurrent = HPMax;
    }

    /// <summary>
    /// Restore health by given amount
    /// </summary>
    /// <param name="amount"></param>
    public void HealHP(int amount)
    {
        HPCurrent += amount;
        if(HPCurrent > HPMax)
        {
            HPCurrent = HPMax;
        }
    }

    public IEnumerator WaitForHit()
    {
        isHit = true;
        float currentTime = 0;
        float alpha = 1;
        while(currentTime <= hitTime)
        {
            sr.color = new Color(1, 1, 1, alpha);
            if(alpha > 0.5f)
            {
                alpha = 0.5f;
            }
            else
            {
                alpha = 1;
            }
            currentTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        sr.color = new Color(1, 1, 1, 1);
        isHit = false;
    }
}
