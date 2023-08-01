using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }

}
