using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public List<EnemyBehaviour> allEnemies;
    public Scene scene;
    
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        
        if (instance == null)
        {
            instance = this;
        }
        
        scene = SceneManager.GetActiveScene();
    }

    void LateUpdate()
    {
        if (allEnemies.Count <= 0)
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    public void ChangeLevel()
    {
        
    }
}
