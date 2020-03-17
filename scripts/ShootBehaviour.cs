using System.Collections;
using UnityEngine;
using XboxCtrlrInput;

public class ShootBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private Vector3 shootDirection;
    private float shootRate = 0.5f;
    private float shootTimer;

    void Awake()
    {
        shootTimer = shootRate;
        shootDirection = new Vector3(1f, 0f, 0f);
    }

    void Update()
    {
        float shootX = XCI.GetAxisRaw(XboxAxis.RightStickX, XboxController.All);
        float shootY = XCI.GetAxisRaw(XboxAxis.RightStickY, XboxController.All);

        if (!(Mathf.Approximately(shootX, 0.0f) && Mathf.Approximately(shootY, 0.0f)))
        {
            shootDirection = new Vector3(shootX, shootDirection.y, shootY);
            shootDirection.Normalize();
        }

        shootTimer += Time.deltaTime;
        if (shootTimer > shootRate)
        {
            if (!Mathf.Approximately(XCI.GetAxisRaw(XboxAxis.RightTrigger, XboxController.All), 0.0f) || Input.GetKey(KeyCode.Space))
            {
                //StartCoroutine(ShootThree());
                ExplosiveShot();
                shootTimer = 0f;
            }
        }
    }

    IEnumerator ShootThree()
    {
        for (int i = 0; i < 3; i++)
        {
            BulletController bulletSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletController>();
            bulletSpawn.startDirection = shootDirection;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ExplosiveShot()
    {
        BulletController bulletSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletController>();
        bulletSpawn.startDirection = shootDirection;
        bulletSpawn.speed = 500f;
        bulletSpawn.explosive = true;
    }
}
