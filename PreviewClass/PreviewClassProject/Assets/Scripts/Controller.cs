using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public MoveController m_blueController;
    public MoveController m_redController;
    public UIScene_FightUI m_FightUIScene;

    public static Controller Instance;

    // Use this for initialization
    void OnEnable () {
        Instance = this;
    }
	
    void OnDisable()
    {
        Instance = null;
        
    }
	
}
