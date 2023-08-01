
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCountInventory;
    [SerializeField] Inventory inventory;

    int countCount;
    // Start is called before the first frame update
    private void OnEnable()
    {
    }

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.OnCoinCountChange += UpdateCountCountInventory;
        UpdateCountCountInventory();
    }

    void UpdateCountCountInventory()
    {
       coinCountInventory.text = "Coins in inventory: " + inventory.GetCoinCountInventory().ToString();
    }
}
