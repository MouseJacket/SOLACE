using System.Collections;
using System.Collections.Generic;
using TMPro;
using Cinemachine;
using UnityEngine;

public class ShipStateManager : MonoBehaviour
{
    public GameObject player;
    public GameObject rocket;
    [SerializeField]
    private CinemachineVirtualCamera playerCam;
    [SerializeField]
    private CinemachineVirtualCamera rocketCam;
    [SerializeField]
    private TextMeshProUGUI shipPrompt;
    [SerializeField]
    private TextMeshProUGUI piecePrompt;
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private GameObject rocketUI;
    private GameManager gameMan;
    [SerializeField]
    private GameObject playerHatch;
    void Start()
    {
        player = FindFirstObjectByType<CharacterController>().gameObject;
        rocket = FindFirstObjectByType<RocketController>().gameObject;
        gameMan = FindFirstObjectByType<GameManager>();
    }
    public void ShipExit(GameObject planet) //Seamless
    {
        playerUI.SetActive(true);
        rocketUI.SetActive(false);
        player.SetActive(true);
        shipPrompt.text="";
        Debug.Log($"Exit - {planet}");
        playerCam.enabled=true;
        player.transform.position = playerHatch.transform.position;
        gameMan.onFoot = true;
        player.transform.SetParent(planet.transform);
        player.GetComponent<GravPull>().orbitTarget = planet;
        player.GetComponent<CharacterController>().planet = planet;
    }
    public void ShipEnter() //Seamless
    {
        playerUI.SetActive(false);
        rocketUI.SetActive(true);
        shipPrompt.text="Space to Exit";
        player.transform.SetParent(null);
        player.SetActive(false);
        playerCam.enabled=false;
        gameMan.onFoot = false;
    }
}
