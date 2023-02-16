using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth1 : MonoBehaviour
{
    [SerializeField] public int Health = 1; 
    // Start is called before the first frame update
    void Start()
    {
        Health = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0) {
            Debug.Log("Died!!!!!!");

            GameManager.dead = true;
            //make enemy spawner stop
            //end game
            //hello pranav
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Enemy")
        {
            Health--;
            Debug.Log("AHHHH");
        }
        Debug.Log("AHHHH");
    }

    // public void TakeDamage(int damage) {
    //     Debug.Log("Kinda Died!!!!!!");
    //     health = health - damage;

    // }
}
