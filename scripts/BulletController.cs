using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 startDirection;
    public Vector3 forceToAdd;
    public float speed = 200f;
    public float lifetime = 5f;
    public bool fast = false;
    private bool hasMoved = false;
    private Rigidbody rb;
    private float lifeTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = startScale;
    }

    void Start()
    {
       


    }

    void Update()
    {
        if(!hasMoved)
        {
            rb.AddForce(startDirection * speed, ForceMode.Impulse);
            hasMoved = true;
        }
        if(fast)
        {
        rb.AddForce(startDirection * speed * 2 , ForceMode.Impulse);
            fast = false;
        }
       
        if(forceToAdd != new Vector3(0,0,0))
        {
            rb.AddForce(forceToAdd * speed, ForceMode.Impulse);
            forceToAdd = new Vector3(0, 0, 0);
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifetime)
        {
            Destroy(gameObject);
        }




    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}