using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public float playerScore = 0;
	public UILabel scoreLabel;
	// Update is called once per frame
	void Update ()
	{
		scoreLabel.text = playerScore.ToString();
	}
	
	//Method to increase score though other scripts:
	//Like collectibles, etc.
	public void IncreaseScore(int amount)
	{
		playerScore += amount;
	}
	/*void OnDisable()
	{
		//Look to store somewhere else. Like a packet to persist with dont destroy on load.
		PlayerPrefs.SetInt ("Score", (int)(playerScore*100));
	}*/
	/*void OnGUI()
	{
		GUI.Label (new Rect (10, 10, 100, 30), "Score " + (int)(playerScore));
	}*/
}

