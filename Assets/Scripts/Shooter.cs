
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shotPerSecond =1f;

    PlayerController playerController;
    Health currentTarget;
    Fighter fighter;
    bool isAttacking = false;
    float timer = 0;

    private void Start()
    {
        fighter = GetComponent<Fighter>();
        playerController = GetComponent<PlayerController>();
        playerController.OnAttackPatternChange += UpdateAttackPattern;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        currentTarget = fighter.GetCurrentTarget();
        if(currentTarget == null)
        {
            return;
        }

        if (currentTarget.GetComponent<Health>().IsDead())
        {
            currentTarget = null;
            return;
        }

        if (isAttacking && timer > 1f/shotPerSecond)
        {
            ShotBullet();
            timer = 0;
        }
    }

    private void ShotBullet()
    {
        Transform bulletTransform = Instantiate<Transform>(bulletPrefab.transform, transform.position + Vector3.up, Quaternion.identity);
        Vector3 direction = currentTarget.transform.position - transform.position;
        bulletTransform.forward = new Vector3(direction.x, 0, direction.z);
    }

    void UpdateAttackPattern(bool onBase)
    {
        if (onBase)
        {
            StopShooting();
        }
        else
        {
            StartShooting();
        }
    }

    public void StopShooting()
    {
        isAttacking = false;
        currentTarget = null;
    }

    public void StartShooting()
    {
        isAttacking = true;
    }
}
