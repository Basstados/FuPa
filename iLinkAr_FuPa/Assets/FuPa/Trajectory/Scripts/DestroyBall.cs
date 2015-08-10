using UnityEngine;
using System.Collections;

public class DestroyBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(DestroyObj());
	}

	IEnumerator DestroyObj()
	{
		yield return new WaitForSeconds(5.0f);  //Wait 2 seconds
		Destroy(gameObject);
		
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
