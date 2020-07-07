// A Compiler is a tool that translates your code into a language that the computer can understand, because computers don’t read English!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour // class是component
// All GameObjects start with a Transform component that allows you to specify its position and rotation in the Scene
{
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10; // This will make Unity render 10 frames per second
                                            // cinema renders 24 frames per second
                                            // The illusion of movement will be a bit broken, because you don’t have enough frames rendered(10/24)
    }

    // Update is called once per frame
    // To give the impression of movement, a game (just like a movie) is still images that are shown at high speed. Typically in games, 30 or 60 images show in one second. Each of those images is called a frame.
    // In this Update function, you will write anything you want to happen continuously in the game (for example, reading input from the player, moving GameObjects, or counting time passing).
    void Update()
    {
        // make the Ruby GameObject move
        float
            horizontal = Input.GetAxis("Horizontal"); // be ready to tie the character's movement to keyboard input
                                                      // keyboard input through axes
                                                      // It stores the result that Input.GetAxis ("Horizontal") provides
                                                      // using the Unity Input System, which is composed of Input Settings and input code
                                                      // which Unity gives you to query the value of an axis for that frame

        float
            vertical = Input.GetAxis(
                "Vertical"); // The default axis corresponding to the key up and down is called Vertical

        //Debug.Log(horizontal); // Debug contains all functions that help debug your game
        Vector2
            position = transform
                .position; // storing the current position contained in the Transform component inside a variable of type Vector2
        // Think of Variables as boxes stored on a shelf with names written on the front, and inside are values
        // Types tell the computer what kind of data you want to store, so it can get the right amount of space in memory
        // Now there is a place in memory with the name “position” on it, which contains a copy of your GameObject’s current position
        // A Vector2 is a data type that stores two numbers, it “contains” x and y
        
        // Transform is a variable too (created by Unity, not you), that contains the Transform component on the GameObject that the script is on
        // the Transform values in the Inspector that use x for the horizontal position, y for the vertical position and z for the depth
        // Think of the dot between transform and position as “contains”, so here you are retrieving the position that is “contained” inside the Transform
        position.x =
            position.x +
            3.0f * horizontal * Time.deltaTime; // “f”: This tells the computer that the number is a floating point number, so that it can store the number properly
        // Since all the Unity built-in scripts use floating points, you need to use floating point by adding an “f” to your decimal number
        
        // it moves Ruby 0.1 units every time Unity calls the Update function, which means every FRAME
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
        position.y =
            position.y + 3.0f * vertical * Time.deltaTime; // The y value stored inside a Vector2 corresponds to the vertical position
        transform.position =
            position; // Before this line of code, you were only modifying a copy, like doing math on a side sheet of paper
    }
}