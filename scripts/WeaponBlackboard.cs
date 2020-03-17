using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBlackboard : MonoBehaviour
{
    public static WeaponBlackboard instance;

    public Image gunType;
    public Sprite normalGun;
    public Sprite bounceGun;
    public Sprite explosiveGun;
    public Sprite fastGun;
    public Sprite spreadGun;
    public Sprite burstGun;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
