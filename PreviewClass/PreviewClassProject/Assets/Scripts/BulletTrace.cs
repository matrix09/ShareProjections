using UnityEngine;
using System.Collections;

public class BulletTrace : MonoBehaviour
{
	public float m_fSpeed = 105f;
	public float m_fMaxTime = 5f;
	private float m_fStartTime = 0f;
	// Use this for initialization

    public void OnTriggerEnter (Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            int score = int.Parse(Controller.Instance.m_FightUIScene.BlueTeam.text);
            if(score > 0)
            {
                score--;
            }

            Controller.Instance.m_FightUIScene.BlueTeam.text = score.ToString();

            score = int.Parse(Controller.Instance.m_FightUIScene.RedTeam.text);

            score++;

            Controller.Instance.m_FightUIScene.RedTeam.text = score.ToString();

            Controller.Instance.m_blueController.RB.AddForce(transform.rotation.eulerAngles * 0.5f);

            Destroy(gameObject);

        }
        else if(other.gameObject.tag == "Enemy")
        {
            int score = int.Parse(Controller.Instance.m_FightUIScene.RedTeam.text);
            if (score > 0)
            {
                score--;
            }

            Controller.Instance.m_FightUIScene.RedTeam.text = score.ToString();

            score = int.Parse(Controller.Instance.m_FightUIScene.BlueTeam.text);

            score++;

            Controller.Instance.m_FightUIScene.BlueTeam.text = score.ToString();

            Controller.Instance.m_redController.RB.AddForce(transform.rotation.eulerAngles * 0.5f);

            Controller.Instance.m_CameraController.m_bIsHit = true;


            Destroy(gameObject);




        }
    }
    
    

	public void OnStart (Vector3 pos, Quaternion rot)
	{
		m_fStartTime = Time.time;

		transform.rotation = rot;
		transform.position = pos;

	}
	Vector3 Tmp;
	// Update is called once per frame
	void Update ()
	{
		if (Time.time - m_fStartTime < m_fMaxTime) {
			transform.Translate (new Vector3 (0f, 0f, m_fSpeed * Time.deltaTime));
		} else {
			Destroy (gameObject);
		}
	}
}
