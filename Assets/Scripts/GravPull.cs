using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GravPull : MonoBehaviour
{
    public UnityEngine.Vector3 gravVec;
    public List<GameObject> planets;
    public float gravMultiplier = 1f;
    public float massOb;
    private Rigidbody rigidBody;
    public UnityEngine.Vector3 gravDirection;
    public float newtonsTot;
    public float distMultiplier;
    public GameObject orbitTarget;
    public GameManager gameMan;
    public CharacterController player;
    //public enum gravTypes {orbit, lock, sum }

    public string gravType;
    public bool randomizeStart;

    void Start()//Get an array of all planets in scene
    {
        if (!player)
        {
            rigidBody = GetComponent<Rigidbody>();
            massOb = rigidBody.mass;
        }
        gameMan = FindFirstObjectByType<GameManager>();
        gravMultiplier = gameMan.gravMultiplier;
        distMultiplier = gameMan.distMultiplier;
        

        planets.AddRange(GameObject.FindGameObjectsWithTag("GravBody"));
        Debug.Log($"Planets Identified = {planets}");
        
        if (gravType=="orbit" && orbitTarget!=null && randomizeStart)
        {
            transform.RotateAround(orbitTarget.transform.position, 
            UnityEngine.Vector3.forward, UnityEngine.Random.Range(0,360));
        }
    } 
    void FixedUpdate() //Newtonian solution
    {
        switch (gravType)
        {
            case ("orbit"): //Orbitting another grav body (planets around the sun)
                Orbit(orbitTarget,6.6743e-11f*gravMultiplier);
                break;
            case ("lock"): //locked to the surface of one planet (player)
                PlanetLocked(orbitTarget, 6.6743e-11f*gravMultiplier);
                break;
            case ("sum"): //Sum of the gravitational pulls of all grav bodys (rocket)
                SumGrav(6.6743e-11f*gravMultiplier);
                break;
            default:
                Debug.LogError($"No Selected Grav Type - {gameObject.name}");
                break;
        }
    }
    void SumGrav(float G = 6.6743e-11f)
    {
        gravVec = new UnityEngine.Vector3(0,0,0);
        foreach (GameObject planet in planets)
        {
            if (planet != gameObject)
            {
                float dist = UnityEngine.Vector3.Distance(transform.position, planet.transform.position)*distMultiplier;
                UnityEngine.Vector3 dir = (transform.position - planet.transform.position).normalized;
                float massP = planet.GetComponent<GravPull>().massOb;
                float gravMag = -G*(massOb*massP)/(dist*dist);
                gravVec+=gravMag*dir; //-G*(((m1*m2)/r^2)*rhat)
                
            }
        }
        rigidBody.AddForce(gravVec);
        //transform.up = -gravVec;
        newtonsTot = gravVec.magnitude;
        gravDirection = gravVec.normalized;
    }
    void Orbit(GameObject orbit, float G = 6.6743e-11f)
    {
        if (orbit != null)
        {
            float massP = orbit.GetComponent<GravPull>().massOb;
            float dist = UnityEngine.Vector3.Distance(transform.position, orbit.transform.position)*distMultiplier;
            float gravMag = -G*(massOb*massP)/(dist*dist);
            gravMag /= 2;
            transform.RotateAround(orbit.transform.position, UnityEngine.Vector3.forward, Time.deltaTime*gravMag);
        }   
    }
    void PlanetLocked(GameObject planet, float G = 6.6743e-11f)
    {
        float dist = UnityEngine.Vector3.Distance(transform.position, planet.transform.position)*distMultiplier;
        UnityEngine.Vector3 dir = (transform.position - planet.transform.position).normalized;
        float massP = planet.GetComponent<GravPull>().massOb;
        float gravMag = -G*(massOb*massP)/(dist*dist);
        gravVec=gravMag*dir; //-G*(((m1*m2)/r^2)*rhat)
        
        if (player)
        {
            player.gravity = gravVec;
        }
        else
        {
            rigidBody.AddForce(gravVec);
            transform.up = -gravDirection;
        }
        newtonsTot = gravVec.magnitude;
        gravDirection = gravVec.normalized;
    }
}
