using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *Code is not very "Clean". But this is intended since I'm not quite sure how Audio should be handeled with Unity.
 *Delegates and event listeners would be my guess. I'll come back to clean this after I learn a little bit more about Unity organization.
 */

public class GoalDisappear : MonoBehaviour {

    [SerializeField] Vector3 target_point = new Vector3(1, 1, 1);
    [SerializeField] float speed = 1;
    private Transform Goal;
    private bool triggered = false;    
    
	// Use this for initialization
	void Start () {
        Goal = transform.parent;
	}

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }

    private void DisappearMe()
    {
        float step = speed * Time.deltaTime;
        Goal.position = Vector3.MoveTowards(Goal.position, target_point, step);
    }

    private void Update()
    {
        if (triggered)
        {
            DisappearMe();
        }
    }
}
