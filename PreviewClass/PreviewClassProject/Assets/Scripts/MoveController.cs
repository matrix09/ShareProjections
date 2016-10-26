using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour
{
	//move speed
	[Range (0f, 10f)] [SerializeField] public float m_fMoveSpeed = 5f;

	//rotate speed
	[Range (0f, 10f)] [SerializeField] public float m_fRotSpeed = 5f;

    //0 : player
    //1 : enemy
    public int PlayerType;

	public float m_fRotSmooth;

	public GameObject m_RootObj;

	public GameObject m_GunPortObj;

	private Vector3 m_ModernPos;

	private Quaternion m_ModernRot;

	private float TempAngle;

	private bool m_bJump = false;

	private Rigidbody rb;
    public Rigidbody RB
    {
        get
        {
            return rb;
        }
    }
	// Use this for initialization
	void Start ()
	{
		m_ModernPos = transform.localPosition;

		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		//control jump
		HandleJump ();

		//control rotation
		UpdateRotation ();

		//control position
		UpdatePos ();

		//control fire bullet
		FireBullet ();

	}

	void HandleJump ()
	{
		if ((m_bJump == false) && ((transform.localPosition.y - 0) <= 1f)) {

            //
            if(PlayerType == 0)
            {
                if((Input.GetKeyDown(KeyCode.Space)))
                {
                    m_bJump = true;
                    rb.AddForce(new Vector3(0f, 300f, 0f));
                }
            }
            else
            {
                if ((Input.GetKeyDown(KeyCode.Alpha0)))
                {
                    m_bJump = true;
                    rb.AddForce(new Vector3(0f, 300f, 0f));
                }
            }
         

		}
	}
		
	/*
	 * 
	 * shoot
	 * 
	 * shoot distance
	 * 
	 * 
	 * */
	void FireBullet ()
	{
		if (m_bJump) {
			m_bJump = false;
			return;
		}
        int score = 0;
        if (PlayerType == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if((score = int.Parse(Controller.Instance.m_FightUIScene.BlueTeam.text)) > 0)
                {
                    score--;
                    Controller.Instance.m_FightUIScene.BlueTeam.text = score.ToString();
                    GameObject obj = Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject;
                    obj.transform.parent = null;
                    BulletTrace bt = obj.GetComponent<BulletTrace>();
                    bt.OnStart(m_GunPortObj.transform.position, transform.rotation);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if ((score = int.Parse(Controller.Instance.m_FightUIScene.RedTeam.text)) > 0)
                {
                    score--;
                    Controller.Instance.m_FightUIScene.RedTeam.text = score.ToString();
                    GameObject obj = Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject;
                    obj.transform.parent = null;
                    BulletTrace bt = obj.GetComponent<BulletTrace>();
                    bt.OnStart(m_GunPortObj.transform.position, transform.rotation);
                }
            }
        }        
	}

	void UpdateRotation ()
	{
		if (m_bJump) {
			m_bJump = false;
			return;
		}

        float rot = 0f;
        if(PlayerType == 0)
        {
            rot = Input.GetAxis("Horizontal");
        }
        else
        {
            rot = Input.GetAxis("Horizontal1");
        }
		 

		TempAngle += rot * m_fRotSpeed;

		m_ModernRot = Quaternion.Euler (0f, TempAngle, 0f);

		transform.localRotation = Quaternion.Lerp (transform.localRotation, m_ModernRot, m_fRotSmooth * Time.deltaTime);


	}

	void UpdatePos ()
	{
		if (m_bJump) {
			m_bJump = false;
			return;
		}
        float pos = 0f;

        if(PlayerType == 0)
        {
            pos = Input.GetAxis("Vertical");
        }
        else
        {
            pos = Input.GetAxis("Vertical1");
        }

		if (pos != 0) {
			transform.Translate (new Vector3 (0f, 0f, pos * m_fMoveSpeed * Time.deltaTime));
		}
			
	}
		
}
