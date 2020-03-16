using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 startDirection;
    public float speed = 200f;
    public float lifetime = 5f;

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
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
