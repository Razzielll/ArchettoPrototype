
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] DropItem[] dropItems;
    [SerializeField] float dropChance = 0.5f;
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnDie +=DropLoot;
    }
    public void DropLoot()
    {
        float chance = Random.Range(0, 1f);
        if (dropChance <= chance)
        {
            return;
        }
        if (dropItems.Length == 0)
        {
            return;
        }
        int randNumber = Random.Range(0, dropItems.Length);
        Instantiate(dropItems[randNumber], transform.position + Vector3.up, Quaternion.identity);
    }
}
