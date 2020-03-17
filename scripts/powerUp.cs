using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public enum powerUpType
    {
        bouncy,
        explosive,
        fast,
        spread,
    };

    public powerUpType currentPower;
    private int rand;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 100; i++)
        {
        rand = Random.Range(0, 4);
            Debug.Log(rand);
        }
        switch (rand)
        {
            case 0:
                {
                    currentPower = powerUpType.bouncy;
                    break;
                }
            case 1:
                {
                    currentPower = powerUpType.explosive;
                    break;
                }
            case 2:
                {
                    currentPower = powerUpType.fast;
                    break;
                }
            case 3:
                {
                    currentPower = powerUpType.spread;
                    break;
                }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
