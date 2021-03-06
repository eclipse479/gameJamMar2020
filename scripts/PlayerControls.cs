﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerControls : MonoBehaviour
{
    float xInput;
    float yInput;
    float xRotationInput;
    float yRotationInput;
    public float playerSpeed = 1;
    public int playerNumber;
    public float maxShootingTimer = 3;
    public float burstTimer = 1.0f;
    float shootingTimer;
    private Vector3 shootDirection;
    public GameObject projectile;
    public Transform spawner;
    Rigidbody body;
    private Vector3 lookDirrection;

    public Image ammoTracker;
    public float maxAmmo;
    private float currentAmmo;
    public enum bulletType
    {
        normal,
        bouncy,
        explosive,
        fast,
        spread,
        burst
    };

    public bulletType currentBullet;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        body = gameObject.GetComponent<Rigidbody>();
        shootingTimer = maxShootingTimer;
        currentBullet = bulletType.normal;
    }

    // Update is called once per frame
    void Update()
    {
        //gets the input form the left control stick form the Xbox controller
        xInput = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerNumber);
        yInput = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerNumber);
        //makes the player move speed more uniform
        xInput *= playerSpeed * Time.deltaTime;
        yInput *= playerSpeed * Time.deltaTime;
        //moves the player
        Vector3 position = new Vector3(xInput, 0, yInput);
        transform.Translate(position, Space.World);
        //when the "A" button is held down
        if (XCI.GetButton(XboxButton.RightBumper, (XboxController)playerNumber) && shootingTimer >= maxShootingTimer && currentAmmo > 0)
        {
            shootProjectile();
        }
        xRotationInput = XCI.GetAxis(XboxAxis.RightStickX, (XboxController)playerNumber);
        yRotationInput = XCI.GetAxis(XboxAxis.RightStickY, (XboxController)playerNumber);


        if (!(Mathf.Approximately(xRotationInput, 0.0f) && Mathf.Approximately(yRotationInput, 0.0f)))
        {
            shootDirection = new Vector3(xRotationInput, 0, yRotationInput);
            shootDirection.Normalize();

        }

        //deadzone on joysticks
        if (xRotationInput < 0.1f && xRotationInput > -0.1f)
            xRotationInput = 0;
        if (yRotationInput < 0.1f && yRotationInput > -0.1f)
            yRotationInput = 0;
        //rotate the playre in the direction the right joystick is faceing
        if (xRotationInput != 0 || yRotationInput != 0)
        {
            lookDirrection = new Vector3((xRotationInput), 0, (yRotationInput));
            lookDirrection.Normalize();
            transform.rotation = Quaternion.LookRotation(lookDirrection, new Vector3(0, 1, 0));
        }
        //shooting timer count
        if (shootingTimer < maxShootingTimer)
            shootingTimer += Time.deltaTime;

        body.velocity = new Vector3(0, 0, 0);
    }

    void shootProjectile()
    {
        currentAmmo--;
        switch (currentBullet)
        {
            case bulletType.normal:
                {
                    //default bullet
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    break;
                }
            case bulletType.bouncy:
                {
                    //default bullet
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn.startDirection = shootDirection;
                    bulletSpawn.bouncy = true;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    break;
                }
            case bulletType.explosive:
                {
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    //makes the bullet larger
                    bulletSpawn.transform.localScale += new Vector3(1.5f, 1.5f, 1.5f);
                    bulletSpawn.explosive = true;
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    break;
                }
            case bulletType.fast:
                {
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    //makes the bullet faster
                    bulletSpawn.fast = true;
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    break;
                }
            case bulletType.spread:
                {
                    //first
                    BulletController bulletSpawn = Instantiate(projectile, (spawner.transform.position), Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    //second
                    BulletController bulletSpawn2 = Instantiate(projectile, (spawner.transform.position), (Quaternion.identity)).GetComponent<BulletController>();
                    bulletSpawn2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    shootDirection = Quaternion.AngleAxis(20, Vector3.up) * shootDirection;
                    bulletSpawn2.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn2.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    //third
                    BulletController bulletSpawn3 = Instantiate(projectile, (spawner.transform.position), Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn3.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    shootDirection = Quaternion.AngleAxis(-40, Vector3.up) * shootDirection;
                    bulletSpawn3.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn3.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    break;
                }
            case bulletType.burst:
                    {
                    StartCoroutine(ShootThree());
                    break;
                }
        }
        shootingTimer = 0;
        if(currentAmmo == 0)
        {
            currentBullet = bulletType.normal;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "powerUp")
        {
            powerUp newpower = collision.gameObject.GetComponent<powerUp>();
            currentBullet = (bulletType)newpower.currentPower;
            switch (currentBullet)
            {
                case bulletType.normal:
                    {
                        currentAmmo = 10;
                        break;
                    }
                case bulletType.bouncy:
                    {
                        currentAmmo = 10;
                        break;
                    }
                case bulletType.explosive:
                    {
                        currentAmmo = 4;
                        break;
                    }
                case bulletType.fast:
                    {
                        currentAmmo = 7;
                        break;
                    }
                case bulletType.spread:
                    {
                        currentAmmo = 10;
                        break;
                    }
                case bulletType.burst:
                    {
                        currentAmmo = 5;
                    break;
                 }
            }
             Destroy(collision.gameObject);
        }
    }

    IEnumerator ShootThree()
    {
        for (int i = 0; i < 3; i++)
        {
            BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
            bulletSpawn.bursting = true;
            bulletSpawn.startDirection = shootDirection;
            Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            yield return new WaitForSeconds(burstTimer);
        }
    }
}

