using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScene_FightUI : MonoBehaviour {

    // Use this for initialization
	void Start () {

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
                //else if (bo.gameObject.name == "StartBall")
                //{
                //    UIEventListener.Get(bo.gameObject).onClick = StartBall;
                //}
                //else if (bo.gameObject.name == "EndBall")
                //{
                //    UIEventListener.Get(bo.gameObject).onClick = EndBall;
                //}
            }
        }
    }


    #endregion

}
