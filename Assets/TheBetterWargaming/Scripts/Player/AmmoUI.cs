using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Another script for the Network player script to reference.
/// This is done in order to avoid GetComponent issues when trying to find
/// the correct slider.
/// Note: If you're re- setting this up, make sure to delete the network identity on the ammo prefab so it doesn't cause an error on start host.
/// </summary>
public class AmmoUI : MonoBehaviour
{
	private Slider ammoSlider;
	private void Start()
	{
		ammoSlider = GetComponent<Slider>();
		ammoSlider.interactable = false;
	}

	void Update()
	{
		float alpha = ammoSlider.value / ammoSlider.value;
		Color cachedColour = ammoSlider.image.color;
		cachedColour.a = alpha;
	}
	

}
