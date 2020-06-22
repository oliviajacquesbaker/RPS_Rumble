using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public enum GameState
{
    MENU,
    PAUSE,
    PLAY,
    GAMEOVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController[] players { get; private set; }
    private GameState currentState;
    [SerializeField]
    private string menuScene;
    [SerializeField]
    private string playScene;
    [SerializeField]
    private string endScene;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        players = FindObjectsOfType<PlayerController>();
    }

    bool checkGameOver()
    {
        for(int i=0; i<players.Length; i++)
        {
            if (players[i].health <= 0)
            {
                return true;
            }
        }
        return false;
    }

    void disablePlayers()
    {
        for(int i=0; i < players.Length; i++)
        {
            players[i].enabled = false;
        }
    }
    
    void enablePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].enabled = true;
        }
    }

    void pauseGame()
    {
        disablePlayers();
        currentState = GameState.PAUSE;
    }

    void unpauseGame()
    {
        enablePlayers();
        currentState = GameState.PLAY;
    }

     void endGame()
    {
        disablePlayers();
        SceneManager.LoadScene(endScene);
        currentState = GameState.GAMEOVER;
    }

    bool checkForPause()
    {
        //idk how we're doing this,, by pressing a key? by clicking a button?
        return false;
    }

    bool checkForStart()
    {
        //same here, not sure how to check until the menu is actually set up
        return false;
    }

    void startGame()
    {
        SceneManager.LoadScene(playScene);
        enablePlayers();
        currentState = GameState.PLAY;
    }

    void Update()
    {
        if(currentState == GameState.PLAY)
        {
            if (checkGameOver())
            {
                endGame();
            }else if (checkForPause())
            {
                pauseGame();
            }
        }
        else if (currentState == GameState.PAUSE)
        {
            if (checkForPause())
            {
                unpauseGame();
            }
        }
        else if (currentState == GameState.MENU)
        {
            if (checkForStart())
            {
                startGame();
            }
        }
        else if (currentState == GameState.GAMEOVER)
        {
            //smthn abt,,, if click rematch,
            //startGame();
            //if click menus,
            //SceneManager.LoadScene(menuScene);
        }
    }

}
