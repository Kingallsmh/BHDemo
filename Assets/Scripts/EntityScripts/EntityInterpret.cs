﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInterpret : PhysicsObject {

	public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 10;
    public float tackleSpeed = 10;
    public AttackUtils attackUtil;
    bool jumpAgain = true;
    bool isBusy = false;

    public BaseController pc;
    private CharacterStats stats;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        stats.InitStats();
        //To have it facing right * * *
        SetFacing(1);
    }

    private void Start()
    {
        StartCoroutine(InterpretInput());
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = pc.DirectionInput.x;

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

        //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        //if (flipSprite)
        //{
        //    spriteRenderer.flipX = !spriteRenderer.flipX;
        //}
        
        animator.SetBool("InAir", !this.grounded);
        animator.SetBool("Moving", Mathf.Abs(velocity.x) > 0);
        if(pc.DirectionInput.x != 0)
        {
            animator.SetFloat("Input_X", pc.DirectionInput.x);
        }

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
                if (!isBusy)
                {
                    pc.GetInput();
                    if (pc.GetButton(1)) //Attack button
                    {
                        Attack();
                    }
                    else if(pc.GetButton(2)){ //Special button
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
        StartCoroutine(AttackEventExecution(direction));
    }

    public void AttackEventEnd()
    {
        inAttackAnim = false;
    }

    IEnumerator AttackEventExecution(int direction)
    {
        unphysics = true;
        int currentSubtract = 0;
        while (inAttackAnim)
        {
            Movement(new Vector2(tackleSpeed - currentSubtract, 0) * Time.deltaTime * direction, false);
            currentSubtract++;
            yield return null;
        }
        unphysics = false;
    }

    public IEnumerator Special(float cooldown)
    {
        isBusy = true;
        animator.SetTrigger("Special");
        yield return new WaitForSeconds(cooldown);
        isBusy = false;
    }

    public void LaunchProjectile()
    {
        if(animator.GetFloat("Input_X") > 0)
        {
            attackUtil.FireAmmo(1, 0);
        }
        else
        {
            attackUtil.FireAmmo(-1, 0);
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
}
