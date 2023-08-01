using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Reference to the UI that displays the countdown before the game starts
    [SerializeField] CountDownUI countDownUI;

    // Duration of the countdown before the game starts
    [SerializeField] float countDownTime = 3f;

    // Reference to the enemy spawner script
    [SerializeField] private EnemySpawner enemySpawner;

    // Reference to the exit game object (to be activated when the player wins)
    [SerializeField] GameObject exitGO;

    // List to store the spawned enemy AI agents
    private List<EnemyAI> enemyAIList = new List<EnemyAI>();

    // Timer for the countdown
    private float countDownTimer = 0;

    // Subscribes to events and initializes the countdown timer
    private void OnEnable()
    {
        enemyAIList.Clear();
        enemySpawner.OnEnemySpawn += EnemySpawner_OnEnemySpawn;
    }

    // Called when the script starts
    void Start()
    {
        // Hide the exit game object and pause the game
        exitGO.SetActive(false);
        Time.timeScale = 0f;
        countDownTimer = countDownTime;
    }

    // Called when an enemy is spawned, adds the enemy to the list and subscribes to its OnDie event
    private void EnemySpawner_OnEnemySpawn(object sender, EnemySpawner.OnEnemySpawnEventArgs e)
    {
        enemyAIList.Add(e.enemyAI);
        if (e.enemyAI.TryGetComponent<Health>(out Health enemyHealth))
        {
            enemyHealth.OnDie += enemy_OnDie;
        }
    }

    // Called when an enemy dies, removes it from the list
    private void enemy_OnDie(Health health)
    {
        enemyAIList.Remove(health.GetComponent<EnemyAI>());
    }

    // Called every frame
    void Update()
    {
        // If the countdown timer is not yet finished, continue the countdown
        if (countDownTimer > -3f)
        {
            CountDown();
        }

        // If there are no enemies left, activate the win condition
        if (enemyAIList.Count == 0)
        {
            OnWinCondition();
        }
    }

    // Activates the win condition by showing the exit game object
    private void OnWinCondition()
    {
        exitGO.SetActive(true);
    }

    // Handles the countdown logic and updates the UI accordingly
    private void CountDown()
    {
        countDownTimer -= Time.unscaledDeltaTime;

        if (countDownTimer > 2f)
        {
            exitGO.SetActive(false);
            countDownUI.SetCountDownText("3");
        }
        else if (countDownTimer > 1f)
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
