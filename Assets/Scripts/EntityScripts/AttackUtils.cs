using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUtils : MonoBehaviour {

    public GameObject ammo;
    public int bagSize = 10;
    List<GameObject> ammoBag;
    
    private void Start()
    {
        ammoBag = new List<GameObject>();
        FillAmmoBag();
    }

    public void AddAmmo()
    {
        GameObject newAmmo = Instantiate(ammo);
        newAmmo.transform.parent = this.transform;
        newAmmo.transform.localPosition = ammo.GetComponent<ProjectileUtils>().spawnAdjust;
        newAmmo.GetComponent<ProjectileUtils>().AddReferenceToAttackUtil(this);        
        ammoBag.Add(newAmmo);
    }

    void FillAmmoBag()
    {
        for(int i = 0; i < bagSize; i++)
        {
            AddAmmo();
        }
    }

    public void FireAmmo(int x, int y)
    {
        if(ammoBag[0] != null)
        {
            ammoBag[0].gameObject.SetActive(true);
            ammoBag[0].GetComponent<ProjectileUtils>().FireProjectile(x, y);
            ammoBag.Remove(ammoBag[0]);
        }
        else
        {
            Debug.LogWarning("Not enough ammo in bag, created another");
            AddAmmo();
            ammoBag[0].gameObject.SetActive(true);
            ammoBag[0].GetComponent<ProjectileUtils>().FireProjectile(x, y);
            ammoBag.Remove(ammoBag[0]);
        }
    }

    public void AddBackToBag(GameObject ammo)
    {
        ammoBag.Add(ammo);
    }
}
