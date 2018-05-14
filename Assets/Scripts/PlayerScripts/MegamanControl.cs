using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanControl : PhysicsObject, Damagable {

    public float maxSpeed = 7;
    public float speed = 7;
    public float jumpTakeOffSpeed = 10;
    public float dodgeSpeed = 2;
    public bool isDodging = false;
    public float totalDodgeTime = 2;
    float dodgeTimer = 0;
    public GameObject attackUtil;
    bool jumpAgain = true;

    Vector2 pushVelocity = Vector2.zero;

    public BaseController pc;
    private CharacterStats stats;
    private Animator animator;
    public HitboxControl hitbox;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        SetFacing(1);
    }

    private void Start()
    {
        hitbox.damageItem = this;
        StartCoroutine(InterpretInput());
    }

    //For controlling movement of the entity
    Vector2 previousMove;
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        if (!isBusy)
        {
            move.x = pc.DirectionInput.x;
            if (pc.GetButton(0) && grounded && jumpAgain)
            {
                velocity.y = jumpTakeOffSpeed;
                jumpAgain = false;
                grounded = false;
                StartCoroutine(WatchForJumpRelease());
            }
            else if (!pc.GetButton(0))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            if (pc.GetButton(2) && grounded && !isBusy)
            {
                isDodging = true;
                isBusy = true;
                animator.SetBool("Dash", true);
            }
        }
        else
        {
            if (isDodging)
                move.x = Dodge(totalDodgeTime);
            if (!grounded)
            {
                move = previousMove;
            }
        }



        if (move.x > 0)
        {
            transform.localScale = new Vector3(-1, 1);
        }
        else if (move.x < 0)
        {
            transform.localScale = new Vector3(1, 1);
        }
        animator.SetFloat("Movement_Y", velocity.y);
        animator.SetBool("InAir", !this.grounded);
        animator.SetBool("Moving", Mathf.Abs(velocity.x) > 0);
        previousMove = move;
        move += pushVelocity;
        PushBack();
        targetVelocity = move * maxSpeed;
    }

    public IEnumerator WatchForJumpRelease()
    {
        while (pc.GetButton(0) || !grounded)
        {
            yield return null;
        }
        jumpAgain = true;
    }

    public IEnumerator InterpretInput()
    {
        while (pc)
        {
            if (!GameManagerScript.Instance.PauseActions)
            {
                pc.GetInput();
                if (pc.GetButton(1) && !isBusy) //Attack button
                {
                    Attack();
                }
            }
            yield return null;
        }
    }



    public IEnumerator Hit()
    {
        isBusy = true;
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Hit", false);
        isBusy = false;
    }

    public void Attack()
    {
        isBusy = true;
        animator.SetTrigger("Attack");
    }

    bool inAttackAnim = false;
    public void AttackEventStart(int direction)
    {
        inAttackAnim = true;
        //StartCoroutine(AttackEventExecution(direction));
    }

    public void AttackEventEnd()
    {
        inAttackAnim = false;
    }

    //IEnumerator AttackEventExecution(int direction)
    //{
    //    //unphysics = true;
    //    int currentSubtract = 0;
    //    while (inAttackAnim)
    //    {
    //        Movement(new Vector2(tackleSpeed - currentSubtract, 0) * Time.deltaTime * direction, false);
    //        currentSubtract++;
    //        yield return null;
    //    }
    //    //unphysics = false;
    //}

    public IEnumerator Special(float cooldown)
    {
        isBusy = true;
        animator.SetTrigger("Special");
        yield return new WaitForSeconds(cooldown);
        isBusy = false;
    }

    //public void LaunchProjectile()
    //{
    //    if(animator.GetFloat("Input_X") > 0)
    //    {
    //        attackUtil.FireAmmo(1, 0);
    //    }
    //    else
    //    {
    //        attackUtil.FireAmmo(-1, 0);
    //    }
    //}

    float Dodge(float time)
    {
        if (dodgeTimer > time)
        {
            isDodging = false;
            isBusy = false;
            dodgeTimer = 0;
            animator.SetBool("Dash", false);
            StartCoroutine(AllActionCooldown(0.1f));
            return 0;
        }
        else
        {
            dodgeTimer += Time.deltaTime;
            return dodgeSpeed * -transform.localScale.x;
        }
    }

    public void SetToNotBusy(float delay)
    {
        StartCoroutine(AllActionCooldown(delay));
    }

    public IEnumerator AllActionCooldown(float secs)
    {
        isBusy = true;
        yield return new WaitForSeconds(secs);
        isBusy = false;
    }

    public void SetFacing(float x)
    {
        animator.SetFloat("Input_X", x);
    }

    public void TakeDamage(int dmg)
    {
    }

    public void TakeDamage(int dmg, Vector2 location)
    {
        if (!isDodging)
        {
            StartCoroutine(Hit());
        }
    }

    public void PushBack()
    {
        if (pushVelocity.x > 0.1f || pushVelocity.x < -0.1f)
        {
            pushVelocity.x -= Mathf.Sign(pushVelocity.x) * 0.1f;
        }
        else
        {
            pushVelocity.x = 0;
        }
        if (pushVelocity.y > 0.1f || pushVelocity.y < -0.1f)
        {
            pushVelocity.x -= Mathf.Sign(pushVelocity.y) * 0.1f;
        }
        else
        {
            pushVelocity.y = 0;
        }
    }

    public void PushBack(Vector2 push)
    {
        pushVelocity = push;
    }
}
