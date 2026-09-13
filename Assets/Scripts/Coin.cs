using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    bool wasCollected = false;


    void Start()
    {

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (wasCollected == false)
            {
                wasCollected = true;
                FindObjectOfType<PlayerMoney>().AddMoney(1);
                gameObject.SetActive(false);
                Destroy(gameObject);

            }
        }


    }

}
