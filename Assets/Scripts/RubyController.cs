// A Compiler is a tool that translates your code into a language that the computer can understand, because computers don’t read English!

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
// All GameObjects start with a Transform component that allows you to specify its position and rotation in the Scene
// Because all GameObjects have a Transform component by default, Unity makes the transform variable available in all scripts
// Transform is a variable too (created by Unity, not you), that contains the Transform component on the GameObject that the script is on
// the Transform values in the Inspector that use x for the horizontal position, y for the vertical position and z for the depth
// Think of the dot between transform and position as “contains”, so transform.position are retrieving the position that is “contained” inside the Transform

{
    public float speed = 3.0f; // “f”: This tells the computer that the number is a floating point number, so that it can store the number properly
                               // Since all the Unity built-in scripts use floating points, you need to use floating point by adding an “f” to your decimal number
    
    public int maxHealth = 5;

    public GameObject projectilePrefab; // make it a GameObject type because that’s what Prefabs are
                                        // GameObjects saved as Assets
                                        // it appears in your Editor as a slot where you can assign any GameObject
    
    public int health { get { return currentHealth; }} // In the first block, you used the get keyword, to get whatever is in the second block.
                                                       // only specified a “get” operation for health, so it’s read-only. 
                                                       // The set keyword and works exactly like get, but makes the property writable.
                                                       // The second block is just like a normal function, so you just return the currentHealth value.
                                                       // The compiler handles this exactly like a function, so you can write anything you want in a function inside that get block
                                                       // (such as declaring a variable, doing computations and calling other functions).
    int currentHealth; // if you make currentHealth public, you could change it to 10 in another script, even if the maxHealth is just 5.
                       // To avoid that, it is best to only allow changes through functions.
                       // This enables you to do checks — just like the ChangeHealth function,
                       // which uses a clamp to prevent health never going below 0 or above maxHealth.
                       // Properties are used like variables, but it only works like that when you READ it.
    
    public float timeInvincible = 2.0f; // she should only get damaged every two seconds when stay in the damage zone
    private bool isInvincible; // to store if you are currently invincible or not
    private float invincibleTimer; // store how much time Ruby has left being invincible before reverting to being hurtable
    
    private Rigidbody2D rigidbody2D; // store the Rigidbody
                                     // a Rigidbody component has to be manually added to a GameObject,
                                     // so Unity doesn’t have it as a built-in variable
    private float horizontal;
    private float vertical;
    
    Animator animator; // The component we want to interact with is the Animator
                       // we will use GetComponent to retrieve the Animator and store it in that variable
    Vector2 lookDirection = new Vector2(1,0); // compared to the Robot, Ruby can stand still
                                                    // When she stands still, Move X and Y are both 0,
                                                    // cuz the State Machine doesn’t know which direction to use unless we tell it.
                                                    // Make lookDirection store the direction that Ruby is looking so you can always provide a direction to the State Machine.
                                                    // Indeed, if you look at the Animator parameter, it expects a Look X and a Look Y parameter.
                                                    // You will send those Look parameters and the Speed from the Update function

    /*public UIHealthBar UiHealthBar;*/
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); // give you the Rigidbody2D that is attached to the same GameObject that your script is attached to, which is your character
        animator = GetComponent<Animator>(); // give you the Animator that is attached to the same GameObject that your script is attached to

        currentHealth = maxHealth; // Ruby starts the game with full health
                                   /* QualitySettings.vSyncCount = 0;
                                   // Application.targetFrameRate = 10;*/ // This will make Unity render 10 frames per second
                                   // cinema renders 24 frames per second
                                   // The illusion of movement will be a bit broken, because you don’t have enough frames rendered(10/24)
    }

    // Update is called once per frame, i.e. every time the game computes a new image (an uncertain rate)
    // It could be 20 images per second on a slow computer, or 3000 on a very fast one
    // To give the impression of movement, a game (just like a movie) is still images that are shown at high speed. Typically in games, 30 or 60 images show in one second. Each of those images is called a frame.
    // In this Update function, you will write anything you want to happen continuously in the game (for example, reading input from the player, moving GameObjects, or counting time passing).
    void Update()
    {
        // make the Ruby GameObject move
        horizontal = Input.GetAxis("Horizontal"); // be ready to tie the character's movement to keyboard input
                                                  // keyboard input through axes
                                                  // It stores the result that Input.GetAxis ("Horizontal") provides
                                                  // using the Unity Input System, which is composed of Input Settings and input code
                                                  // which Unity gives you to query the value of an axis for that frame
        vertical = Input.GetAxis("Vertical"); // The default axis corresponding to the key up and down is called Vertical

        /*Debug.Log(horizontal);*/ // Debug contains all functions that help debug your game
        
        Vector2 move = new Vector2(horizontal, vertical); // Instead of doing x and y independently for the movement, you store the input amount in a Vector2 called move
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) // Check to see whether move.x or move.y isn’t equal to 0
                                                                                                  // Use Mathf.Approximately instead of == because the way computers store float numbers means there is a tiny loss in precision
                                                                                                  // you should never test for perfect equality
                                                                                                  // because an operation that should end up giving 0.0f could instead give something like 0.0000000001f instead          
        {
            lookDirection.Set(move.x, move.y); // If either x or y isn’t equal to 0, then Ruby is moving, set your look direction to your Move Vector and Ruby should look in the direction that she is moving.
                                               // If she stops moving (Move x and y are 0) then that won’t happen and look will remain as the value it was just before she stopped moving.
                                               // equal to lookDirection = move
            lookDirection.Normalize(); // In general, you will normalize vectors that store direction because length is not important, only the direction is
        }
        
        animator.SetFloat("Look X", lookDirection.x); // send the direction you look in and the speed (the length of the move vector) to the Animator
                                                            // If Ruby doesn’t move, it will be 0, but if she does then it will be a positive number.
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime; // if Ruby is invincible, you remove deltaTime from your timer
            if (invincibleTimer < 0) // When that time is less than or equal to zero, the timer reaches its end and Ruby’s invincibility is finished,
                isInvincible = false; // so you remove her invincibility by resetting the bool to false
        }
        
        if(Input.GetKeyDown(KeyCode.C)) // when the player presses a key, and call Launch when they do
        {
            Launch(); // launch a Cog
        }
        
        if (Input.GetKeyDown(KeyCode.X)) // if the “talk” button is pressed, enter the if block and start your Raycast
        {
            // RaycastHit2D is a variable stores the result of a Raycast, which is given to us by Physics2D.Raycast
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            // The starting point is an upward offset from Ruby’s position because you want to test from the centre of Ruby’s Sprite, not from her feet.
            // The direction, which is the direction that Ruby is looking
            // A layer mask which allows us to test only certain layers. Any layers that are not part of the mask will be ignored during the intersection test.
            if (hit.collider != null) // If the Raycast didn’t intersect anything, this will be null so do nothing
                                      // Otherwise, RaycastHit2D will contain the Collider the Raycast intersected 
            {
                /*Debug.Log("Raycast has hit the object " + hit.collider.gameObject);*/ // log the object you have just found with the Raycast
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>(); // trying to find a NonPlayerCharacter script on the object the Raycast hit
                if (character != null) // if that script exists on that object, you will display the dialog
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    // For the physics computation to be stable, it needs to update at regular intervals (for example, every 16ms)
    // shouldn’t read input in the Fixedupdate function
    private void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position; // use the Rigidbody itself instead of GameObject Transform (to avoid Ruby’s jittering)
                                                 // storing the current position contained in the Rigidbody component inside a variable of type Vector2
                                                 // Think of Variables as boxes stored on a shelf with names written on the front, and inside are values
                                                 // Types tell the computer what kind of data you want to store, so it can get the right amount of space in memory
                                                 // Now there is a place in memory with the name “position” on it, which contains a copy of your GameObject’s current position
                                                 // A Vector2 is a data type that stores two numbers, it “contains” x and y
        position.x = position.x + speed * horizontal * Time.deltaTime;
        // it moves Ruby %speed(=0.1) units every time Unity calls the Update function, which means every FRAME
        // If your game runs at 60 frames per second, Ruby will move at 0.1 * 60, so six units per second
        // But if the game runs at ten frames per second, Ruby only moves 0.1 * 10, so 1 unit per second!
        // If a player has a very old computer that can only run the game at 30 frames per second
        // and another player has a computer that can run it at 120 frames per second
        // those two players will have a main character that moves at radically different speeds
        // This will make the game either harder or easier to play, DEPENDING on the machine running the game

        // kept the amount of movement to 0.1 * Time.deltaTime, so that means your character will move 0.1 units per SECOND.
        // It will take ten seconds to cover a single unit. That’s way too slow. A running character should cover three or four units per second

        // To fix this problem, you need to express the amount that Ruby moves not in units per frames, but in units per second
        // To do this, you will change the movement speed by multiplying it with the time it takes for Unity to render a frame
        // deltaTime, contained inside Time, is a variable that Unity fills with the time it takes for a frame to be rendered
        // If the game runs at ten frames per second, each frame will take 0.1 seconds.
        // If it runs at 60 frames per second, each frame will take 0.017 seconds.
        // If you multiply the movement by that value, then the movement becomes expressed in seconds.
        // Your character now runs at the same speed on any machine, regardless of how many frames per second are used to render the game
        // It will just look choppier on slower machines. It’s now “frame INdependent”.
        // i.e. the character covers the same number of units per second as when the game ran faster


        // it should only add it if the player presses the right arrow key on their keyboard
        // If you press left, the axis will be -1 (which makes the movement  position.x + -0.1f;). Ruby will move -0.1 to the left
        // If you press right, the axis will be 1. Ruby will move 0.1 to the right
        // If you press no key, the axis will be 0 and Ruby will not move (0.1 * 0 is 0)
        position.y = position.y + speed * vertical * Time.deltaTime; // The y value stored inside a Vector2 corresponds to the vertical position
        
        rigidbody2D.MovePosition(position); // MovePosition (Vector2 newPosition)
                                            // let the Physics System synchronize the GameObject position to the Rigidbody position
                                            // Before this line of code, you were only modifying a copy, like doing math on a side sheet of paper
    }

    public void ChangeHealth(int amount) // function declaration (tells the compiler how to compile that function)    
                                         // parameter is the amount of change to the Health
    {
        if (amount < 0) // add a check to see if you are currently hurting the character
        {
            animator.SetTrigger("Hit"); // triggering the hit animation

            if (isInvincible) // first check if Ruby is invincible already
                return;
            
            isInvincible = true; // make Ruby invincible since she is just getting hurt by setting your isInvincible bool to true
            invincibleTimer = timeInvincible; // setting the invincibleTimer variable to timeInvincible
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth); // Clamping ensures that the first parameter never goes lower than the second parameter
                                                                          // and never goes above the third parameter
                                                                          /*Debug.Log(currentHealth + "/" + maxHealth);*/ // use + to merge strings together
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth); // give the ratio of currentHealth over maxHealth to UIHealthBar SetValue function
    }
    
    void Launch() // will be called when you want to launch a Projectile (such as when a keyboard key is pressed)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        // Instantiate takes an object as the first parameter and creates a copy at the position in the second parameter, with the rotation in the third parameter.
        // The object you will copy is your Prefab and you will place it at the position of your Rigidbody
        // (but offset it a bit upward so the object is near Ruby’s hands not her feet)
        // with a rotation of Quaternion.identity
        // Quaternions are mathematical operators that can express rotation
        // but all you need to remember here is that Quaternion.identity means “no rotation”

        Projectile projectile = projectileObject.GetComponent<Projectile>(); // get the Projectile script from that new object
        projectile.Launch(lookDirection, 300); // call the Launch function with the direction being where your character is looking and the force value field set at 300

        animator.SetTrigger("Launch"); // a trigger has been set for your Animator
                                             // This will make your Animator play the launching animation   
    }
}