using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartCounter : MonoBehaviour
{
    private float count = 4;

    [SerializeField] Image block;

    [SerializeField] TextMeshProUGUI counterText;

    public UnityEvent startEnable;

    private void Update()
    {
        count -= Time.deltaTime;

        counterText.text = ((int)count).ToString();

        if (count <= 1)
        {
            counterText.enabled = false;
            block.enabled = false;
            startEnable?.Invoke();
            enabled = false;
        }
    }
}

