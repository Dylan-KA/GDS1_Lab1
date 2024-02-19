using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float movementSpeed = 10;
    int MaxCarryingCapacity = 3;
    int SoldiersCarrying = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        Vector2 newDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(newDirection * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision occurred");
        if (collision.gameObject.tag == "Soldier" && (SoldiersCarrying < MaxCarryingCapacity))
        {
            Destroy(collision.gameObject);
            SoldiersCarrying += 1;
            //Debug.Log("Soldier Picked up");
        }
        if (collision.gameObject.tag == "Hospital")
        {
            SoldiersCarrying = 0;
            //Debug.Log("Soldiers Dropped off");
        }
    }
}
