using TMPro;
using UnityEngine;
using UnityEngine.Events;

public sealed class TimerCounter : MonoBehaviour
{
    public float timer;

    [SerializeField] private TextMeshProUGUI timerText;

    public UnityEvent timeOver;
    private void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = ((int)timer).ToString();

        if (timer <= 0)
        {
            Time.timeScale = 0f;
            timeOver?.Invoke();
            GameManager.instance.BestScoreSet();
            enabled = false;
        }
    }

    [ContextMenu("Test")]
    private void Test()
    {
        timer = 0;
    }

    public void BestScoreSet()
    {
        GameManager.instance.BestScoreSet();
    }
}
