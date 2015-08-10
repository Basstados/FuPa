#pragma strict

// Trajectory Test Package v1.0
// http://unitycoder.com/blog/
// ** remember to donate to keep more examples coming faster :) **

public var floor: Transform;
public var source: Transform;
public var target: Transform;
public var targetLight: Transform;
public var cannonTube:Transform;
public var cannonTubeHolder:Transform;
public var angleInfo:GUIText;
public var dummyRotation:Transform;

public var grenadePrefab: Transform;
private var grenade: Transform;
private var shooting:boolean = false;
private var angle:float = 10;

private var minDistance:float=2;

function BallisticVel(newsource: Transform, newtarget: Vector3, angle: float): Vector3 
{
	var dir:Vector3 = newtarget - (newsource.position);  // get target direction
	
	//print (dir);
	
	var h:float = dir.y;  // get height difference
	dir.y = 0;  // retain only the horizontal direction
	var dist:float = dir.magnitude ;  // get horizontal distance
	
//	print (dist);
	
	var a:float = angle * Mathf.Deg2Rad;  // convert angle to radians
	
//	print (a);
	
	dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
	dist += h / Mathf.Tan(a);  // correct for small height differences
	
//	print (dist);
	dist = Mathf.Clamp(dist,3,999);
	//dist = Mathf.Abs(dist);
	
	// calculate the velocity magnitude
	var vel:float = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
	
	
	
	//print ("vel:"+(vel * dir.normalized));
	
	return vel * dir.normalized;
}


function Update()
{

	// mouse moved..
	var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	var hit : RaycastHit;
	if (!shooting)
	{
	
		if (Physics.Raycast (ray, hit, 1000)) 
		{
			Debug.DrawLine (ray.origin+Vector3(0,-0.5,0), hit.point, Color.red);
			
			// check min distance
			if (Vector3.Distance(source.position,hit.point)>minDistance)
			{
				
				PredictLine(hit.point);
				
				// move light there
				targetLight.position = hit.point+Vector3(0,3,0);
				
				// TODO: clamp min angle.. / dist
				dummyRotation.LookAt(hit.point);
				//cannonTube.rotation = Quaternion(cannonTube.rotation.x,dummyRotation.rotation.y,cannonTube.rotation.z,cannonTube.rotation.w);
				cannonTube.eulerAngles = Vector3(-angle, dummyRotation.eulerAngles.y, cannonTube.eulerAngles.z);
				
				cannonTubeHolder.eulerAngles = Vector3(-angle, dummyRotation.eulerAngles.y, cannonTube.eulerAngles.z);
				
				angleInfo.text = "Angle:" + angle;
			}
		}
	}

	var scrollWheel:float = Input.GetAxis ("Mouse ScrollWheel");
	
	if (scrollWheel)
	{
		angle = Mathf.Clamp(angle-scrollWheel*12,10,80);
		
		// rotate cannonTube
		//cannonTube.Rotate(Vector3.forward, angle, Space.World);
		 cannonTube.eulerAngles = Vector3(-angle, cannonTube.eulerAngles.y, cannonTube.eulerAngles.z);
	}

	

	if (Input.GetMouseButtonUp(0))
	{
	
		if (Physics.Raycast (ray, hit, 1000)) 
		{
//			Debug.DrawLine (ray.origin+Vector3(0,-0.5,0), hit.point, Color.red);
			//shooting = true;
			// move light there
//			targetLight.position = hit.point+Vector3(0,3,0);
		
			//if (grenade!=null) Destroy(grenade.gameObject);
			
			// check min distance
			if (Vector3.Distance(source.position,targetLight.position-Vector3(0,3,0))>minDistance)
			{
			
				cannonTube.audio.Play(); 
				grenade = Instantiate(grenadePrefab,source.position,Quaternion.identity);
				//grenade.rigidbody.velocity = BallisticVel(source,hit.point, angle); // pass the angle and the target transform
				var vel:Vector3 = BallisticVel(source,hit.point, angle); 
				grenade.rigidbody.velocity = vel; // pass the angle and the target transform
				

				 
				 //print (Physics.gravity.magnitude);
				 
				// print ("horRange:"+r);
//				 print ("flight time:"+t);
//				 print ("peak height:"+(h+floor.TransformPoint(source.position).y));
				 

				



				//var someScript : SmoothFollowCannonBall;
				//someScript = GetComponent (SmoothFollowCannonBall);
				//someScript.target = grenade;
			}
		}
	}
	
	/*
	if (Input.GetMouseButtonDown(1))
	{
		// reset cam
		if (grenade!=null) Destroy(grenade.gameObject);
		
		transform.position = new Vector3(0,5,-5);
		shooting = false;
	}
	*/
}

function vyt(v:float,a:float,t:float){return v*Mathf.Sin(a*Mathf.PI/180)-Physics.gravity.magnitude*t;}
//function yt(v,a,t){return v*Math.sin(a*Math.PI/180)*t-.5*9.8*t*t}
//function vxf(v,a){return v*Math.cos(a*Math.PI/180)}
//function trad(v,y){return Math.sqrt(v*v/(9.8*9.8)-2*y/9.8)}


// http://forum.unity3d.com/threads/12588-Projection-Trajectory-Arc
/*p0 - is the start point
p1 - is the end point
c0 - is the middle point ( controlling the arcness )
t - between 0 and 1 */

function PredictLine(hitp:Vector3)
{

	var vel:Vector3 = BallisticVel(source,hitp, angle); 

	// v0
	var v:float = vel.magnitude; // launch velocity
	var a:float = angle; // launch angle
	
	// results
	var r:float = 0.0; // horizontal range
	var t:float = 0.0; // flight time
	var h:float = 0.0;// peak height
	
	 var ag:float=a;
	 h=vyt(v,ag,0)*vyt(v,ag,0)/(2*Physics.gravity.magnitude);
	 t=2*vyt(v,ag,0)/Physics.gravity.magnitude;
	 r=v*v*Mathf.Sin(2*ag*Mathf.PI/180)/Physics.gravity.magnitude; //m/s

	
	var midpoint:Vector3 = (source.position+hitp)/2+Vector3(0,h+floor.TransformPoint(source.position).y,0);
	Plot(source.position,midpoint,hitp);
}


function Plot(p0:Vector3,c0:Vector3,p1:Vector3)
{

    var t : float;    
	var sections:float = 30; // !!! if this is INT, t is also INT!!!
	var lineRenderer : LineRenderer = GetComponent(LineRenderer);

    for (var i : int = 0 ; i < sections ; i++ ){

        t = i/(sections-1) ;

        lineRenderer.SetPosition (i ,GetQuadraticCoordinates(t,p0,c0,p1));
		//print (GetQuadraticCoordinates(t,p0,c0,p1));

    }

}


function GetQuadraticCoordinates(t:float,p0:Vector3,c0:Vector3,p1:Vector3) : Vector3 
{
	return Mathf.Pow(1-t,2)*p0 + 2*t*(1-t)*c0 + Mathf.Pow(t,2)*p1 ;
	//return Mathf.Pow(1-t,2)*start.position + 2*t*(1-t)*middle + Mathf.Pow(t,2)*end.position;
}