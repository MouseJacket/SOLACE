using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // LateUpdate is called once per frame after all Update functions have been completed
    void LateUpdate()
    {
        // This makes the UI face the same direction as the camera is looking
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}