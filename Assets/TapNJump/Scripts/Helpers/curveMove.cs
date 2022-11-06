using UnityEngine;
using System.Collections;

public class curveMove : MonoBehaviour
{

	public AnimationCurve curve ;
	[Range(0,1)]
	public float
		speed = 1;
	Vector3 targetToReach;
	[Range(0,5)]
	public float
		move_delay_time = 0;
	Vector3 currentPosition;
	float incrementStep;
	Transform thisTransform;
	public bool QuickSlideOnEnable = true ;
	public Vector3 quickSlideDistance = new Vector3 (-200, 0, 0);
	bool isDelay = false;


	void Start ()
	{

		thisTransform = GetComponent<Transform> ();
		targetToReach = thisTransform.position;
		if (QuickSlideOnEnable) {

			thisTransform.Translate (quickSlideDistance);
		}

		Invoke ("Delay", move_delay_time);

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (isDelay == true) {
			thisTransform.position = Vector3.Lerp (thisTransform.position, targetToReach, curve.Evaluate (incrementStep));
			incrementStep += speed * Time.deltaTime;

			if (incrementStep >= 1)
				Destroy (this);
		}

	}


	void Delay ()
	{
		isDelay = true;
	}

}
