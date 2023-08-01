using UnityEngine;

public class DropItem : MonoBehaviour
{
    // The prefab of the item to be dropped
    [SerializeField] GameObject itemPrefab;

    // The speed at which the dropped item will rotate
    [SerializeField] float rotationSpeed = 10f;

    private void Start()
    {
        // Instantiate the itemPrefab as a child of the current game object (DropItem)
        Instantiate(itemPrefab, transform);
    }

    private void Update()
    {
        // Rotate the DropItem around its Y-axis (upwards) based on the rotationSpeed and deltaTime
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered the collision has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // If the collision is with the player, add a coin to the player's inventory
            other.GetComponent<Inventory>().AddCoin();

            // Disable the collider of the DropItem to prevent multiple pickups by the player
            GetComponent<Collider>().enabled = false;

            // Destroy the DropItem game object after the player picks it up
            Destroy(this.gameObject);
        }
    }
}
