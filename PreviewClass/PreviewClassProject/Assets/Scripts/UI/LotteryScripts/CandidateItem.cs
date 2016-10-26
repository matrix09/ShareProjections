using UnityEngine;
using System.Collections;

public class CandidateItem : MonoBehaviour {

    public UISprite BackPic;//候选人背景图片
    public UILabel CandidateLabel;//候选人序号

    #region 初始化
    void Start ()
    {
        //CandidateLabel.text = "No. 001";
        CandidateLabel.text = "";
    }
    #endregion

    #region 更新候选人序号
    public void UpdateCandidateNum (int cannum)
    {

        if (cannum < 1 || cannum > UIScene_Lottery.WHOLE_CANDIDATE_NUMBER)
        {
            Debug.LogErrorFormat("CandidateItem :: OnStart -> cannum is illegal ({0})", cannum);
            return;
        }
        string str = "";

        if (cannum < 10)
        {
            str = "00" + cannum.ToString();
        }
        else if (cannum > 9 && cannum < 100)
        {
            str = "0" + cannum.ToString();
        }
        else
        {
            str = cannum.ToString();
        }

        CandidateLabel.text = "No. " + str;
    }
    #endregion

    #region 更新候选人背景
    public void UpdateCandidatePic (int picnum)
    {

        if(picnum <=0 || picnum > 5)
        {
            Debug.LogErrorFormat("CandidateItem :: OnStart -> picnum is illegal ({0})", picnum);
            return;
        }

        //加载候选人背景图片
        BackPic.spriteName = "lucky_" + picnum.ToString();
    }
    #endregion

}
