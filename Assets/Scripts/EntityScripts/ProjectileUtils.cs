using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUtils : MonoBehaviour {

    public ProjectileType type = ProjectileType.DirectX;
    public Vector2 spawnAdjust;
    public float speed;
    public bool destructableOnTerrain = true;
    public bool destructableOnCharacter = true;
    public float maxDistance = 1000;
    public Transform spawnPoint;
    DamageUtils dmgUtil;
    AttackUtils attackUtil;

    Rigidbody2D rb;
    Animator anim;

    public enum ProjectileType
    {
        DirectX, DirectY
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<DamageUtils>())
        {
            dmgUtil = GetComponent<DamageUtils>();
        }
    }

    public void AddReferenceToAttackUtil(AttackUtils util)
    {
        attackUtil = util;
    }

    public void FireProjectile(int x, int y)
    {
        switch (type)
        {
            case ProjectileType.DirectX: StartCoroutine(DirectPath(x, y));
                break;
        }
    }

    public void SetupAmmo()
    {
        rb.velocity = Vector3.zero;
        transform.SetParent(spawnPoint);
        transform.localPosition = spawnAdjust;
    }

    public IEnumerator DirectPath(int xDirection, int yDirection)
    {
        SetupAmmo();
        SetFacing(xDirection, yDirection);
        transform.parent = null;
        float traveledDistance = 0;
        Vector2 velocity = new Vector2(xDirection, yDirection) * Time.deltaTime * speed * 10;
        while (traveledDistance < maxDistance)
        {
            traveledDistance += velocity.magnitude;
            rb.velocity = velocity;
            if (CheckIfHitTarget(destructableOnCharacter) || CheckIfHitTerrain(destructableOnTerrain))
            {
                break;
            }
            yield return null;
        }
        EndProjectile();
    }

    public void SetFacing(int xDirection, int yDirection)
    {
        if(xDirection > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public bool CheckIfHitTarget(bool doCheck)
    {
        if (doCheck)
        {
            if (dmgUtil)
            {
                if(dmgUtil.hitTarget)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckIfHitTerrain(bool doCheck)
    {
        if (doCheck)
        {
            if (dmgUtil)
            {
                if (dmgUtil.hitTerrain && destructableOnTerrain)
                {
                    EndProjectile();
                    return true;
                }
            }
        }
        return false;
    }

    public void EndProjectile()
    {
        SetupAmmo();
        dmgUtil.ResetHits();
        attackUtil.AddBackToBag(this.gameObject);
        gameObject.SetActive(false);        
    }
}
