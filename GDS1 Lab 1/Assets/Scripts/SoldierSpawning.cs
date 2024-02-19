using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawning : MonoBehaviour
{

    [SerializeField] GameObject Soldier;
    public int numSoldiers = 9;
    List<GameObject> SoldiersList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnSoldiers();
    }

    public void SpawnSoldiers()
    {
        //Clear any existing Soldiers first
        foreach (GameObject soldier in SoldiersList)
        {
            if (soldier != null)
            {
                Destroy(soldier);
            }
        }
        SoldiersList.Clear();

        for (int i = 0; i < numSoldiers; i++)
        {
            //Spawn soldier in random position
            Vector2 RandomPosition = new Vector2(Random.Range(-2.0f, 8.5f), Random.Range(-5.0f, 5.0f));
            GameObject newSoldier = Instantiate(Soldier);
            SoldiersList.Add(newSoldier);
            newSoldier.transform.position = RandomPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
