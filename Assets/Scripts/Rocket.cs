using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *Code is not very "Clean". But this is intended since I'm not quite sure how Audio should be handeled with Unity.
 *Delegates and event listeners would be my guess. I'll come back to clean this after I learn a little bit more about Unity organization.
 */

public class Rocket : MonoBehaviour {

    Rigidbody ship_rigidbody;
    AudioSource ship_audio;
    Vector3 thrust;
    Vector3 direction;
    bool Alive = true;
    int nextSceneIndex;
    [SerializeField] float thrust_Speed = 1;
    [SerializeField] float max_Thrust = 3;
    [SerializeField] float turn_Speed = 1;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip engine;
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem loseParticles;
    

    // Use this for initialization
    void Start () {
        ship_rigidbody = GetComponent<Rigidbody>();
        ship_audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Alive)
        {
            HandleInput();
        }
        
    }

    private void HandleInput()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        ship_rigidbody.velocity = Vector3.ClampMagnitude(ship_rigidbody.velocity, max_Thrust);
        
        if (Input.GetAxis("Vertical") > 0)
        {
            ApplyThrust();
            engineParticles.Play();
        }
        else
        {
            ship_audio.Stop();
            engineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        ship_rigidbody.AddRelativeForce(Vector3.up * thrust_Speed);

        if (!ship_audio.isPlaying)
        {
            ship_audio.PlayOneShot(engine);

        }
    }

    private void Rotate()
    {

        if (Input.GetAxis("Horizontal") != 0)
        {
            ship_rigidbody.angularVelocity = Vector3.zero;
            direction = new Vector3(0, 0, Input.GetAxis("Horizontal") * -1);
            transform.Rotate(direction * turn_Speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friend":
                break;
            case "Goal":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }        
    }

    private void StartDeathSequence()
    {
        Alive = false;
        ship_audio.Stop();
        ship_audio.PlayOneShot(lose);
        loseParticles.Play();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, 1f));
    }

    private void StartSuccessSequence()
    {
        Alive = false;
        ship_audio.Stop();
        ship_audio.PlayOneShot(win);
        winParticles.Play();
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            StartCoroutine(LoadLevel(nextSceneIndex, 1f));
        }
        
    }

    IEnumerator LoadLevel(int num, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(num);
    }
}