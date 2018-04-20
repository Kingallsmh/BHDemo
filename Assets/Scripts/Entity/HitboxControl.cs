using UnityEngine;
using System.Collections;

public class HitboxControl : MonoBehaviour
{
    public BoxType boxType = BoxType.Hitbox;
    public enum BoxType{
        Hitbox, Hurtbox, Guardbox
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
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

    void DealWithCollidedHurtBox(HitboxControl box){
        if(boxType == BoxType.Hurtbox){
            Debug.Log("Weapon Clash!");
            //Possible clashing of weapons or some attack on attack action
        }
        else if(boxType == BoxType.Hitbox){
            Debug.Log("Damage Recieved...");
            //Recieve damage
        }
        else if (boxType == BoxType.Guardbox)
        {
            Debug.Log("Guarded Damage Recieved...");
            //Recieve damage
        }
    }

    void DealWithCollidedHitBox(HitboxControl box){
        if(boxType == BoxType.Hurtbox){
            Debug.Log("Dealt damage!");
            //Deal damage to said box/owner
        }
    }

    void DealWithCollidedGuardBox(HitboxControl box){
        if(boxType == BoxType.Hurtbox){
            Debug.Log("Dealt reduced damage!");
            //Deal less damage due to guarding or hard part
        }
    }
}
