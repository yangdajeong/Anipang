using TMPro;
using UnityEngine;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }

    public int _score;

    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return;

            _score = value;
            scoreText.text = _score.ToString();
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }
}
