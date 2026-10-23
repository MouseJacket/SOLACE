using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyKillScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SphereLerp movement;
    private Optimizer _optimizer;

    void Start()
    {
        movement = gameObject.GetComponent<SphereLerp>();
        anim = gameObject.transform.GetComponentInChildren<Animator>();
        anim.Play("FoiliageWalk");
        _optimizer = FindFirstObjectByType<Optimizer>();
    }
    
    public IEnumerator EnemyDeath()
    {
        Debug.Log("Kill");
        movement.enabled = false;
        anim.speed = 1f;
        anim.SetTrigger("Kill");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        _optimizer.enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
