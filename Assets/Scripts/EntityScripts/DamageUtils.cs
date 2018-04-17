using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUtils : MonoBehaviour {

    public GameObject parentObject;
    public int damage;
    public bool hitTarget = false;
    public bool hitTerrain = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Character" && collision.gameObject != parentObject)
        {
            if (collision.gameObject.GetComponent<CharacterStats>())
            {
                collision.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);
                hitTarget = true;
            }
            else
            {
                Debug.LogError("Character tagged object does not have a stats script attached!");
            }
        }
        if(collision.gameObject.tag == "Terrain")
        {
            hitTerrain = true;
            if (collision.gameObject.GetComponent<CharacterStats>())
            {
                collision.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);
                hitTarget = true;
            }
        }
    }

    public void ResetHits()
    {
        hitTarget = false;
        hitTerrain = false;
    }
}
