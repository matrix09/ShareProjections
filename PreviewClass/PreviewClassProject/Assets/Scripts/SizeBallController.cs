using UnityEngine;
using System.Collections;

public class SizeBallController : MonoBehaviour
{

	public float m_fNormalScale = 1.001f;
	public float m_fBigScale = 3f;
	public float m_fSpeed = 0.4f;
	public float m_fSmooth = 4f;

	private float m_origScale = 1f;

	float tmpScale = 1f;
	bool bIsAddScale = false;
	// Update is called once per frame
	void Update ()
	{
		if (transform.localScale.x > m_fBigScale) {
			bIsAddScale = false;
		} 

		if (transform.localScale.x < m_fNormalScale) {
			bIsAddScale = true;
		}
			
		if (bIsAddScale) {
			tmpScale += m_fSpeed;
		} else {
			tmpScale -= m_fSpeed;
		}

		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (tmpScale, tmpScale, tmpScale), m_fSmooth * Time.deltaTime);

	}
}
