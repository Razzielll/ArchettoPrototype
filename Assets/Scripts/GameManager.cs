
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CountDownUI countDownUI;
    [SerializeField] float countDownTime = 3f;
    private float countDownTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        countDownTimer = countDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(countDownTimer > -3f)
        {
            CountDown();
        }
        
    }

    private void CountDown()
    {
        countDownTimer -= Time.unscaledDeltaTime;
        if(countDownTimer > 2f)
        {
            countDownUI.SetCountDownText("3");
        }
        else if(countDownTimer > 1f)
        {
            countDownUI.SetCountDownText("2");
        }
        else if (countDownTimer > 0f)
        {
            countDownUI.SetCountDownText("1");
        }
        else if (countDownTimer > -1f)
        {
            countDownUI.SetCountDownText("Fight");
            Time.timeScale = 1f;
        }
        else if (countDownTimer > -2f)
        {
            countDownUI.DisableCountDownUI();
        }
        
    }

    IEnumerator GameStartCountdown()
    {
        yield return null;
    }
}
