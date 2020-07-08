using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // This function is called every frame the Rigidbody is inside the Trigger instead of just once when it enters.
    
    // To optimize resources, the Physics System stops computing collision for a Rigidbody when it stops moving; the Rigidbody “sleeps”.
    // But in your case, you want the computation to happen all the time, because you need to detect whether Ruby is hurt even when she stops moving,
    // so you will instruct the Rigidbody to never sleep.
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController >();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
