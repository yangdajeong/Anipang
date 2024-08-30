using TMPro;
using UnityEngine;

public class BestScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestScore;

    private void Awake()
    {
        bestScore.text = (PlayerPrefs.GetInt("BestScore")).ToString();
    }
}
