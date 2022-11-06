using UnityEngine;
using System.Collections;

public class curveScale : MonoBehaviour
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
		float scale = curve.Evaluate (increment);
		thisTransform.localScale = new Vector3 (scale, scale, 0);
		
		if (increment > 1)
			increment = 0;
	}
}
