using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


    public Vector3 m_vDistance;
    public Vector3 m_vRotation;
    public Transform m_BlueTeam;
    public float m_fPosSmooth;
    public float m_fRotSmooth;
    Vector3 m_vFinalPos;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        m_vFinalPos = m_BlueTeam.transform.position + m_vDistance;
        transform.position = Vector3.Lerp(transform.position, m_vFinalPos, m_fPosSmooth * Time.deltaTime);

        Quaternion tmp = Quaternion.Euler(m_vRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, tmp, m_fRotSmooth * Time.deltaTime);


    }
}
