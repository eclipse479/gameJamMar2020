using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentLifeHealth = 100;
    public Image heartIndicator;
    private float damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        damageAmount = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            takeDamage();
            Destroy(collision.gameObject);
        }
    }

    public void takeDamage()
    {
        currentLifeHealth -= damageAmount;
        Debug.Log(currentLifeHealth);

        heartIndicator.fillAmount = currentLifeHealth / 100;
      
        if (currentLifeHealth == 0)
        {
            this.gameObject.SetActive(false);
            this.GetComponent<PlayerControls>().ammoTracker.gameObject.SetActive(false);
        }

    }
}
