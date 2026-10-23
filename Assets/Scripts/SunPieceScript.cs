using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPieceScript : MonoBehaviour
{
    public Material sunMat;
    public float distance;
    public GameObject player;
    public GameObject gameOverScreen;
    public float glowDist;
    public GameManager gameMan;
    private Optimizer _optimizer;
    void Start()
    {
        sunMat=GetComponent<MeshRenderer>().material;
        player = FindFirstObjectByType<CharacterController>().gameObject;
        gameMan = FindFirstObjectByType<GameManager>();
        _optimizer = FindFirstObjectByType<Optimizer>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = UnityEngine.Vector3.Distance(transform.position, player.transform.position);
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
        if (other.gameObject == player)
        {
            gameMan.health = gameMan.maxHealth;
            Debug.Log("Piece Gained");
            gameMan.pieceCount++;
            _optimizer.lights.Remove(gameObject.GetComponent<Light>());
            Destroy(gameObject);
        }
    }
}
