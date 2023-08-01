
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] DropItem dropItem;
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnDie +=DropLoot;
    }
    public void DropLoot(Health health)
    {
        
        Instantiate(dropItem, transform.position + Vector3.up, Quaternion.identity);
    }
}
