using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenWallBehaviour : MonoBehaviour {

    public Animator lowPunch, medPunch, main;
    public int orbDamage = 0;

	// Use this for initialization
	void Start () {
        StartCoroutine(RandomActivate(lowPunch));
        StartCoroutine(RandomActivate(medPunch));
	}
	
	// Update is called once per frame
	void Update () {
        if(orbDamage > 10){
            main.SetBool("OpenShell", true);
        }
        else{
            main.SetBool("OpenShell", false);
        }
	}

    IEnumerator RandomActivate(Animator puncher){
        while(true){
            float num = Random.Range(1, 10);
            yield return new WaitForSeconds(num);
            puncher.SetBool("Punch", true);
            yield return new WaitForSeconds(4);
            puncher.SetBool("Punch", false);
        }
    }
}
