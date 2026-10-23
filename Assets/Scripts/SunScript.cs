using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SunScript : MonoBehaviour
{
    public Material sunMat;
    public float distance;
    public GameObject rocket;
    public GameObject gameOverScreen;
    public float glowDist;
    [SerializeField]
    private VisualEffect solarFlares;
    private GameManager gameMan;
    void Start()
    {
        gameMan = FindFirstObjectByType<GameManager>();
        sunMat=GetComponent<MeshRenderer>().material;
        rocket = FindFirstObjectByType<RocketController>().gameObject;
    }
  void Awake()
  {
    solarFlares.playRate = 0.05f;
  }

  // Update is called once per frame
  void Update()
    {
        distance = UnityEngine.Vector3.Distance(transform.position, rocket.transform.position);
        if (distance<=glowDist)
        {
            sunMat.SetFloat("_Brightness",15*(1+(1-(distance/glowDist))));
        }
        else
        {
            sunMat.SetFloat("_Brightness",15);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == rocket)
        {
            Destroy(rocket);
            gameMan.GameOver();
        }
    }
}
