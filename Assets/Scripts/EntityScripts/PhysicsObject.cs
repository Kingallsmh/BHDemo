using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;
    public float maxVerticalSpeed = 10;
    protected Vector2 targetVelocity;
    protected bool grounded;
    protected bool unphysics;
    protected bool isBusy = false;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {        
        if (!unphysics)
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }        
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        if (unphysics)
        {
            velocity = Vector2.zero;
            return;
        }
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
        velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed, maxVerticalSpeed);

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;
        
		Debug.Log("Current normal: " + groundNormal);

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    protected void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            Collider2D[] rbC = new Collider2D[1];
            rb2d.GetAttachedColliders(rbC);
            Collider2D rbCol = rbC[0];
            Vector2 pos = new Vector2(rbCol.bounds.center.x, rbCol.bounds.center.y - rbCol.bounds.extents.y);
            Debug.DrawRay(pos, move * 100, Color.blue);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                PlatformEffector2D platform = hitBuffer[i].collider.GetComponent<PlatformEffector2D>();
                PlatformAddition platformAddition = hitBuffer[i].collider.GetComponent<PlatformAddition>();
                if (!platform)
                {
                    hitBufferList.Add(hitBuffer[i]); // get the colliding objects
                }
                else if ((velocity.y < 0 && yMovement))
                { //hitBuffer[i].normal == Vector2.up && 
                    if (hitBuffer[i].normal.y > 0)
                    {
                        Debug.Log(hitBuffer[i].normal);
                        if (rb2d.IsTouching(hitBuffer[i].collider))
                        {
                            hitBufferList.Add(hitBuffer[i]);
                        }
                    }
                }
                if(platformAddition){
                    if(platformAddition.MoveWithPlatform){
                        Debug.Log("Platform Move: " + platformAddition.rb2d.velocity);
                        rb2d.velocity += hitBuffer[i].collider.GetComponent<PlatformAddition>().rb2d.velocity;
                    }
                }
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }

	public void ResetGroundNormal(){
		groundNormal = new Vector2(0, 1);
	}

}