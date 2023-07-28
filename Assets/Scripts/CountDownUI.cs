
using TMPro;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    

    public void SetCountDownText(string text)
    {
        countdownText.text = text;
    }

    public void DisableCountDownUI()
    {
        countdownText.gameObject.SetActive(false);
    }
}
