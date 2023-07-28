
using UnityEngine;

public class BallBullet : Bullet
{
    private Rigidbody rigidbody;
    [SerializeField] float forceValue =20f;
    [SerializeField] float gravityValue =9f;
    [SerializeField] float aimSpreadMax = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Shoot();

    }

    private void Shoot()
    {
        float randomNumber = Random.Range(0, aimSpreadMax);
        Vector3 aimSpread = transform.forward + new Vector3 (0f, randomNumber,0f);
        rigidbody.AddForce(aimSpread * forceValue, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(new Vector3(0, -gravityValue, 0));
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyAI>() != null)
        {
            return;
        }
        if (other.GetComponent<Bullet>() != null)
        {
            return;
        }

        other.transform.GetComponent<Health>()?.TakeDamage(this.gameObject, damage);


        Destroy(this.gameObject);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<EnemyAI>() != null)
        {
            return;
        }
        if (collision.transform == this)
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

