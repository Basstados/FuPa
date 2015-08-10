using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// SIMPLE FOOTBALL SWIPE KIT - ACCURATE SWIPE FOR FLICK GAMES
	/// 
	/// SAAD KHAWAJA - AUTHOR
	/// 
	/// IN CASE OF ANY PROBLEMS, FEEL FREE TO EMAIL ME:
	/// me@saadkhawaja.com
	/// 
	/// Follow on Twitter:
	/// http://www.twitter.com/saadskhawaja
	/// 
	/// 
	/// </summary>

	//DECLARING VARIABLES TO USE
	public GameObject ballInitial; //USED TO CREATE ANOTHER BALL AFTER ONE HAS BEEN SWIPED
	public GameObject ball; //THE CURRENT BALL THAT IS BEING USED
	public GameObject lineInitial; //THE LINE GAMEOBJECT THAT IS DUPLICATED AFTER EVERY SWIPE
	public GameObject lineTmp; //THE CURRENT LINE OBJECT TO USE
	public GameObject camera; //Ref to Camera so Objects can be placed as a child of camera

	/*public GUIStyle style; //STYLE FOR LABELS
	float hSliderValue = 25f; //THE SLIDER ON THE RIGHT - THE FORCE OF THE BALL (Z DIRECTION)
	float hWindValue = 0f; //THE SLIDER ON THE RIGHT - THE FORCE OF THE WIND*/

	Vector3 mouseStart = Vector3.zero; //THE MOUSE POSITION WHEN IT IS FIRST PRESSED (MOUSEDOWN)
	Vector3 mouseEnd = Vector3.zero; //THE MOUSE POSITION WHEN IT IS RELEASED (MOUSEUP)

	
	// Use this for initialization
	void Start () {
		
		Physics.IgnoreLayerCollision(8,8); //IGNORE BALLS COLLIDING WITH OTHER BALLS
		createAnotherBall(); //CREATE A BALL TO START WITH
	}


	// Update is called once per frame
	void Update () {
	
		Vector3 currentPos = Input.mousePosition; //THE CURRENT POSITION OF THE MOUSE
		currentPos.z = 5f; //ADDING DEPTH TO THE POSITION

		if(Input.GetMouseButtonDown(0)) //IF MOUSE IS PRESSED FOR THE FIRST TIME
		 {
			mouseStart = Input.mousePosition; //SAVE THE MOUSEPOSITION IN MOUSESTART VECTOR
			lineTmp = GameObject.Instantiate(lineInitial,currentPos,Quaternion.identity) as GameObject; //CREATE A NEW LINE (TRACE)
		}


		//SWIPE CODE
		if(Input.GetMouseButtonUp(0)) //IF MOUSE IS RELEASED (SWIPE COMPLETE)
		{
			mouseEnd = Input.mousePosition; //SAVE MOUSE END POSITION

			if(Vector3.Distance(mouseStart,mouseEnd) > 20) //CHECK IF THE MOUSE WAS SWIPED A DISTANCE - NOT A CLICK
			{
				Destroy(lineTmp); //DESTROY THE PREVIOUS LINE GAME OBJECT (TRACE LINE)
				ball.transform.rotation=Quaternion.Euler(new Vector3(-GetLength(),-(GetAngle()-90),0)); //ROTATE THE BALL TO FACE THE DIRECTION OF THE SWIPE - LENGTH = Y, ANGLE = X
				ball.rigidbody.AddRelativeForce(Vector3.forward*25f/**hSliderValue*/,ForceMode.Impulse); //NOW THE BALL IS FACING THE DIRECTION OF SWIPE, ADD FORWARD FORCE SIMPLY
				ball.constantForce.enabled = true;//ENABLE WIND
				//ball.GetComponent<DestroyBall>().enabled = true;
				ball.AddComponent("DestroyBall");
				ball.rigidbody.useGravity = true;
				ball.collider.enabled = true; 

				createAnotherBall(); //CREATE ANOTHER BALL AS THIS ONE HAS BEEN DISPATCHED
				//updateGUI(); //UPDATE GUI WITH SWIPE VALUES
			}

		}

		if(Input.GetMouseButton(0)) //WHILE MOUSE IS DOWN (MOVE THE TRACE LINE)
		{
			if(lineTmp!=null)
			{
				lineTmp.transform.position = Camera.main.ScreenToWorldPoint(currentPos); //MOVE THE TRACE LINE TO CURRENTPOS
			}
		}
	}


	void createAnotherBall() //CREATE A REPLICA OF THE INITIAL BALL
	{
		ball = Instantiate(ballInitial,ballInitial.transform.position,Quaternion.identity) as GameObject; //CREATE A BALL
		ball.SetActive(true);
		//ball.transform.parent = camera.transform.parent;
		ball.constantForce.force = new Vector3(/*hWindValue*/0f,0f,0f);
	}

	float GetAngle () {
		//GET ANGLE BETWEEN TWO VECTORS OF THE SWIPE GESTURE
		Vector3 v2 = mouseEnd - mouseStart;
		float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg;
		return angle;
	}
	
	float GetLength () {

		//GET LENGTH OF THE MOUSESTART AND MOUSEEND POSITIONS
		Vector3 v2 = mouseEnd - mouseStart;
		return v2.magnitude/10f;
	}

	string GetDirection () {

		//GET DIRECTION OF SWIPE
		bool isUp = false;
		bool isDown = false;
		bool isRight = false;
		bool isLeft= false;

		float angle = GetAngle();

		if(angle>=45&&angle<=135)
			isUp=true;

		if(angle>=0&&angle<=80 || angle>=-80&&angle<=0)
			isRight=true;

		if(angle>=100&&angle<=180 || angle>=-180&&angle<=-100)
			isLeft=true;


		if(angle>=-180&&angle<=-0)
			isDown=true;


		string direction = "";

		if(isUp)
			direction+="UP ";
		if(isDown)
			direction+="DOWN ";
		if(isRight)
			direction+="RIGHT ";
		if(isLeft)
			direction+="LEFT ";


		
		return direction;

		//if(angle<45) direction
	}




	/*float angleGUI;
	string directionGUI;
	float lengthGUI;

	void updateGUI() //UPDATE GUI WITH THE LAST SWIPE VALUES
	{
		angleGUI = GetAngle();
		directionGUI = GetDirection();
		lengthGUI = GetLength();
	}

	void OnGUI()
	{

		if(true)
		{
		//DRAW ON GUI

		GUI.Label(new Rect(10f,10f,300f,100f),"This is a simple swipe scene. You can change the action of different axis in the code easily" +
		          "All the code is commented and easy to modify - the swipe functionality works as follows: angle = x axis, length = elevation/height of the ball" +
		          ",the power is constant. Try hitting the boxes!"
		          );
		GUI.Label(new Rect(10,120f,400,80),"Swipe Angle: " + angleGUI);
		GUI.Label(new Rect(10f,140f,400,80),"Swipe Direction: " + directionGUI);
		GUI.Label(new Rect(10f,160f,400,80),"Swipe Length: " + lengthGUI);

		//GUI.HorizontalSlider(new Rect(0,10,150,30),0.1f,1,0.1f,1,style,style,true,1);

		GUI.Label(new Rect(Screen.width-110,10,100,30),"Speed/Force");
		hSliderValue = GUI.HorizontalSlider(new Rect(Screen.width-110,40,100,30),hSliderValue,25,50);

		GUI.Label(new Rect(Screen.width-110,80,100,30),"Wind");
		hWindValue = GUI.HorizontalSlider(new Rect(Screen.width-110,110,100,30),hWindValue,-25,25);
		}
	}*/

}
