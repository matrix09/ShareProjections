using UnityEngine;
using System.Collections;

public class FreeLookCam : PivotController
{

	public Transform m_TargetTransform;
	public float m_TargetPosSmooth = 10f;



	// target transform rotation
	private Quaternion m_TargetTransformRotation;
	[Range (0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;
	private float m_LookAngle;
	//rotate with Y axis
	private float m_tiltAngle = 0f;
	//rotate with x axis
	// Use this for initialization
	void Start ()
	{
		m_TargetTransformRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
			
		//update player's pos, then update cam's pos
		UpdateCamPos ();
		//update cam's rotation
		UpdateCamRot ();
	}

	//update cam pos
	void UpdateCamPos ()
	{
		transform.localPosition = Vector3.Lerp (transform.localPosition, m_TargetTransform.localPosition, m_TargetPosSmooth * Time.deltaTime);
	}

	//update cam rotation
	void UpdateCamRot ()
	{

		var x = Input.GetAxis ("Mouse X");
		var y = Input.GetAxis ("Mouse Y");

		m_LookAngle += x * m_TurnSpeed;

		//m_tiltAngle -= y * m_TurnSpeed;

		m_TargetTransformRotation = Quaternion.Euler (m_tiltAngle, m_LookAngle, 0f);

		transform.localRotation = Quaternion.Lerp (transform.localRotation, m_TargetTransformRotation, m_TargetPosSmooth * Time.deltaTime);



	}



}
