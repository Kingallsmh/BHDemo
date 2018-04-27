using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrotesqueBehaviour : MonoBehaviour, Damagable {

    public float thoughtTime = 0.4f;
    public float moveSpeed = 4;
    Rigidbody2D rb2d;
    public Animator anim;
    SpriteRenderer render;

    [Header("AtkClips")]
    public AnimationClip atk1Clip;
    Vector3 atk2BeginSpot = new Vector3(-11, -3.5f, 10);
    public float atk2Time = 3;
    public List<HitboxControl> boxList;

    public BossState currentState = BossState.Idle;
    public bool isHit = false;
    public GameObject dmgSpot;

    public enum BossState{
        Idle = 0, Move = 1, Atk1 = 2, Atk2 = 3
    }

	// Use this for initialization
	void Start () {
        StartHitbox();
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(GrotesqueLoop());
	}

    void StartHitbox(){
        for (int i = 0; i < boxList.Count; i++){
            boxList[i].damageItem = this;
        }
    }

    IEnumerator GrotesqueLoop(){
        while(true){
            switch(currentState){
                case BossState.Idle: yield return StartCoroutine(Idle(0.4f));
                    break;
                case BossState.Move: yield return StartCoroutine(MoveForward(0.4f));
                    break;
                case BossState.Atk1: yield return StartCoroutine(Atk1(atk1Clip));
                    break;
                case BossState.Atk2: yield return StartCoroutine(Atk2());
                    break;
            }
            yield return new WaitForSeconds(thoughtTime);
            DecideNextAction();
        }
    }

    void FaceInDirection(Vector2 position){
        float facing = Mathf.Sign(transform.position.x - position.x);
        transform.localScale = new Vector3(facing*transform.localScale.x, 2, 1);
    }

    IEnumerator MoveForward(float time){
        Vector2 move = Vector2.zero;
        anim.SetBool("Moving", true);
        move.x = moveSpeed * transform.localScale.x * Time.deltaTime * 10;
        rb2d.velocity = move;
        yield return new WaitForSeconds(time);
        anim.SetBool("Moving", false);
        rb2d.velocity = Vector2.zero;
    }

    IEnumerator Idle(float time){
        anim.SetBool("Moving", false);
        yield return new WaitForSeconds(time);
    }

    IEnumerator Atk1(AnimationClip atkAnim){
        anim.SetTrigger("Atk1");
        yield return new WaitForSeconds(atkAnim.length);
    }

    IEnumerator Atk2()
    {
        anim.SetTrigger("Atk2");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length - 0.1f);
        Vector3 v3 = new Vector3(-28, -3.5f, 10);
        transform.position = v3;
        rb2d.velocity = new Vector3(100 * Time.deltaTime, 0, 0);
        yield return new WaitForSeconds(atk2Time);
        anim.SetTrigger("ExitAtk");
        transform.position = atk2BeginSpot;
    }

    void DecideNextAction(){
        int rndAction = Random.Range(0, 4);
        Debug.Log(((BossState)rndAction) + " " + rndAction);
        currentState = (BossState)rndAction;
    }

    void CleaveSpot(Vector2 pos){
        dmgSpot.transform.position = new Vector3(pos.x, pos.y, -3);
        dmgSpot.GetComponent<Animator>().SetTrigger("Hit");
    }

	public void TakeDamage(int dmg, Vector2 location)
	{
        CleaveSpot(location);
	}

    public void TakeDamage(int dmg)
    {
    }
}
