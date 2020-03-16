using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 startDirection;
    public float speed = 200f;
    public float lifetime = 5f;
    public bool fast = false;

    private Rigidbody rb;
    private float lifeTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = startScale;
    }

    void Start()
    {
        rb.AddForce(startDirection * speed, ForceMode.Impulse);


    }

    void Update()
    {
        if(fast)
        {
        rb.AddForce(startDirection * speed * 2 , ForceMode.Impulse);
            fast = false;
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