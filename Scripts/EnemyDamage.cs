using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public PlayerHealth1 playerHealth;

    public void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("|"+collision.gameObject.tag+"|"+collision.gameObject.tag.Equals("Player"));
        if(collision.gameObject.tag.Equals("Player")) {
            playerHealth.Health = playerHealth.Health - damage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
