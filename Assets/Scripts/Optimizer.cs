using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.GlobalIllumination;

public class Optimizer : MonoBehaviour
{
    public bool optimizePlanets;
    public List<Light> lights;
    public List<GameObject> enemies;
    public List<GameObject> planets;
    [SerializeField] private float lightDistance;
    [SerializeField] private float enemyDistance;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject sun;
    // Start is called before the first frame update
    void Start()
    {
        //Lights
        lights = new List<Light>(FindObjectsByType<Light>(FindObjectsSortMode.None));
        player = FindFirstObjectByType<CharacterController>().gameObject;
        sun = FindFirstObjectByType<SunScript>().gameObject;
        lights.Remove(sun.GetComponent<Light>());
        lights.Remove(player.GetComponent<Light>());
        
        //Enemies
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //Planets
        planets = GameObject.FindGameObjectsWithTag("LevelDetails").ToList();
    }
    void Update()
    {
        OptimizeLights();
        //OptimizeEnemies();
    }

    public void EnableAllPlanets()
    {
        foreach (GameObject planet in planets)
        {
            planet.SetActive(true);
        }
    }

    public void OptimizePlanets(GameObject landed)
    {
        if (optimizePlanets)
        {
            foreach (GameObject planet in planets)
            {
                if (planet.transform.parent.gameObject == landed)
                {
                    planet.SetActive(true);
                }
                else
                {
                    planet.SetActive(false);
                }
            }
        }
    }

    private void OptimizeLights()
    {
        foreach (Light light in lights)
        {
            if (Vector3.Distance(player.transform.position, light.gameObject.transform.position) > lightDistance)
            {
                light.enabled = false;
            }
            else
            {
                light.enabled = true;
            }
        }
    }

    private void OptimizeEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            
            if (Vector3.Distance(player.transform.position, enemy.gameObject.transform.position) > enemyDistance)
            {
                enemy.SetActive(false);
            }
            else
            {
                enemy.SetActive(true);
            }
        }
    }
}
