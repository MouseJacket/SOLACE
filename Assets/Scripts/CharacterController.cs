using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject planet;
    public float move;
    public float speed;
    //public GroundCollider floorCollider;

    public GravPull grav;
    public UnityEngine.Vector3 gravity;
    public float jumpForce;
    public Rigidbody rb;
    public RaycastHit standingRay;
    [SerializeField]
    private float obstacleDistance;
    public float standDistance;
    public float runSpeed;
    public GameManager gameMan;
    public TextMeshProUGUI shipPrompt;
    public Animator charAnimator;
    private ShipStateManager shipState;
    public bool running;
    public bool standing;
    public float knockback;
    public bool vulnerable;
    public float invulnerabilitySeconds;
    [SerializeField] private float knockbackUp;
    [SerializeField] private float knockbackUpAdd;
    [SerializeField] private float knockbackMinimum;


    [SerializeField]
    private AudioClip audRun;
    [SerializeField]
    private AudioClip audWalk;
    [SerializeField]
    private AudioClip audDamage;
    [SerializeField]
    private float minStandDistance;

    [SerializeField] private Optimizer optimizer;


    private float audWalkMultiplier = 2f;
    public AudioSource audioSource;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grav = GetComponent<GravPull>();
        gameMan = FindFirstObjectByType<GameManager>();
        shipPrompt = GameObject.Find("ShipPrompt").GetComponent<TextMeshProUGUI>();
        charAnimator = transform.GetChild(0).GetComponent<Animator>();
        shipState = FindFirstObjectByType<ShipStateManager>();
        audioSource = GetComponent<AudioSource>();
        optimizer = FindFirstObjectByType<Optimizer>();
    }

    void OnEnable()
    {
        StartCoroutine("PlanetPick");
    }

    void OnDisable()
    {
        optimizer.EnableAllPlanets();
    }

    private IEnumerator PlanetPick()
    {
        yield return new WaitForSeconds(1);
        optimizer.OptimizePlanets(planet);
    }
    void LateUpdate()
    {
        (bool standing, bool obstacleLeft, bool obstacleRight) = CollisionDetection();
        float runMultiplier = 1f;

        
        if (standing)
        {
            rb.isKinematic = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                runMultiplier = runSpeed;
                running = true;
            }
            else
            {
                running = false;
            }
            if (move != 0)
            {
                if (running)
                {
                    AudioPlay(audRun,1f,true);
                    charAnimator.Play("NovaRun");
                }
                else
                {
                    AudioPlay(audWalk,audWalkMultiplier,true);
                    charAnimator.Play("NovaWalk");
                }
            }
            else
            {
                charAnimator.Play("NovaIdle");
                audioSource.Stop();
            }
        }
        else
        {
            rb.isKinematic = false;
            audioSource.Stop();
            
            try
            {
                string currentClip = charAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                if (currentClip != "NovaJumpUp")
                {
                    charAnimator.Play("NovaAirtime");
                }
            }
            catch
            {
                charAnimator.Play("NovaAirtime");
            }
        }
        float distance = UnityEngine.Vector3.Distance(transform.position, planet.transform.position);
        move = Input.GetAxis("Horizontal");
        if ((move > 0 && obstacleLeft)||(move < 0 && obstacleRight))
        {
            move = 0;
        }

        transform.RotateAround(planet.transform.position, UnityEngine.Vector3.forward,
            (speed*Time.deltaTime*move*runMultiplier)/(distance*distance));
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LandingPad")
        {
            shipPrompt.text="F to Enter";
            if (Input.GetKey(KeyCode.F))
            {
                shipState.ShipEnter();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (new List<string> {"Enemy", "Hazard"}.Contains(other.gameObject.tag) && vulnerable)
        {
            HazardDamage(-move*transform.right,gameMan.hazardDamage);
        }
        if (other.gameObject.layer == 11)
        {
            SunCollect(other.gameObject);
        }


    }
  void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LandingPad")
        {
            shipPrompt.text="";
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (new List<string> {"Enemy", "Hazard"}.Contains(collision.gameObject.tag) && vulnerable)
        {
            HazardDamage(-collision.relativeVelocity.normalized,gameMan.hazardDamage);
        }
    }
    void HazardDamage(UnityEngine.Vector3 knockbackDir, float damage)
    {
        AudioPlay(audDamage);
        transform.position += transform.up*knockbackUpAdd;
        standing = false;
        rb.isKinematic = false;
        rb.velocity *= 0;
        UnityEngine.Vector3 knockbackVec = knockback * (knockbackDir + (transform.up * knockbackUp));
        if (knockbackVec.magnitude > knockbackMinimum)
        {
            rb.velocity += knockbackVec;
        }
        else
        {
            rb.velocity += knockbackMinimum*transform.up;
        }
        gameMan.health -= damage;
        StartCoroutine(Invulnerable(invulnerabilitySeconds));
    }
    public IEnumerator Invulnerable(float invulnerableSecs)
    {
        SpriteRenderer sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        vulnerable = false;
        sprite.color = Color.red;
        yield return new WaitForSeconds(invulnerableSecs);
        vulnerable = true;
        sprite.color=Color.white;
    }
    public void AudioPlay(AudioClip clip, float pitch = 1f, bool random = false)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.pitch = pitch;
            if (random)
            {
                audioSource.time = Random.Range(0,clip.length);
            }
            audioSource.Play();
        }
        
    }

    void SunCollect(GameObject piece)
    {
        gameMan.health = gameMan.maxHealth;
        Debug.Log("Piece Gained");
        gameMan.pieceCount++;
        Destroy(piece);       
    }

    public (bool standing, bool obstacleLeft, bool obstacleRight) CollisionDetection()
    {
        Debug.DrawRay(transform.position,-transform.up*standDistance, Color.white);

        //Replace with collision based
        standing = Physics.SphereCast(transform.position, 0.3f, -transform.up, out standingRay, standDistance);

        if (standing&&standingRay.collider.gameObject.tag=="Hazard"&&vulnerable)
        {
            HazardDamage(transform.up,gameMan.hazardDamage);
        }
        
        if (standing&&standingRay.collider.gameObject.tag=="Enemy"&&vulnerable)
        {
            standing = false;
            rb.isKinematic = false;
            rb.velocity *= 0;
            rb.velocity += knockback*transform.up;
            standingRay.collider.gameObject.GetComponent<EnemyKillScript>().StartCoroutine("EnemyDeath");
        }
        
        
        bool colliderLeft = Physics.SphereCast(transform.position, 0.3f, -transform.right, out RaycastHit leftRay, obstacleDistance);
        bool colliderRight = Physics.SphereCast(transform.position, 0.3f, transform.right, out RaycastHit rightRay, obstacleDistance);
        bool obstacleLeft = colliderLeft && !leftRay.collider.isTrigger;
        bool obstacleRight = colliderRight && !rightRay.collider.isTrigger;
        if (obstacleLeft && leftRay.collider.gameObject.layer == 11)
        {
            SunCollect(leftRay.collider.gameObject);
        }
        else if (obstacleRight && rightRay.collider.gameObject.layer == 11)
        {
            SunCollect(leftRay.collider.gameObject);
        }
        
        if (move > 0)
        {
            charAnimator.gameObject.transform.localScale = new UnityEngine.Vector3(1,1,1); //Bullshit but it works
        }
        else if (move < 0)
        {
            charAnimator.gameObject.transform.localScale = new UnityEngine.Vector3(-1,1,1);
        }

        
        if (standing && standingRay.distance < minStandDistance)
        {
            Debug.Log($"in ground - {standingRay.distance}");
            transform.position+=transform.up*0.2f;
        }
        return (standing, obstacleLeft, obstacleRight);
    }
    void Jump()
    {
        transform.position+=transform.up;
        rb.isKinematic = false;
        rb.velocity += jumpForce*transform.up;
        charAnimator.Play("NovaJumpUp");
    }
}
