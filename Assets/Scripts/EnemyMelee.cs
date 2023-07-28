
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float damageValue = 3f;
    [SerializeField] Mover mover;
    [SerializeField] Transform playerTransform;
    private float attackTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0f; 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        mover.MoveTo(playerTransform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            if(attackTimer > attackDelay)
            {
                
                other.GetComponent<Health>()?.TakeDamage(this.gameObject, damageValue);
                attackTimer = 0f;
            }
        }
    }
}
