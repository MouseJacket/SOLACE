using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SphereLerp : MonoBehaviour
{
    [SerializeField] private float waitTime;
    public string status;
    public bool waiting;
    [SerializeField] private float percent;
    [SerializeField] private float angleRange;
    [SerializeField] private float deltaAngle;
    [SerializeField] private GameObject rotateObj;
    [SerializeField] private float anglePercent;
    [SerializeField] private UnityEngine.Vector3 gravDir;
    [SerializeField] private Animator anim;

    void Start()
    {
        List<string> states = new List<string> {"forward", "backward", "wait1", "wait2"};
        if (!states.Contains(status))
        {
            status = "forward";
        }
        anglePercent = deltaAngle/angleRange;
        anim = GetComponentInChildren<Animator>();
        angleRange = math.abs(angleRange);
        if (status == "backward")
        {
            percent = 1f;
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, 
                gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }
    
    void FixedUpdate()
    {
        gravDir = (transform.position - rotateObj.transform.position).normalized;
        transform.up = -gravDir;
        switch (status)
        {
            case "forward":
                percent += anglePercent;
                transform.RotateAround(rotateObj.transform.position, Vector3.forward, deltaAngle);

                if (percent >= 1f)
                {
                    percent = 1f;
                    status = "wait1";
                }
                break;

            case "backward":
                percent -= anglePercent;
                transform.RotateAround(rotateObj.transform.position, Vector3.forward, -deltaAngle);

                if (percent <= 0f)
                {
                    percent = 0f;
                    status = "wait2";
                }
                break;

            case "wait1":
            case "wait2":
                if (!waiting)
                    StartCoroutine(Wait());
                break;
        }
    }
    
    IEnumerator Wait()
    {
        waiting = true;
        if (anim)
        {
            anim.speed = 0f;
        }
        yield return new WaitForSeconds(waitTime);
        switch (status)
        {
            case "wait1":
                status = "backward";
                break;
            case "wait2":
                status = "forward";
                break;
        }
        waiting = false;
        if (anim)
        {
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, 
                gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            anim.speed = 1f;
        }
    }
}
