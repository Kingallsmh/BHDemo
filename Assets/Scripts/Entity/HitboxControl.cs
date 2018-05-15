using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitboxControl : MonoBehaviour
{
    public BoxType boxType = BoxType.Hitbox;
    public enum BoxType{
        Hitbox, Hurtbox, Guardbox
    }

    public Vector2 pushStrength = Vector2.zero;

    public Damagable damageItem;

    public List<Collider2D> ignoreList;

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //Debug.Log("Trigger: " + collision);
        if(ignoreList.Contains(collision.GetComponent<Collider2D>()) || !collision.GetComponent<HitboxControl>()){
            return;
        }
        HitboxControl colBox = collision.GetComponent<HitboxControl>();
        if(colBox != null){
            if(colBox.boxType == BoxType.Hurtbox){
                DealWithCollidedHurtBox(colBox);
            }
            else if(colBox.boxType == BoxType.Hitbox){
                DealWithCollidedHitBox(colBox);
            }
            else if(colBox.boxType == BoxType.Guardbox){
                DealWithCollidedGuardBox(colBox);
            }
        }
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        //Debug.Log("Collision: " + collision);
        HitboxControl colBox = collision.gameObject.GetComponent<HitboxControl>();
        if (colBox != null)
        {
            if (colBox.boxType == BoxType.Hurtbox)
            {
                DealWithCollidedHurtBox(colBox);
            }
            else if (colBox.boxType == BoxType.Hitbox)
            {
                DealWithCollidedHitBox(colBox);
            }
            else if (colBox.boxType == BoxType.Guardbox)
            {
                DealWithCollidedGuardBox(colBox);
            }
        }
	}

	void DealWithCollidedHurtBox(HitboxControl box){
        if(boxType == BoxType.Hurtbox){
            //Debug.Log("Weapon Clash!");
            //Possible clashing of weapons or some attack on attack action
        }
        else if(boxType == BoxType.Hitbox){
            //Debug.Log("Damage Recieved...");
            if(damageItem != null){
                damageItem.TakeDamage(1, box.GetComponent<Collider2D>().transform.position);
                float x = box.transform.position.x - transform.position.x;
                //Debug.Log(x);
                damageItem.PushBack(new Vector2(Mathf.Sign(x) * -box.pushStrength.x, box.pushStrength.y));
            }
            //Recieve damage
        }
        else if (boxType == BoxType.Guardbox)
        {
            //Debug.Log("Guarded Damage Recieved...");
            //Recieve damage
        }
    }

    void DealWithCollidedHitBox(HitboxControl box){
        if(boxType == BoxType.Hurtbox){
            //Debug.Log("Dealt damage!");

            //Deal damage to said box/owner
        }
    }

    void DealWithCollidedGuardBox(HitboxControl box){
        if(boxType == BoxType.Hurtbox){
            //Debug.Log("Dealt reduced damage!");
            //Deal less damage due to guarding or hard part
        }
    }

	private void OnDrawGizmos()
	{
		Collider2D col = GetComponent<Collider2D>();
		Vector3 boxSize = col.bounds.extents;
		switch(boxType){
			case BoxType.Hitbox:
				Gizmos.color = Color.green;
				break;
			case BoxType.Hurtbox:
				Gizmos.color = Color.red;                
                break;
			case BoxType.Guardbox:
				Gizmos.color = Color.blue;
                break;
		}
		Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(col.offset.x, col.offset.y, 0), transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxSize.x * 2, boxSize.y * 2, 1));
        
	}
}
