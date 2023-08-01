
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroyEffect = 2f;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] protected float damage = 5f;

    

    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,5f);
    }

    

    // Update is called once per frame
    void Update()
    {
        MoveAction();
    }

    public virtual void MoveAction()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other == this)
        {
            return;
        }
        if(other.GetComponent<Bullet>() != null)
        {
            return;
        }

        other.transform.GetComponent<Health>()?.TakeDamage(this.gameObject, damage);


        Destroy(this.gameObject);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("DropItem"))
        {
            return;
        }
        if (collision.transform ==this)
        {
            return;
        }
        if (collision.transform.GetComponent<Bullet>() != null)
        {
            return;
        }
        
        Destroy(this.gameObject);
    }





}
