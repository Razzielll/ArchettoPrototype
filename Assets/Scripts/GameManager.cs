
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CountDownUI countDownUI;
    [SerializeField] float countDownTime = 3f;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] GameObject exitGO;
    private List<EnemyAI> enemyAIList = new List<EnemyAI>();

    private float countDownTimer = 0;

    private void OnEnable()
    {
        enemyAIList.Clear();
        enemySpawner.OnEnemySpawn += EnemySpawner_OnEnemySpawn;
    }
    // Start is called before the first frame update
    void Start()
    {
        exitGO.SetActive(false);
        Time.timeScale = 0f;
        countDownTimer = countDownTime;
        
        
    }

    private void EnemySpawner_OnEnemySpawn(object sender, EnemySpawner.OnEnemySpawnEventArgs e)
    {
        enemyAIList.Add(e.enemyAI);
        if( e.enemyAI.TryGetComponent<Health>(out Health enemyHealth))
        {
            enemyHealth.OnDie += enemy_OnDie;
        }
        
    }

    private void enemy_OnDie(Health health)
    {
        enemyAIList.Remove(health.GetComponent<EnemyAI>());
    }

    // Update is called once per frame
    void Update()
    {
        if(countDownTimer > -3f)
        {
            CountDown();
            
        }
        if(enemyAIList.Count == 0)
        {
            OnWinCondition();
        }
    }

    private void OnWinCondition()
    {
        exitGO.SetActive(true);
    }

    private void CountDown()
    {
        countDownTimer -= Time.unscaledDeltaTime;
        if(countDownTimer > 2f)
        {
            exitGO.SetActive(false);
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

    


}
