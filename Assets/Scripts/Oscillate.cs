using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Code is not very "Clean". But this is intended since I'm not quite sure how Audio should be handeled with Unity.
 *Delegates and event listeners would be my guess. I'll come back to clean this after I learn a little bit more about Unity organization.
 */

[DisallowMultipleComponent]
public class Oscillate : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10, 10, 10);
    [SerializeField] float period = 10f;

    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
	}
}
