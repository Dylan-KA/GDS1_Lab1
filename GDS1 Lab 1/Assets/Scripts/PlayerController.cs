using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    float movementSpeed = 8;
    int MaxCarryingCapacity = 3;
    int SoldiersCarrying = 0;
    int SoldiersRescued = 0;
    float CurrentFuel = 100;
    float FuelConsumptionRate = 8;
    int HighScore = 0;
    bool IsFacingRight = true;
    bool IsGameOver = false;
    bool GameWon = false;
    [SerializeField] TextMeshProUGUI CarryingText;
    [SerializeField] TextMeshProUGUI RescuedText;
    [SerializeField] TextMeshProUGUI WinLoseText;
    [SerializeField] TextMeshProUGUI HighScoreText;
    [SerializeField] TextMeshProUGUI FuelText;
    [SerializeField] GameObject BackgroundMid;
    [SerializeField] GameObject BackgroundBonus;
    [SerializeField] TextMeshProUGUI BonusText;
    [SerializeField] SoldierSpawning soldierSpawning;
    [SerializeField] AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameSceneBonus")
        {
            Invoke("BonusLevelIntro", 3.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResetGameCheck();
        if (!IsGameOver)
        {
            PlayerMovement();
        }
        if (GameWon && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadSceneAsync(1);
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
            //Reset variables & UI
            SoldiersCarrying = 0;
            SoldiersRescued = 0;
            CurrentFuel = 100;
            CarryingText.text = "Soldiers In Helicopter: " + SoldiersCarrying;
            RescuedText.text = "Soldiers Rescued: " + SoldiersRescued;
            FuelText.text = "Fuel: 100.0";
            WinLoseText.text = "";
            IsGameOver = false;
            BackgroundMid.SetActive(false);

            //Reset Player Position
            transform.position = new Vector3(-5.0f, 0.0f, -1.0f);

            //Remove existing Soldiers and Spawn new Soldiers
            soldierSpawning.SpawnSoldiers();

            //Restart BGM
            audioManager.PlayBGM();

            return true;
        }
        return false;
    }

    void PlayerMovement()
    {
        if (CurrentFuel <= 0)
        {
            LoseCondition("Out of Fuel Game Over!");
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

        //Fuel Consumption
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
            audioManager.PlayCrash();
            LoseCondition("You Crashed! Game Over!");
        }
        if (collision.gameObject.tag == "Refuel")
        {
            if (CurrentFuel < 100.0f)
            {
                audioManager.PlayRefuel();
            }
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
        IsGameOver = true;
        WinLoseText.text = "Game Over!";
        BackgroundMid.SetActive(true);
        if (SceneManager.GetActiveScene().name == "GameSceneBonus")
        {
            if (SoldiersRescued > HighScore)
            {
                HighScore = SoldiersRescued;
                HighScoreText.text = "HighScore: " + HighScore;
            }
        }
        //Debug.Log("You Lose!");
    }

    private void LoseCondition(String LoseText)
    {
        IsGameOver = true;
        WinLoseText.text = LoseText;
        BackgroundMid.SetActive(true);
        if (SceneManager.GetActiveScene().name == "GameSceneBonus")
        {
            if (SoldiersRescued > HighScore)
            {
                HighScore = SoldiersRescued;
                HighScoreText.text = "HighScore: " + HighScore;
            }
        }
        //Debug.Log("You Lose!");
    }

    private void WinConditionCheck()
    {
        // If rescued all soldiers
        if (SoldiersRescued == soldierSpawning.numSoldiers)
        {
            if (SceneManager.GetActiveScene().name != "GameSceneBonus")
            {
                WinLoseText.text = "You Win!";
                BackgroundMid.SetActive(true);
                IsGameOver = true;
                GameWon = true;
                BackgroundBonus.SetActive(true);
                BonusText.text = "Press 'Enter' for Bonus Level";
                //Debug.Log("You Win!");
            } else
            {
                //Debug.Log("In Bonus Scene");
                soldierSpawning.SpawnSoldiers();
            }

        }
    }

    private void BonusLevelIntro()
    {
        BackgroundMid.SetActive(false);
        WinLoseText.text = "";
    }

}
