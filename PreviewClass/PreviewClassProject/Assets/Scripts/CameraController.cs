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


        CamHitAnim();

    }

    #region 子弹碰撞UI扭曲
    public bool m_bIsHit = false;
    private CustomImageEffect m_CamCustomImageEffect;
    bool m_IsIncrease = true;
    float m_Step = 0f;
    public void CamHitAnim ()
    {
        if(m_bIsHit)
        {
            if(null == m_CamCustomImageEffect)
            {
                m_CamCustomImageEffect = gameObject.GetComponent<CustomImageEffect>();
            }

           if(m_IsIncrease)
                m_Step += 0.005f;
           else
                m_Step -= 0.005f;
            m_CamCustomImageEffect.EffectMaterial.SetFloat("_Magnitude", m_Step);
            if (m_CamCustomImageEffect.EffectMaterial.GetFloat("_Magnitude") >= 0.1)
            {
                m_IsIncrease = false;
            }
            else if(m_CamCustomImageEffect.EffectMaterial.GetFloat("_Magnitude") <=0)
            {
                m_IsIncrease = true;
                m_bIsHit = false;
            }

          
        }
    }
    #endregion


}
