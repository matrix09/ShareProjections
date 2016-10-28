using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities;
using System.Collections.Generic;

public class UIScene_Lottery : MonoBehaviour {

    #region 静态数据
    public static int WHOLE_CANDIDATE_NUMBER = 100;
    public static string CANDIDATE_PREFAB_ROUTE = "UI/Prefabs/UI/CandidateItem";
    public static int ONCE_CANDIDATE_NUMBER = 5;
    public static int CANDIDATE_DITANCE = 75;
    #endregion

    List<string> CandidateList = new List<string>();//计数用的
    List<string> CandidateWholeList = new List<string>();//避免重复序列号出现
    Dictionary<int, CandidateItem> CandidateItemDic = new Dictionary<int, CandidateItem>();//获取当前所有候选者的信息

    // Use this for initialization
    void Start () {

        //初始化输入按钮事件
        InitializeEvents();

        //加载候选人列表
        LoadCandidateList();

        //初始化开始抽奖按钮
        SetStartBtnStatus(false);

        //初始化结束抽奖按钮
        SetEndBtnStatus(false);

    }
	
	// Update is called once per frame
	void Update () {
	
        if(m_bIsBeginLotteryAnim)
        {
            //动画
            FrameDegree += LotteryFrameAnimSpeed * Time.deltaTime;
            LotteryFrameTransform.rotation = Quaternion.Lerp(LotteryFrameTransform.rotation, Quaternion.Euler(0f, 0f, FrameDegree), 10);
            LotteryFontTransform.rotation = Quaternion.Lerp(LotteryFontTransform.rotation, Quaternion.Euler(0f, 0f, 0 - FrameDegree), 10);
            if (FrameDegree >= 360)
                FrameDegree = 0f;

            //显示中奖名单
            LotteryResultIndex += 1;
            if(LotteryResultIndex > (ONCE_CANDIDATE_NUMBER - 1 ))
            {
                LotteryResultIndex = 0;
            }

            LotteryResultLabel.text = CandidateItemDic[LotteryResultIndex].CandidateLabel.text;


        }

	}

    #region 事件初始化
    void InitializeEvents()
    {

        List<BoxCollider> boxs = GlobeHelper.GetComponentsAll<BoxCollider>(this.gameObject, true);
        if (boxs != null && boxs.Count > 0)
        {
            for (int j = 0; j < boxs.Count; j++)
            {
                BoxCollider bo = boxs[j];
                if (bo.gameObject.name == "InputPic2")
                {
                    UIEventListener.Get(bo.gameObject).onClick = InputNum;
                }
                else if (bo.gameObject.name == "StartBall")
                {
                    UIEventListener.Get(bo.gameObject).onClick = StartBall;
                }
                else if (bo.gameObject.name == "EndBall")
                {
                    UIEventListener.Get(bo.gameObject).onClick = EndBall;
                }
            }
        }

    }
    #endregion

    #region 开始抽奖按钮
    public BoxCollider PressStartBallCollider;
    public UIButton PressStartBallBtn;
    public UISprite PressStartBallSprite;
    
    void SetStartBtnStatus (bool bOpen)
    {
        PressStartBallCollider.enabled = bOpen;
        PressStartBallBtn.enabled = bOpen;
        if (bOpen)
            PressStartBallSprite.color = Color.white;
        else
            PressStartBallSprite.color = Color.black;
    }

    void StartBall (GameObject obj)
    { 
        //关闭开始抽奖按钮
        SetStartBtnStatus(false);
        //打开关闭抽奖按钮
        SetEndBtnStatus(true);
        //开始播放抽奖动画
        m_bIsBeginLotteryAnim = true;

    }

    #endregion

    #region  结束抽奖按钮
    public BoxCollider PressEndBallCollider;
    public UIButton PressEndBallBtn;
    public UISprite PressEndBallSprite;
    void SetEndBtnStatus(bool bOpen)
    {
        PressEndBallCollider.enabled = bOpen;
        PressEndBallBtn.enabled = bOpen;
        if (bOpen)
            PressEndBallSprite.color = Color.white;
        else
            PressEndBallSprite.color = Color.black;
    }

    void EndBall (GameObject obj)
    {
        SetEndBtnStatus(false);

        ClearData();

        //关闭抽奖动画
        m_bIsBeginLotteryAnim = false;

        LotteryFontTransform.rotation = Quaternion.Euler(Vector3.zero);
    }

    #endregion

    #region Input按钮事件处理
    public UIInput inputs;
    public UISprite InputSprite;
    public BoxCollider InputCollider;
    public UIButton InputBtn;
    void InputNum (GameObject obj)
    {
        LotteryResultLabel.text = "";
        string str = inputs.value;
        inputs.value = "";
        //校验输入数据的合法性
        if (str.Length == 0)
        {

            UIAlert.Show("Invalid Input :: input is empty :) ");
            return;
        }
        for(int i = 0; i < str.Length; i++)
        {
            if ((str[i] >= '0' && str[i] <= '9'))
                continue;
            else
            {
                UIAlert.Show("Invalid Input :: illegal number:) ");
                return;
            }
        }

        //判断是否还可以继续输入
        if (CandidateList.Count >= ONCE_CANDIDATE_NUMBER)
        {
            UIAlert.Show("Candidate Count up to epic, You can't input anymore :)");
            return;
        }

        int num = int.Parse(str);
        //输入数据超过限制
        if(num < 0 || num > WHOLE_CANDIDATE_NUMBER)
        {
            UIAlert.Show("Invalid Input :: input number beyond legal scope :)");
            return;
        }

        //输入数据是否重复
        for(int i = 0; i < CandidateWholeList.Count; i++)
        {
            if(str.CompareTo(CandidateWholeList[i]) == 0)
            {
                UIAlert.Show("Invalid Input :: duplicate candidate numbers :)");
                return;
            }
        }

        CandidateList.Add(str);
        CandidateWholeList.Add(str);
        int n = CandidateList.Count - 1;

        CandidateItemDic[n].UpdateCandidateNum(num);

        if (CandidateList.Count >= ONCE_CANDIDATE_NUMBER)
        {
            //关闭输入框
            SetInputAreaStatus(false);
            //激活开始抽奖按钮
            SetStartBtnStatus(true);
            return;
        }

    }

    //设置输入框区域控件状态
    void SetInputAreaStatus (bool bOpen)
    {
        InputBtn.enabled = bOpen;
        InputCollider.enabled = bOpen;
        if (bOpen)
            InputSprite.color = Color.white;
        else
            InputSprite.color = Color.black;

        inputs.gameObject.SetActive(bOpen);
    }

    #endregion

    #region 加载候选人列表

    public GameObject GridGameObject;
    //只在程序初始化的时候调用一次
    void LoadCandidateList ()
    {
        for (int i = ONCE_CANDIDATE_NUMBER - 1; i >= 0; i--)
        {
            Object obj = Resources.Load(CANDIDATE_PREFAB_ROUTE);
            GameObject go = Instantiate(obj) as GameObject;
            go.name = obj.name + "_" + (i + 1).ToString();

            go.transform.parent = GridGameObject.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(0f , 0f - i * CANDIDATE_DITANCE, 0f);

            CandidateItem ci = go.GetComponent<CandidateItem>();
            ci.UpdateCandidatePic(i + 1);
            CandidateItemDic.Add(i, ci);
        }
    }

    #endregion

    #region 抽奖结束需要清空的数据
    void ClearData ()
    {
        CandidateList.Clear();
        for(int i = 0; i < CandidateItemDic.Count; i++)
        {
            CandidateItemDic[i].CandidateLabel.text = "";
        }
     
        SetInputAreaStatus(true);
        //InputCollider.enabled = true;
        //InputSprite.color = Color.white;
        //inputs.gameObject.SetActive(true);
    }
    #endregion

    #region 抽奖中心动画
    public float LotteryFrameAnimSpeed;//抽奖区域边框图片旋转速度
    public float LotteryFontAnimSpeed;//抽奖区域中心字体旋转速度
    public Transform LotteryFrameTransform;//抽奖边框对象
    public Transform LotteryFontTransform;//抽奖字体对象
    public UILabel LotteryResultLabel;//抽奖结果Label
    int LotteryResultIndex = 0;
    float FrameDegree = 0;
    bool bIsBeginLotteryAnim = false;
    bool m_bIsBeginLotteryAnim
    {
        get
        {
            return bIsBeginLotteryAnim;
        }

       set
        {
            if(value != bIsBeginLotteryAnim)
            {
                bIsBeginLotteryAnim = value;
            }
        }

    }
    //开始抽奖动画
    void BeginLotteryAnim ()
    {

    }
    //结束抽奖动画
    void EndLotteryAnim ()
    {

    }
    #endregion

}
