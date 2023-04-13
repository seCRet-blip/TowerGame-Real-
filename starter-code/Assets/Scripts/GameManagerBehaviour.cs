using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManagerBehaviour : MonoBehaviour {

	public Text goldLabel;
	// Use this for initialization
	void Start () {
		Gold = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private int gold;
	public int Gold {

		get { return gold; }
		set
		{
			gold = value;
			goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
		}

	}
}
