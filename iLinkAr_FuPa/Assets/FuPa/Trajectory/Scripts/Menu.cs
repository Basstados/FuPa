using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject goal;
	public GameObject wall;
	public GameObject buttonGoal;
	public GameObject buttonGoalA;
	public GameObject buttonwall;
	public GameObject buttonwallA;

	
	
	public void OnGoalClick() 
	{
		wall.SetActive(false);
		goal.SetActive(true);
		buttonGoalA.SetActive(true);
		buttonGoal.SetActive(false);
		buttonwallA.SetActive(false);
		buttonwall.SetActive(true);
	}

	public void OnWallClick() 
	{
		wall.SetActive(true);
		goal.SetActive(false);
		buttonwallA.SetActive(true);
		buttonwall.SetActive(false);
		buttonGoalA.SetActive(false);
		buttonGoal.SetActive(true);
	}
}
