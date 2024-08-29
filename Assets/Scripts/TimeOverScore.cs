using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeOverScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeOverText;

    public void isTimeOverScore()
    {
        timeOverText.text = (ScoreCounter.Instance.Score).ToString();
    }
}
