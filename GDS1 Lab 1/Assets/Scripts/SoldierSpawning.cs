using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Vector2 RandomPosition = new Vector2(Random.Range(-3.0f, 9.0f), Random.Range(-4.0f, 3.0f));
            GameObject newSoldier = Instantiate(Soldier);
            SoldiersList.Add(newSoldier);
            newSoldier.transform.position = RandomPosition;
            if (i % 2 == 0)
            {
                newSoldier.transform.localScale = new Vector3(newSoldier.transform.localScale.x * -1, newSoldier.transform.localScale.y, newSoldier.transform.localScale.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
