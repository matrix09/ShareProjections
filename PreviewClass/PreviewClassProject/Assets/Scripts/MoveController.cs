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

	public GameObject m_GunPortObj;

    [HideInInspector]
    public bool m_bIsKeyDown = false;

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

    #region jump
    public void HandleJumpEvent ()
    {
        m_bJump = true;
        rb.AddForce(new Vector3(0f, 300f, 0f));
    }

    public bool CanJump ()
    {
        bool bJump = (m_bJump == false) && ((transform.localPosition.y - 0) <= 1f);
        return bJump;
    }

	void HandleJump ()
	{
		if ((m_bJump == false) && ((transform.localPosition.y - 0) <= 1f)) {

            //
            if(PlayerType == 0)
            {
                if((Input.GetKeyDown(KeyCode.Space)))
                {
                    HandleJumpEvent();
                }
            }
            else
            {
                if ((Input.GetKeyDown(KeyCode.Alpha0)))
                {
                    HandleJumpEvent();
                }
            }
         

		}
	}
    #endregion

    /*
	 * 
	 * shoot
	 * 
	 * shoot distance
	 * 
	 * 
	 * */

    public void HandleFireBullet ()
    {
        int score = 0;
        if ((score = int.Parse(Controller.Instance.m_FightUIScene.BlueTeam.text)) > 0)
        {
            score--;
            Controller.Instance.m_FightUIScene.BlueTeam.text = score.ToString();
            GameObject obj = Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject;
            obj.transform.parent = null;
            BulletTrace bt = obj.GetComponent<BulletTrace>();
            bt.OnStart(m_GunPortObj.transform.position, transform.rotation);
        }
    }
    void FireBullet ()
	{
		if (m_bJump) {
			m_bJump = false;
			return;
		}
      
        if (PlayerType == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HandleFireBullet();
            }
        }
        else
        {
            int score = 0;
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

        if(m_bIsKeyDown == true)
        {
            rot = Controller.Instance.m_FightUIScene.dir;
            m_ModernRot = Quaternion.AngleAxis(rot, Vector3.up);
            TempAngle = rot;
        }
        else
        {
            TempAngle += rot * m_fRotSpeed;


            m_ModernRot = Quaternion.Euler(0f, TempAngle, 0f);

           
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, m_ModernRot, m_fRotSmooth * Time.deltaTime);



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

        if(m_bIsKeyDown)
        {
            pos = 0.6f;
        }


		if (pos != 0) {
			transform.Translate (new Vector3 (0f, 0f, pos * m_fMoveSpeed * Time.deltaTime));
		}
			
	}
		
}
