using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownController : MonoBehaviour
{
	public float moveSpeed = 1;
	public float jumpSpeed = 1;

	Rigidbody2D rb;
	CharacterControl control;
	bool pause = false;
	bool grounded = true;

    // Use this for initialization
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine(Loop());
    }

    // Update is called once per frame
    void Update()
    {

    }

	IEnumerator Loop(){
		while(true){
			control.GetInput();
			if(!pause){
				Vector3 move = GetMovement();
				move += Jump();
				rb.velocity = move;
			}
			yield return null;
		}
	}

	Vector3 GetMovement(){
		Vector3 move = control.DirectionInput;
		Debug.Log("Given input" + move);
		return move * (moveSpeed * 10) * Time.deltaTime;
	}

	Vector3 Jump(){
		if(grounded && control.GetButton(0)){
			return new Vector3(0, 10 * jumpSpeed * Time.deltaTime, 0);
		}
		return Vector3.zero;
	}
}
