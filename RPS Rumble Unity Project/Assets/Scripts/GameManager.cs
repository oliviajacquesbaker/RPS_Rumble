using System.Collections;
using System.Collections.Generic;
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
    private PlayerController[] players;
    private GameState currentState;
    void Awake()
    {
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

    void Update()
    {
        if(currentState == GameState.PLAY)
        {
            if (checkGameOver())
            {
                disablePlayers();
                currentState = GameState.GAMEOVER;
            }
        }
    }
}
