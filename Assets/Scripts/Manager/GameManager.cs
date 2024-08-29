using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void BestScoreSet()
    {
        if (ScoreCounter.Instance.Score > PlayerPrefs.GetInt("BestScore"))
            PlayerPrefs.SetInt("BestScore", ScoreCounter.Instance.Score);
    }

    [ContextMenu("Clear")]
    public void PlayerPrefsClear()
    {
        PlayerPrefs.DeleteAll();
    }
}
