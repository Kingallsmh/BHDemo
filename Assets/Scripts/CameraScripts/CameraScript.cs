using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    Camera myCam;
    public Transform target;
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
        myCam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        myCam.orthographicSize = (Screen.height / 16f) / 4f;
        if (target)
        {
            //transform.position = Vector3.Lerp(transform.position, target.position, speed) + new Vector3(0, 0, -10f);
            transform.position = new Vector3(target.position.x, target.position.y + 0.5f, -10);
        }
    }
}
