using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class RocketController : MonoBehaviour
{
    public float thrustForce;
    public float rotateSpeed;
    public float rotate;
    public float thrust;
    public GameManager gameMan;
    private Rigidbody rb;
    [SerializeField]
    private VisualEffect flame;
    [SerializeField]
    private VisualEffect smoke;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameMan = FindFirstObjectByType<GameManager>();
    }
  void Awake()
    {
        flame.Stop();
        smoke.Stop();
        flame.playRate = 2f;
        smoke.playRate = 2f;
    }
  void FixedUpdate()
    {
        if (!gameMan.onFoot)
        {
            rotate = Input.GetAxis("Horizontal")*rotateSpeed*Time.deltaTime;
            rb.AddTorque(new Vector3(0,0,rotate));
            thrust = Input.GetAxis("Vertical")*thrustForce;
            rb.AddForce(transform.up*thrust);
            if (thrust > 0)
            {
                flame.Play();
                smoke.Play();
            }
            else
            {
                flame.Stop();
                smoke.Stop();
            }
        }
    }
}
