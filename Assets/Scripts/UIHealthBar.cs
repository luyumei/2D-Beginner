using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; } // want to be able to access our UIHealthBar script from any other script without needing a reference
                                                             // now can write UIHealthBar.instance in any script and it will call this get property
                                                             // The set property is private because we don’t want people to be able to change it from outside that script.
                                                             // static members are shared across all instances of that scripts,
                                                             // so it is the same value in all the GameObjects with that script attached to them
    
    public Image mask;
    float originalSize;
    
    void Awake() // this is called as soon as the object is created, which is our case is when the game starts
    {
        instance = this; // which is a special C# keyword that means “the object that currently runs that function”
                         // i.e. when the game starts, Health bar script stores itself in the static member called “instance”
                         // if in any other script you call UIHealthBar.instance, then the value it will return to that script is the Health bar in our Scene
                         // now have a reference to the Health bar script in your Scene without having to assign it in the Inspector manually
    }
    
    void Start()
    {
        originalSize = mask.rectTransform.rect.width; // getting the size on screen with rect.width
    }

    public void SetValue(float value) // code will call SetValue when the Health changes to a value between 0 and 1
                                      // (1 full health, 0.5 half health and so on), and this will change the size of our mask 
    {				      
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value); // setting the size and anchor
    }
    
    /* need to call SetValue from RubyController script’s ChangeHealth function, to give the new health amount*/
}
