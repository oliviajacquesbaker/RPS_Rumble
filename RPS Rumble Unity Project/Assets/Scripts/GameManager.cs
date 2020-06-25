using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController[] players;
    private bool playing = false;

    public Countdown counter;
    public EndPannel endPannel;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        counter.Trigger();
        disablePlayers();
    }

    int checkWin()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].health <= 0)
            {
                return i == 1 ? 0 : 1;
            }
        }
        return -1;
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

     void endGame(int winner)
    {
        disablePlayers();
        endPannel.Show(winner);
    }

    void startGame()
    {
        enablePlayers();
        playing = true;
    }

    void Update()
    {
        if (playing)
        {
            int winner = checkWin();
            if (winner != -1)
            {
                endGame(winner);
            }
        }
        else if (counter.isTriggered && counter.isComplete)
        {
            startGame();
        }
    }
}
