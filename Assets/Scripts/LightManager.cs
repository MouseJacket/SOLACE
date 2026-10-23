using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightManager : MonoBehaviour
{
    public List<Light> lights;
    public float distance;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject sun;
    // Start is called before the first frame update
    void Start()
    {
        lights = new List<Light>(FindObjectsByType<Light>(FindObjectsSortMode.None));
        player = FindFirstObjectByType<CharacterController>().gameObject;
        sun = FindFirstObjectByType<SunScript>().gameObject;
        /*
        foreach (Light light in lights)
        {
            if (light.transform.parent.gameObject == sun || light.transform.parent.gameObject == player)
            {
                lights.Remove(light);
            }
        }*/
        lights.Remove(sun.GetComponent<Light>());
        lights.Remove(player.GetComponent<Light>());

    }

    void Update()
    {
        foreach (Light light in lights)
        {
            if (Vector3.Distance(player.transform.position, light.gameObject.transform.position) > distance)
            {
                light.enabled = false;
            }
            else
            {
                light.enabled = true;
            }
        }
    }
}
