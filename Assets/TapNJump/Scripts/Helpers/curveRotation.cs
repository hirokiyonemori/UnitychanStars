using UnityEngine;
using System.Collections;

public class curveRotation : MonoBehaviour
{


	public AnimationCurve curve;
	public float speed ;
	float increment;
	Transform thisTransform ;
	void Start ()
	{
		 
		thisTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		increment += speed * Time.deltaTime;
		thisTransform.rotation = Quaternion.Euler (0, 0, curve.Evaluate (increment));

		if (increment > 1)
			increment = 0;
	}
}
