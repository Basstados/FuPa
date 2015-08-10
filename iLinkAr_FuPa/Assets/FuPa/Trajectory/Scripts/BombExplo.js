#pragma strict

// Trajectory Test Package v1.0
// http://unitycoder.com/blog/
// ** remember to donate to keep more examples coming faster :) **

var exploPrefab:Transform;
var smokePrefab:Transform;
var dirtPrefab:Transform;

private var radius = 7.0;
private var power = 5000.0;

//private var horRange:float=0.0;
private var peakheight:float=0.0;
private var flightTime:float=0.0;

private var startpos:Vector3;

function Start()
{
	flightTime = Time.realtimeSinceStartup;
	startpos=transform.position;
}

function OnCollisionEnter(collision : Collision)
{

	// get distance
	//print ("dist:"+(Vector3.Distance(startpos,transform.position)*Vector3.Distance(startpos,transform.position))*0.0001);

	Invoke("PlaySoundLater", (Vector3.Distance(startpos,transform.position)*Vector3.Distance(startpos,transform.position))*0.0001);

	Instantiate(exploPrefab, transform.position, Quaternion.identity);
	Instantiate(dirtPrefab, transform.position, Quaternion.identity);
	
	renderer.enabled = false;
	rigidbody.velocity = Vector3.zero;
	rigidbody.isKinematic = true;
	rigidbody.detectCollisions = false;
	
	Instantiate(smokePrefab, transform.position, Quaternion.identity);
	
    // Applies an explosion force to all nearby rigidbodies
    var explosionPos : Vector3 = transform.position;
	
	var layerMask = 1 << 8;
	layerMask = ~layerMask;
	
    var colliders : Collider[] = Physics.OverlapSphere (gameObject.transform.position, radius, layerMask);
    
    for (var hit : Collider in colliders) 
	{
        if (!hit)
            continue;
        
		//print (hit.name);
		
        if (hit.rigidbody)
            hit.rigidbody.AddExplosionForce(power, explosionPos+Vector3(0,-1,0), radius, 3.0);
    }

	Destroy(gameObject,5);
	
	//print ("Grenade, flightTime:"+(Time.realtimeSinceStartup-flightTime) + " peakheight:"+peakheight);
	
}

function PlaySoundLater()
{
	audio.Play(); 
}


function Update()
{
	if (transform.position.y>peakheight) peakheight = transform.position.y;
}