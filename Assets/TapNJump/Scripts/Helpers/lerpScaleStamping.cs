using UnityEngine;
using System.Collections;

public class lerpScaleStamping : MonoBehaviour
{

	Transform thisTransform;
	Vector3 originalScale ;

	[Range(1,5)]
	public float
		ScaleRation = 3;
	[Range(0,2)]
	public float
		scaleSpeed = 3;
	 
	float increment;
	void Start ()
	{
		
		thisTransform = GetComponent<Transform> ();

		originalScale = thisTransform.localScale;

		thisTransform.localScale *= ScaleRation;
	}

	// Update is called once per frame
	void Update ()
	{
	
		thisTransform.localScale = Vector3.Lerp (thisTransform.localScale, originalScale, increment);

		increment += scaleSpeed * Time.deltaTime;
	}
}
