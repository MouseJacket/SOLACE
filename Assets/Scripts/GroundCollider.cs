using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public GameObject player;
    public bool onGround;
    public bool quedJump;
    void OnCollisionEnter(Collision collision)
    {
        onGround = true;
    }
    void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
    /*
    void Update()
    {
        if (quedJump)
        {
            onGround = false;
        }
    }
    */
}
