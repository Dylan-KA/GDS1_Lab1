using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float movementSpeed = 10;
    int MaxCarryingCapacity = 3;
    int SoldiersCarrying = 0;
    int SoldiersRescued = 0;
    [SerializeField] TextMeshProUGUI CarryingText;
    [SerializeField] TextMeshProUGUI RescuedText;
    [SerializeField] SoldierSpawning soldierSpawning;

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
            CarryingText.text = "Soldiers In Helicopter: " + SoldiersCarrying;
            //Debug.Log("Soldier Picked up");
        }
        if (collision.gameObject.tag == "Hospital")
        {
            SoldiersRescued += SoldiersCarrying;
            SoldiersCarrying = 0;
            CarryingText.text = "Soldiers In Helicopter: " + SoldiersCarrying;
            RescuedText.text = "Soldiers Rescued: " + SoldiersRescued;
            WinConditionCheck();
            //Debug.Log("Soldiers Dropped off");
        }
    }

    private void WinConditionCheck()
    {
        // If rescued all soldiers
        if (SoldiersRescued == soldierSpawning.numSoldiers)
        {
            Debug.Log("You Win!");
        }
    }
}
