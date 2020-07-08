using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the script to detect when Ruby collides with the collectible health GameObject and give her some Health
// i.e. the code to handle the collision registered by The Physics System, in order to react to the collision
public class HealthCollectible : MonoBehaviour
{
    // Unity calls this OnTriggerEnter2D function on the first frame when it detects ANY new Rigidbody entering the Trigger
    // The parameter called other will contain the Collider that just entered the Trigger
    // Enable the Is Trigger property checkbox under the Box Collider 2D component
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Object that entered the trigger : " + other); // Console: Ruby has entered the Trigger
        RubyController
            controller =
                other.GetComponent<RubyController>(); // Access the RubyController component on the GameObject of the Collider that enters the Trigger

        if (controller != null) // test whether the value stored in controller is not equal to null
            // Well, if you add ANOTHER GameObject that moves (such as an enemy), then entering the Trigger WILL call that function.
            // But then GetComponent won’t find a RubyController on that GameObject (because they are enemies, not the player character).
            // As GetComponent can’t give you a RubyController, it will return NULL and you can’t call the function on an empty variable.
            // Doing this test ensures that you are only giving health to Ruby, and not creating an error trying to do it on other objects.
        {
            if (controller.health < controller.maxHealth) // avoid if you grab a health collectible when Ruby’s health is full,
                                                                 // the script will still destroy the collectible
            {
                controller.ChangeHealth(
                    1); // changed the health of the RubyController by 1 with the function you wrote earlier
                Destroy(gameObject); // This is the GameObject that the script is attached to (the collectible health pack)
            }
        }
    }
}