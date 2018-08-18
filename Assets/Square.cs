using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
	[HideInInspector]
	public string number;

	// Use this for initialization
	void Start ()
	{
		number = "";

	}
	
	public void ChangeNumber(int newNumber)
	{
		number = newNumber.ToString();
		Display();
	}

	private void Display()
	{
		Text t=transform.GetComponentInChildren<Text>();
		t.text = number;
	}
}
