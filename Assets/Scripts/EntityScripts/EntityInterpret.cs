using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInterpret : PhysicsObject, Damagable {

	public float maxSpeed = 7;
    public float speed = 7;
    public float incrementMove = 1;
    public float decreaseMove = 1;
    public float jumpTakeOffSpeed = 10;
	public float lateJumpTime = 0.2f;
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
        //stats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hitbox.damageItem = this;
        StartCoroutine(InterpretInput());
    }

    //Variable used to record previous input and input velocity
    Vector2 previousMove;
	bool previousGroundedState;
    protected override void ComputeVelocity()
    {
        Vector2 move = previousMove;

        if(!isBusy){
            //Ramping speed or decline
            move.x += pc.DirectionInput.x * incrementMove;
            if((int)Mathf.Sign(move.x) != (int)Mathf.Sign(pc.DirectionInput.x) && (int)pc.DirectionInput.x != 0){
                move.x = 0;
            }
            else if(pc.DirectionInput.x <= 0.1f && pc.DirectionInput.x >= -0.1f){
                if(move.x >= 0.1f || move.x <= -0.1f){
                    move.x -= -transform.localScale.x * decreaseMove;
                }
                else{
                    move.x = 0;
                }
            }
        }
        else{
            move.x = 0;
            if(isDodging)
                move.x = Dodge(totalDodgeTime);
            if(!grounded){
                move = previousMove;
            }
        }

        //Handling Dodge (Zero's Dash)
        if(pc.GetButton(2) && grounded && !isBusy){
            isDodging = true;
            isBusy = true;
            animator.SetBool("Dash", true);
        }

        //Handling Jumping and air control
        if (pc.GetButton(0) && grounded && jumpAgain && !isBusy)
        {
			Jump();
        }
		else if (previousGroundedState == true && grounded == false)
        {
			StartCoroutine(LateJumpWatch(lateJumpTime));
        }
        else if (!pc.GetButton(0)) //Can add "&& !isBusy" to continue upwards motion or go change if receiving input or not
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }


        //Change the characters facing direction
        if(move.x > 0)
        {
            transform.localScale = new Vector3(-1, 1);
        }
        else if(move.x < 0)
        {
            transform.localScale = new Vector3(1, 1);
        }

        //Limits the move variable
        if(!isDodging){
            move.x = Mathf.Clamp(move.x, -maxSpeed, maxSpeed);
        }

        //Setting animations based on current variables
        animator.SetFloat("Movement_Y", velocity.y);
        animator.SetBool("InAir", !this.grounded);
        animator.SetBool("Moving", Mathf.Abs(velocity.x) > 0);
		//if(pc.DirectionInput.x != 0)
		//{
		//    animator.SetFloat("Input_X", pc.DirectionInput.x);
		//}

		previousGroundedState = grounded;
        previousMove = move;
        //If there is push back, add to move variable
        move += pushVelocity;
        PushBack();
        targetVelocity = move * speed;
    }

	public void Jump(){
		velocity.y = jumpTakeOffSpeed;
        jumpAgain = false;
        ResetGroundNormal();
        StartCoroutine(WatchForJumpRelease());
		grounded = false;
	}

	public IEnumerator LateJumpWatch(float time){
		float passedTime = 0;
		while(passedTime < time && !grounded){
			if(pc.GetButton(0) && !isBusy){
				Jump();
				break;
			}
			passedTime += Time.deltaTime;
			yield return null;
		}
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

    public IEnumerator InterpretInput2()
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

    float Dodge(float time){
        if(dodgeTimer > time){
            isDodging = false;
            isBusy = false;
            dodgeTimer = 0;
            animator.SetBool("Dash", false);
            StartCoroutine(AllActionCooldown(0.1f));
            return 0;
        }
        else{
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

    public void TakeDamage(int dmg)
    {
    }

    public void TakeDamage(int dmg, Vector2 location)
    {
        if(!isDodging){
            StartCoroutine(Hit());
        }
    }

    public void PushBack(){
        if(pushVelocity.x > 0.1f || pushVelocity.x < -0.1f){
            pushVelocity.x -= Mathf.Sign(pushVelocity.x) * 0.1f;
        }
        else{
            pushVelocity.x = 0;
        }
        if(pushVelocity.y > 0.1f || pushVelocity.y < -0.1f){
            pushVelocity.x -= Mathf.Sign(pushVelocity.y) * 0.1f;
        }
        else
        {
            pushVelocity.y = 0;
        }
    }

    public void PushBack(Vector2 push){
        pushVelocity = push;
    }
}
