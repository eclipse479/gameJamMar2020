using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 startDirection;
    public float speed = 200f;
    public int damage = 1;
    public float lifetime = 5f;
    public Color color;

    public GameObject explosionPrefab;

    [HideInInspector]
    public bool explosive = false;

    private Rigidbody rb;
    private float lifeTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = startScale;
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    void Start()
    {
        rb.AddForce(startDirection * speed, ForceMode.Impulse);
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifetime)
        {
            if (explosive)
                Explode();
            
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosive)
            Explode();
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
