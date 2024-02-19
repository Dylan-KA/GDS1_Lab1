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
    float CurrentFuel = 100;
    [SerializeField] TextMeshProUGUI CarryingText;
    [SerializeField] TextMeshProUGUI RescuedText;
    [SerializeField] TextMeshProUGUI WinLoseText;
    [SerializeField] TextMeshProUGUI FuelText;
    [SerializeField] SoldierSpawning soldierSpawning;
    Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ResetGameCheck())
        {
            PlayerMovement();
        }
    }

    private void FuelConsumption()
    {
        if (CurrentFuel > 0)
        {
            CurrentFuel -= Time.deltaTime * 5;
            float CurrentFuelRounded = Mathf.Round(CurrentFuel * 100f) / 100f;
            FuelText.text = "Fuel: " + CurrentFuelRounded;
        } else if (CurrentFuel < 0)
        {
            CurrentFuel = 0;
            //Debug.Log("Out of Fuel");
        }
        
    }

    private bool ResetGameCheck()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Reset Data & UI
            SoldiersCarrying = 0;
            SoldiersRescued = 0;
            CurrentFuel = 100;
            CarryingText.text = "Soldiers In Helicopter: " + SoldiersCarrying;
            RescuedText.text = "Soldiers Rescued: " + SoldiersRescued;
            FuelText.text = "Fuel: 100.0";
            WinLoseText.text = "";

            //Reset Player Position
            transform.position = new Vector2(-5.0f, 0.0f);

            //Remove existing Soldiers and Spawn new Soldiers
            soldierSpawning.SpawnSoldiers();
            
            //Debug.Log("Game Restarted!");
            return true;
        }
        return false;
    }

    void PlayerMovement()
    {
        if (CurrentFuel <= 0)
        {
            LoseCondition("Out of Fuel You Lose!");
            return;
        }
        Vector2 newDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(newDirection * movementSpeed * Time.deltaTime);
        if (newDirection != Vector2.zero)
        {
            FuelConsumption();
        } else
        {
            //Debug.Log("Not Moving");
        }
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
        if (collision.gameObject.tag == "Tree")
        {
            LoseCondition();
        }
        if (collision.gameObject.tag == "Refuel")
        {
            CurrentFuel = 100.0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Refuel")
        {
            CurrentFuel = 100.0f;
        }
    }

    private void LoseCondition()
    {
        WinLoseText.text = "You Lose!";
        //Debug.Log("You Lose!");
    }

    private void LoseCondition(String LoseText)
    {
        WinLoseText.text = LoseText;
        //Debug.Log("You Lose!");
    }

    private void WinConditionCheck()
    {
        // If rescued all soldiers
        if (SoldiersRescued == soldierSpawning.numSoldiers)
        {
            WinLoseText.text = "You Win!";
            //Debug.Log("You Win!");
        }
    }
}
