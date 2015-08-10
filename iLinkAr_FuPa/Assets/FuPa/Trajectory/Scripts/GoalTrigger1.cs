using UnityEngine;
using System.Collections;

public class GoalTrigger1 : MonoBehaviour {

	public GameObject goalTexture;
	public GameObject fireworks;
	public AudioClip cheers;
	public GameObject menuPanel;
		
	// Use this for initialization
	void Start () {
			
	}
		
	// Update is called once per frame
	void Update () {
			
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			goalTexture.SetActive(true);
			fireworks.SetActive(true);
			audio.PlayOneShot(cheers, 1.0F);
			StartCoroutine(DeactivateGoal());
			Score scoreScript = (Score) menuPanel.GetComponent(typeof(Score));
			scoreScript.IncreaseScore(1);
		
		 }
	}

	IEnumerator DeactivateGoal()
	{
		yield return new WaitForSeconds(1.0f);  //Wait 2 seconds
		goalTexture.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		fireworks.SetActive(false);

		
	}
		
}


