using UnityEngine;

public class BallBullet : Bullet
{
    // Reference to the rigidbody component of the bullet
    private Rigidbody rigidbody;

    // Force applied to the bullet when shooting
    [SerializeField] float forceValue = 20f;

    // Gravity value applied to the bullet
    [SerializeField] float gravityValue = 9f;

    // Maximum spread of the bullet's aim to simulate inaccuracy
    [SerializeField] float aimSpreadMax = 0.5f;

    // Called before the first frame update
    void Start()
    {
        // Get the rigidbody component of the bullet
        rigidbody = GetComponent<Rigidbody>();

        // Shoot the bullet
        Shoot();
    }

    // Method to apply the shooting force to the bullet
    private void Shoot()
    {
        // Generate a random number for aim spread
        float randomNumber = Random.Range(0, aimSpreadMax);

        // Calculate the aim spread vector based on the bullet's forward direction and the random number
        Vector3 aimSpread = transform.forward + new Vector3(0f, randomNumber, 0f);

        // Apply the shooting force to the bullet
        rigidbody.AddForce(aimSpread * forceValue, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Apply gravity force to the bullet (in the negative Y direction)
        rigidbody.AddForce(new Vector3(0, -gravityValue, 0));
    }

    // Override the OnTriggerEnter method to handle collision with other objects
    public override void OnTriggerEnter(Collider other)
    {
        // If the collision is with an enemy or any object with Health component, apply damage
        if (other.GetComponent<EnemyAI>() != null) 
        {
            return;
        }
        if( other.GetComponent<Bullet>() != null)
        {
            return;
        }


        other.transform.GetComponent<Health>()?.TakeDamage(this.gameObject, damage);
        // Destroy the bullet after the collision
        Destroy(this.gameObject);
    }

    // Override the OnCollisionEnter method to handle collision with other physics objects
    public override void OnCollisionEnter(Collision collision)
    {
        // If the collision is with an enemy or any object with Health component, return early
        if (collision.transform.GetComponent<EnemyAI>() != null || collision.transform.GetComponent<Health>() != null)
        {
            return;
        }

        // If the collision is with another bullet or with itself, return early
        if (collision.transform == this.gameObject || collision.transform.GetComponent<Bullet>() != null)
        {
            return;
        }

        // Destroy the bullet after the collision with a non-enemy/non-bullet object
        Destroy(this.gameObject);
    }
}
