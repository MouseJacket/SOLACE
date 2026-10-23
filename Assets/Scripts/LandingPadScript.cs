using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

public class LandingPadScript : MonoBehaviour
{
    public bool landed;
    public GameObject rocket;
    public GameObject landPlanet;
    [SerializeField]
    private GameManager gameMan;
    private ShipStateManager shipState;
    public string planetScene;

    void Start()
    {
        rocket = FindFirstObjectByType<RocketController>().gameObject;
        gameMan=FindFirstObjectByType<GameManager>();
        rocket.transform.position = transform.position;
        shipState = FindFirstObjectByType<ShipStateManager>();
    }
    public void Update()
    {
        if (landed)
        {
            rocket.transform.position = transform.position;
            rocket.transform.rotation = transform.rotation;
            
            if (Input.GetAxis("Vertical") == 1)
            {
                StopAllCoroutines(); //Make more specific later.
                rocket.transform.position = transform.position+transform.up*50;
                rocket.transform.SetParent(null);
                landed = false;
                RocketController control = rocket.GetComponent<RocketController>();
                control.thrust = control.thrustForce;
                rocket.GetComponent<Rigidbody>().AddForce(transform.up*(control.thrustForce*100f));
                rocket.GetComponent<AudioSource>().Play();
                Debug.Log("Launch");
            }
            else if (!gameMan.onFoot)
            {
                rocket.transform.position = transform.position;
                rocket.transform.up = transform.up;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (planetScene == "")
                    {
                        Debug.Log(landPlanet);
                        shipState.ShipExit(landPlanet);
                    }
                    else
                    {
                        gameMan.SceneLoad(planetScene);
                    }
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == rocket)
        {
            other.gameObject.GetComponent<Rigidbody>().velocity *= 0;
            StartCoroutine(Orient(other.gameObject));
        }
    }
    IEnumerator Orient(GameObject rocket)
    {
        UnityEngine.Vector3 padUp = transform.up;
        UnityEngine.Vector3 rocketUp = rocket.transform.up;
        UnityEngine.Vector3 rocketPos = rocket.transform.position;
        float percent = 0f;
        while(rocket.transform.up != padUp)
        {
            rocket.transform.up=UnityEngine.Vector3.Slerp(rocketUp,padUp,percent);
            rocket.transform.position = UnityEngine.Vector3.Lerp(rocketPos,transform.position,percent);
            yield return new WaitForEndOfFrame();
            percent += Time.deltaTime;
        }

        //rocket.transform.SetParent(transform);
        landed = true;
        /*
        while (landed)
        {
            rocket.transform.position = transform.position;
            rocket.transform.rotation = transform.rotation;
            yield return new WaitForEndOfFrame();
        }
        */
    }
}
