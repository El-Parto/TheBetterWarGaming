using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColourChangerUI : MonoBehaviour
{

	public Image previewTankColour; // colour of tank preview to set. Should be the same as the image variable for the player
	
	public static Color PlayerColour { get; private set; }
	[SerializeField] private Slider redColourSlider; 
	[SerializeField] private Slider greenColourSlider; 
	[SerializeField] private Slider blueColourSlider;
	
    
    // Start is called before the first frame update
    //void Start() => LoadPlayerColour();

    // uses the Sliders to get RGB  values and sets them to the image. And because iused the image, i used the images colour to set the colour to the tank colour.
    // We may have to grey scale the hp bar and the tank texture to get this to work properly.
    public void ChangePlayerColour()
    {
	    previewTankColour.color = new Color(redColourSlider.value, greenColourSlider.value, blueColourSlider.value);
	    
	    PlayerColour = new Color(previewTankColour.color.r, previewTankColour.color.g, previewTankColour.color.b);  
    }

    
    // It's in update because it's being visualised as the player changes the values
    void Update()
    {
		ChangePlayerColour();        
    }
}
