using UnityEngine;
using System.Collections;

public class MoveGoalkeeper : MonoBehaviour {

	public GameObject goalkeeper;
	float translation;
	bool directionChange1;
	bool directionChange2;

	void Start() {
		goalkeeper = GameObject.Find("Goalkeeper");
		directionChange1 = true;
		directionChange2 = false;


	}

	void Update() {
		Vector3 goalKeeperPosition = goalkeeper.transform.position;
		//print (goalKeeperPosition);
		//goalkeeper.transform.Translate(translation, 0, 0);
		//print (position);
		if (directionChange1) {
			translation = Time.deltaTime * 9;
			goalkeeper.transform.Translate(translation, 0, 0);
			//directionChange2 = false;
		}

		if (goalKeeperPosition.x >= -17) {
			directionChange2 = true;
			directionChange1 = false;
		}

		if (goalKeeperPosition.x <= -25.5) {
			directionChange2 = false;
			directionChange1 = true;
		}

		if (directionChange2) {
			translation = Time.deltaTime * -9;
			goalkeeper.transform.Translate (translation, 0, 0);
		}
	
	}
}