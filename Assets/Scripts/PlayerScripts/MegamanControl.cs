using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanControl : PhysicsObject {

    public float maxSpeed = 7;
    public float speed = 7;
    public float jumpTakeOffSpeed = 10;
    public float dodgeSpeed = 2;
    public bool isDodging = false;
    public float totalDodgeTime = 2;
    float dodgeTimer = 0;
    public GameObject attackUtil;
    bool jumpAgain = true;

    public BaseController pc;
    private CharacterStats stats;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        SetFacing(1);
    }

    private void Start()
    {
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
        }
        else
        {
            if (isDodging)
                move.x = Dodge(totalDodgeTime);
        }

        if (pc.GetButton(2) && grounded && !isBusy)
        {
            isDodging = true;
            isBusy = true;
            animator.SetBool("Dash", true);
        }

        if (pc.GetButton(0) && grounded && jumpAgain)
        {
            velocity.y = jumpTakeOffSpeed;
            jumpAgain = false;
            StartCoroutine(WatchForJumpRelease());
        }
        else if (!pc.GetButton(0))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
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
                    if (grounded)
                    {
                        Attack();
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator InterpretInput2()
    {
        while (pc)
        {
            if (!GameManagerScript.Instance.PauseActions)
            {
                if (!isBusy || isBusy)
                {
                    pc.GetInput();
                    if (pc.GetButton(1) && !isBusy) //Attack button
                    {
                        Attack();
                    }
                    else if (pc.GetButton(2))
                    {
                        StartCoroutine(Special(1));
                    }
                }
                else
                {
                    pc.ResetInput();
                }
            }

            yield return null;
        }
    }

    public IEnumerator Hit()
    {
        isBusy = true;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
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
    }
}
