using UnityEngine;
using System.Collections;
using System;
public class barController : MonoBehaviour
{


	public Transform leftBar, rightBar;

	public float speed ;
	public float distanceDivider ;
	float Right_originalXPosition, Left_originalXPosition;
	void OnEnable ()
	{
		
	 

		Right_originalXPosition = rightBar.localPosition.x;

		Left_originalXPosition = leftBar.localPosition.x;

		//To increase bars sliding speed at random 

		//if (UnityEngine.Random.Range (-5, 5) > 3) {

			speed = UnityEngine.Random.Range (1.0f, 5.0f);
		//}

	}

	void OnDisable ()
	{
		
		 
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		//translation two bars using Sine wave .

		leftBar.localPosition = new Vector3 (Left_originalXPosition + (Mathf.Sin (Time.timeSinceLevelLoad * speed)) / distanceDivider, leftBar.localPosition.y, 0);

		rightBar.localPosition = new Vector3 (Right_originalXPosition - (Mathf.Sin (Time.timeSinceLevelLoad * speed)) / distanceDivider, rightBar.localPosition.y, 0);

	
	}
	void OnTriggerEnter2D (Collider2D collider)
	{
		//no need to check if player named  object enter into out trigger.because we have only ball moving around

		if (collider.name.Contains ("Player")) {

			Invoke ("stopMoving", 3); //instead of stop at that the time when it enter,we were stoping after a while.
			//or use you onBecameinvisible ,instead of triggered type actions
		}
	}

	void stopMoving ()
	{
		this.enabled = false; //stoping script ,so it wont move
		//leftBar.GetComponent<Collider2D> ().isTrigger = true;
		////rightBar.GetComponent<Collider2D> ().isTrigger = true;
	}
	 
}
