using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{

	public float m_fMaxTime = 3f;

	private float m_fBeginTime = 0f;

	// Use this for initialization
	void Start ()
	{
		m_fBeginTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time - m_fBeginTime > m_fMaxTime) {
			Destroy (gameObject);
		}
	}
}
