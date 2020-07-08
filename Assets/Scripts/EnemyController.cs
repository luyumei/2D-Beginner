using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed; // if make the speed a static member (by adding the keyword static in front of it),
                        // then changing it on one enemy will change it on all enemies at once.
                        // This is because it accesses the same space in memory instead of each having their own space.
                        // It also allows us to access that variable using the class name instead of a reference
                        // (so Enemy.speed instead of myEnemy.speed, where myEnemy is a variable containing a reference to your enemy)
                        // Time.deltaTime is a static member of the Time class, so you don’t need to do:
                        // Time myTime = gameObject.GetComponent<Time>();
                        // myTime.deltaTime;
                        // static members can also be functions, such as Debug.Log
                        // Again, you don’t need to get a reference to a Debug object, you just call the function directly on a class name.
    public bool vertical;
    public float changeTime = 3.0f; // the time before you reverse the enemy’s direction

    Rigidbody2D rigidbody2D;
    float timer; // keep the current value of the timer
    int direction = 1; // enemy’s current direction
    
    Animator animator; // The component we want to interact with is the Animator
                       // we will retrieve it in the Start function with GetComponent and store it inside a class variable 
                       // The Controller stores the State Machine that defines how those animations relate to each other.
                       // And the Animator plays the animation that the Controller assigns to it and tells it to play.
                       // We can send data to the Controller through the Animator in our script to select the right animation based on gameplay

    private bool broken = true; // the robot starts broken

    public ParticleSystem smokeEffect; // if a public member is a Component or Script type (instead of GameObject),
                                       // when you assign a GameObject to it in the Inspector,
                                       // Unity stores the component of the type that is on the GameObject
                                       // This prevents you from having to do GetComponent in the script like you did before
                                       // It also stops you from assigning a GameObject that doesn’t have that component type on it to that setting
                                       // This also avoids creating a bug by mistake
                                       
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); // give you the Rigidbody2D that is attached to the same GameObject that your script is attached to, which is the enemy
        timer = changeTime; // initialize your timer to the time before you reverse the enemy’s direction
        animator = GetComponent<Animator>(); // give you the Animator that is attached to the same GameObject that your script is attached to
    }

    void Update() // do this infinitely
    {
        if(!broken) // add a test to see if the robot is not broken
                    // When fixed, your Robot will stop moving (exiting the update function early will stop the code that moves them to be executed)
        {
            return;
        }
        
        timer -= Time.deltaTime; // decrement your timer

        if (timer < 0) // if timer is less than 0 change the direction and reset the timer
                       // this is not related to physics, it doesn't have to be performed in FixedUpdate 
        {
            direction = -direction; // then reverse the direction
            timer = changeTime; // and reset the timer
        }
    }
    
    void FixedUpdate()
    {
        if(!broken) // add a test to see if the robot is not broken
                    // When fixed, your Robot will stop moving
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical) // see if vertical is true and, if it is, move the enemy along the y axis instead of the x axis in your world
        {
            position.y = position.y + Time.deltaTime * speed * direction;;
            
            animator.SetFloat("Move X", 0); // send Parameter values to the Animator Controller
                                                        // 2nd parameter: the amount of movement in a given direction
                                                        // when your Robot moves vertically, 0 is sent to the horizontal parameter,
            animator.SetFloat("Move Y", direction); // and the direction will define whether the Robot is moving upward or downward
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;;
            
            animator.SetFloat("Move X", direction); // send Parameter values to the Animator Controller
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }    
    
    void OnCollisionEnter2D(Collision2D other) // Unlike the damage zone, you can’t use a Trigger because you want the enemy Collider to be “solid” and actually collide with things.
                                               // is called when a Rigidbody collides with something
    {
        RubyController player = other.gameObject.GetComponent<RubyController >(); // the type of other here is Collision2D not Collider2D
                                                                                  // A Collision2D doesn’t have a GetComponent function 
                                                                                  // but it contains lots of data about the collision,
                                                                                  // like the GameObject with which the enemy collided
                                                                                  // So you call GetComponent on that GameObject

        if (player != null) // check whether the GameObject that the enemy collided with has got a RubyController script
                            // If it has, then you know it’s the main character, so you can damage it
        {
            player.ChangeHealth(-1);
        }
    }
    
    public void Fix() // fix the robot
    {
        broken = false;
        rigidbody2D.simulated = false; // removes the Rigidbody from the Physics System simulation
                                       // so it won’t be taken into account by the system for collision
                                       // and the fixed robot won’t stop the Projectile anymore or be able to hurt the main character
        animator.SetTrigger("Fixed");
        
        smokeEffect.Stop(); // Stop, on the other hand, simply stops the Particle System from creating particles, and the particles that already exist can finish their lifetime normally
    }
}

