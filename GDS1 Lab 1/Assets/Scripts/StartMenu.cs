using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNormal()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void StartEndless()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
