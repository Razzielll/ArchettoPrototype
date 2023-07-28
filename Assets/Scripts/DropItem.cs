
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float rotationSpeed = 10f;

    private void Start()
    {
        Instantiate(itemPrefab,transform);
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime* rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  other.GetComponent<Inventory>().AddCoin();

            Destroy(this.gameObject);
        }
    }
}
