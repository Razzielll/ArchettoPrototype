
using UnityEngine;

public class EnemyMelee : EnemyAI
{
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float damageValue = 3f;
    
   
    private float attackTimer = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackTimer = 0f; 
       
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        attackTimer += Time.deltaTime;
        
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
