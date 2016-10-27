using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScene_FightUI : MonoBehaviour {


    #region  摇杆
    public GameObject NaviKeyObj;
    public GameObject LimitArea;

    private Vector3 vOrigPos;
    //private Vector3 vPressPos;
    private Transform t;
    private Camera cam;
    Vector3 newPos = Vector3.zero;
    [HideInInspector]
    public float dir;
    [HideInInspector]
    public Vector3 vDir = Vector3.zero;
    [HideInInspector]
    public Vector3 speed;
    Vector3 v;
    float z = 0f;
    float tangant = 0f;
    float fRadius = 0.2f;
    void InitJoyStick ()
    {
        t = NaviKeyObj.transform;
        vOrigPos = t.position;

        NaviKeyObj.SetActive(false);
    }

    void UpdateJoyStick ()
    {
        if (null == cam)
        {
            cam = NGUITools.FindCameraForLayer(gameObject.layer);
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            TouchNavigation();
        }
        else
        {
            MouseNavigation();
        }
    }

    void PressObj()
    {
        NaviKeyObj.SetActive(true);
        dir = 0f;
        t.position = newPos;

        //GlobeHelper.CurScene.CurMajor.BIsKeyDown = true;
        Controller.Instance.m_blueController.m_bIsKeyDown = true;
    }

    void DragObj()
    {
        vDir = (newPos - vOrigPos).normalized;

        if ((newPos - vOrigPos).magnitude > fRadius)
        {

            t.position = vOrigPos + fRadius * vDir;
        }
        else
        {
            t.position = newPos;
        }

        speed = (t.position - vOrigPos).normalized;
        if (newPos.y != vOrigPos.y)
        {
            tangant = (newPos.x - vOrigPos.x) / (newPos.y - vOrigPos.y);
            dir = Mathf.Rad2Deg * Mathf.Atan(tangant);
        }
        else
        {
            if (newPos.x < vOrigPos.x)
            {
                dir = 270f;
            }
            else
            {
                dir = 90f;
            }
        }

        if (newPos.y < vOrigPos.y)
        {
            float temp = 180 - (Mathf.Rad2Deg * Mathf.Atan(0 - tangant));
            dir = temp;
        }
    }

    void ReleaseObj()
    {
        t.position = vOrigPos;
        NaviKeyObj.SetActive(false);
        dir = 0f;
        //GlobeHelper.CurScene.CurMajor.BIsKeyDown = false;
        Controller.Instance.m_blueController.m_bIsKeyDown = false;
    }

    void HandleTouchBegin(Touch touch)
    {
        if ((newPos - vOrigPos).magnitude <= fRadius)
        {
            dFingerPress[touch.fingerId] = new Vector2(vOrigPos.x, vOrigPos.y);
            PressObj();
        }
    }

    void ResetData(Touch touch)
    {
        dFingerPress.Clear();
        t.position = vOrigPos;
        NaviKeyObj.SetActive(false);
        dir = 0f;
    }

    void HandleTouchEnd(Touch touch)
    {
        if (dFingerPress.ContainsKey(touch.fingerId))
        {
            dFingerPress.Remove(touch.fingerId);
            ReleaseObj();
        }
    }

    void HandleTouchMoved(Touch touch)
    {
        if (dFingerPress.ContainsKey(touch.fingerId))
        {
            DragObj();
        }
    }

    void MouseNavigation()
    {
        v = Input.mousePosition;

        z = 0 - cam.transform.position.z;

        newPos = cam.ScreenToWorldPoint(new Vector3(v.x, v.y, z));

        //if (GlobeHelper.CurScene.CurMajor == null)
       //     return;

        if (Controller.Instance.m_blueController.m_bIsKeyDown == false && true == Input.GetMouseButtonDown(0))
        {

            if ((newPos - vOrigPos).magnitude <= fRadius)
            {

                PressObj();
            }

        }
        else if (Controller.Instance.m_blueController.m_bIsKeyDown == true && true == Input.GetMouseButton(0))
        {
            DragObj();
        }
        else if (Controller.Instance.m_blueController.m_bIsKeyDown == true && true == Input.GetMouseButtonUp(0))
        {

            ReleaseObj();
        }
    }

    private readonly Dictionary<int, Vector2> dFingerPress = new Dictionary<int, Vector2>();

    void TouchNavigation()
    {

        for (int i = 0; i < Input.touches.Length; i++)
        {
            Touch touch = Input.touches[i];

            v = touch.position;

            z = 0 - cam.transform.position.z;

            newPos = cam.ScreenToWorldPoint(new Vector3(v.x, v.y, z));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        HandleTouchBegin(touch);
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        HandleTouchMoved(touch);
                        break;
                    }
                //A finger is touching the screen but hasn't moved since the last frame.
                case TouchPhase.Stationary:
                    {
                        break;
                    }
                //The system cancel tracking for the touch, as when (for example) the user puts the device to her face 
                //or more than five touches happened simultaneously. This is the final phase of a touch.
                case TouchPhase.Canceled:
                    {
                        ResetData(touch);
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        HandleTouchEnd(touch);
                        break;
                    }
            }//----end switch

        }//----end for cycle
    }

    #endregion

    // Use this for initialization
    void Start () {

        //initialize joy stick
        InitJoyStick();

        //initialize Time Counter
        InitTimeCounter();

        //Initialize Event
        InitEvent();

        //Initialize GameOver
        InitalizeGameOver();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateJoyStick();

        if (!UpDateTimeCounter())
        {
            //end game
            ShowGameOverUI();
        }


    }

    #region 计时器
    public UILabel TimeLabel;
    public int TimeLength;
    private int CurTimeLength;
    private float OrigTime;
    void InitTimeCounter ()
    {
        CurTimeLength = TimeLength;
        OrigTime = Time.time;
        TimeLabel.text = "Left Sec : " + CurTimeLength.ToString();
    }

    bool UpDateTimeCounter ()
    {
        if (Time.time - OrigTime >= 1)
        {
            OrigTime = Time.time;
            CurTimeLength--;
            if (CurTimeLength <= 0)
            {
                //结束游戏
                return false;
            }
            TimeLabel.text = "Left Sec : " + CurTimeLength.ToString();
        }

        return true;
    }

    #endregion

    #region 结束游戏
    public UIGameOver m_UIGameOver;
    void InitalizeGameOver ()
    {
        m_UIGameOver.gameObject.SetActive(false);
    }
    void PressEndGameEvent(GameObject obj)
    {
        Time.timeScale = 1f;
        CurTimeLength = TimeLength;
        BlueTeam.text = "3";
        RedTeam.text = "3";
        m_UIGameOver.gameObject.SetActive(false);
    }
    void ShowGameOverUI ()
    {
        m_UIGameOver.gameObject.SetActive(true);
        Time.timeScale = 0f;
        int bluescore = int.Parse(BlueTeam.text);
        int redscore = int.Parse(RedTeam.text);

        if(bluescore > redscore)
        {
            m_UIGameOver.m_GameResult.text = "Blue Win";
        }
        else if(bluescore < redscore)
        {
            m_UIGameOver.m_GameResult.text = "Red Win";
        }
        else
        {
            m_UIGameOver.m_GameResult.text = "nobody Win";
        }


    }
    #endregion

    #region 计分器
    public UILabel BlueTeam;
    public UILabel RedTeam;
    #endregion

    #region 初始化事件
    void InitEvent ()
    {
        List<BoxCollider> boxs = GlobeHelper.GetComponentsAll<BoxCollider>(this.gameObject, true);
        if (boxs != null && boxs.Count > 0)
        {
            for (int j = 0; j < boxs.Count; j++)
            {
                BoxCollider bo = boxs[j];
                if (bo.gameObject.name == "Button")
                {
                    UIEventListener.Get(bo.gameObject).onClick = PressEndGameEvent;
                }
                else if (bo.gameObject.name == "Jump")
                {
                    UIEventListener.Get(bo.gameObject).onClick = PressJumpEvent;
                }
                else if (bo.gameObject.name == "Fire")
                {
                    UIEventListener.Get(bo.gameObject).onClick = PressFireEvent;
                }
            }
        }
    }


    #endregion


    #region fire 

    void PressFireEvent (GameObject obj)
    {
        Controller.Instance.m_blueController.HandleFireEvent();
    }

    #endregion

    #region jump
    void PressJumpEvent(GameObject obj)
    {
        if(Controller.Instance.m_blueController.CanJump ())
          Controller.Instance.m_blueController.HandleJumpEvent();
    }
    #endregion

}
