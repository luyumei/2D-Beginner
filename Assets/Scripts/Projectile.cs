using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By giving a force to a Rigidbody, the Physics System moves the RigidBody for you based on the force, and you don’t have to change its position in the Update function manually.
public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    
    void Awake() // Unity doesn’t run Start when you create the object, but on the next frame
                 // we just Instantiate in RubyCtrler.cs, so don't call Start()
                 // Awake is called immediately when the object is created (when Instantiate is called)
                 // so Rigidbody2d is properly initialized before calling Launch
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); // store the Rigidbody
    }
    
    public void Launch(Vector2 direction, float force) // parameters to move the Rigidbody
    {
        rigidbody2d.AddForce(direction * force); // When that force is added, the Physics Engine will move the Projectile every frame based on that force and direction.
    }
    
    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 1000.0f) // avoid that Cog will keep on going outside of the screen for as long as the game runs
                                                   // position can be seen as a vector from the center of our world to where your object is
                                                   // magnitude is the length of that vector
                                                   // So the magnitude of the position is the distance to the center 
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other) // want to detect collision
    {
        EnemyController e = other.collider.GetComponent<EnemyController>(); // Get an EnemyController from the object the Projectile collided with
        if (e != null) // if the object gets one, it means you have fixed that enemy
        {
            e.Fix();
        }
        
        /*Debug.Log("Projectile Collision with " + other.gameObject);*/ //we also add a debug log to know what the projectile touch
        Destroy(gameObject);
    }
}
