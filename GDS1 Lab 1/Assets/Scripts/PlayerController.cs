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
    float FuelConsumptionRate = 8;
    bool IsFacingRight = true;
    [SerializeField] TextMeshProUGUI CarryingText;
    [SerializeField] TextMeshProUGUI RescuedText;
    [SerializeField] TextMeshProUGUI WinLoseText;
    [SerializeField] TextMeshProUGUI FuelText;
    [SerializeField] SoldierSpawning soldierSpawning;
    [SerializeField] AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        
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
            CurrentFuel -= Time.deltaTime * FuelConsumptionRate;
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
            transform.position = new Vector3(-5.0f, 0.0f, -1.0f);

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
        
        //Movement
        Vector2 newDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(newDirection * movementSpeed * Time.deltaTime);

        //Rotation
        if (newDirection.x > 0 && !IsFacingRight)
        {
            //Debug.Log("Flipping Horizontal to face right");
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            IsFacingRight = true;
        }
        if ((newDirection.x < 0) && IsFacingRight)
        {
            //Debug.Log("Flipping Horizontal to face left");
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            IsFacingRight = false;
        }

        if (newDirection != Vector2.zero)
        {
            FuelConsumption();
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        } else
        {
            //Debug.Log("Not Moving");
            if (IsFacingRight)
            {
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, 18.0f);
            } else
            {
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, -18.0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision occurred");
        if (collision.gameObject.tag == "Soldier" && (SoldiersCarrying < MaxCarryingCapacity))
        {
            audioManager.PlaySoldierPickup();
            Destroy(collision.gameObject);
            SoldiersCarrying += 1;
            CarryingText.text = "Soldiers In Helicopter: " + SoldiersCarrying;
            //Debug.Log("Soldier Picked up");
        }
        if (collision.gameObject.tag == "Hospital")
        {
            if (SoldiersCarrying != 0)
            {
                audioManager.PlaySoldierDropOff();
            }
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
