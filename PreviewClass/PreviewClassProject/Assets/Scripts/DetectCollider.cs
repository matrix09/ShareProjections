using UnityEngine;
using System.Collections;

public class DetectCollider : MonoBehaviour
{
	private void OnTriggerEnter (Collider other)
	{

		if (other.gameObject.tag == "Player") {
			DoTriggerStuff (other, 0);
		}
        else if(other.gameObject.tag == "Enemy")
        {
            DoTriggerStuff(other, 1);
        }


	}

	private void DoTriggerStuff (Collider other, int type)
	{
        //Debug.Log ("DetectCollider :: OnTriggerEnter-> DoTriggerStuff");

        int score = 0;

        if(type == 0)
        {
            score = int.Parse(Controller.Instance.m_FightUIScene.BlueTeam.text);
            score++;
            Controller.Instance.m_FightUIScene.BlueTeam.text = score.ToString();
        }
        else
        {
            score = int.Parse(Controller.Instance.m_FightUIScene.RedTeam.text);
            score++;
            Controller.Instance.m_FightUIScene.RedTeam.text = score.ToString();
        }

		GameObject obj = Instantiate (Resources.Load ("Prefabs/Explosion"), transform.localPosition, Quaternion.identity) as GameObject;

		Invoke ("SelfDestroy", 0.3f);
	}

	private void SelfDestroy ()
	{

		Transform parent = transform.parent;

		int n = Random.Range (0, 5);
		GameObject obj = null;
		if (n > 3) {
			obj = Instantiate (Resources.Load ("Prefabs/SizeBall")) as GameObject;
		} else {
			obj = Instantiate (gameObject) as GameObject;
		}

		obj.transform.parent = parent;
		obj.transform.localPosition = new Vector3 (Random.Range (-28f, 28f), transform.localPosition.y, Random.Range (-28f, 28f));
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;

		Destroy (gameObject);
	}
}
