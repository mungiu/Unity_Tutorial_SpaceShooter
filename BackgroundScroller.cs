using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeZ;
    
    private Vector3 startPosition;

	// Use this for initialization
	void Start ()
    {
        //setting "startPosition" to position at start of game
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //looping through background position value
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        //applying "newPosition" value and forward movement relative to "startPosition"
        transform.position = startPosition + Vector3.forward * newPosition;
	}
}
