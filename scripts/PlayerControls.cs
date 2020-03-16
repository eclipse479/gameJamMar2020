
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
    float shootingTimer;
    private Vector3 shootDirection;
    public GameObject projectile;
    public Transform spawner;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        shootingTimer = maxShootingTimer;
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
        if (XCI.GetButton(XboxButton.RightBumper,(XboxController)playerNumber) && shootingTimer >= maxShootingTimer)
        {
            Debug.Log("A is being pressed");
            shootProjectile();
        }
        xRotationInput =  XCI.GetAxis(XboxAxis.RightStickX,(XboxController)playerNumber);
        yRotationInput =  XCI.GetAxis(XboxAxis.RightStickY,(XboxController)playerNumber);
        
        
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
            Vector3 lookDirrection = new Vector3((xRotationInput),0,(yRotationInput));
            lookDirrection.Normalize();
            transform.rotation = Quaternion.LookRotation(lookDirrection, new Vector3(0,1,0));
        }
        //shooting timer count
        if (shootingTimer < maxShootingTimer)
        shootingTimer += Time.deltaTime;

        body.velocity = new Vector3(0, 0, 0);
    }

    void shootProjectile()
    {
        BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
        bulletSpawn.startDirection = shootDirection;
        Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        shootingTimer = 0;
    }


}
