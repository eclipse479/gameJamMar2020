using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 startDirection;
    public Vector3 forceToAdd;
    public float speed = 200f;
    public int damage = 1;
    public float lifetime = 5f;
    public Color color;

    public GameObject explosionPrefab;

    [HideInInspector]
    public bool explosive = false;
    public bool fast = false;
    private bool hasMoved = false;
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
    }

    void Update()
    {
        if (!hasMoved)
        {
            rb.AddForce(startDirection * speed, ForceMode.Impulse);
            hasMoved = true;
        }
        if (fast)
        {
            rb.AddForce(startDirection * speed * 2, ForceMode.Impulse);
            fast = false;
        }

        if (forceToAdd != new Vector3(0, 0, 0))
        {
            rb.AddForce(forceToAdd * speed, ForceMode.Impulse);
            forceToAdd = new Vector3(0, 0, 0);
        }
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
