using UnityEngine;

public class AudioManager : MonoBehaviour {
    //public Dictionary<string,AudioSource>

    public static float volum_Background = 0.5f;
    public static float volum_SoundEffect = 1f;
    public static bool playing_Background = true;
    public static bool playing_SoundEffect = true;
    private static bool isInit = false;

    private static GameObject sourceObj_UI;
    private static GameObject sourceObj_Story;

    public enum eAudioType {
        eAT_Backround,  //
        eAT_Scene,      //
        eAT_Skill,      //
        eAT_Shout,      //
        eAT_Monster,    //
        eAT_UI,         //
        eAT_Dialog,     //
    }

    private static void Init() {
        string isSound = PlayerPrefs.GetString("SettingSound");
        string isMusic = PlayerPrefs.GetString("Music");
        //GlobeHelper.LODLevel = PlayerPrefs.GetString("UseLod") == "true" ? 1 : 0;
        //GlobeHelper.LODLevel = GlobeHelper.foreceLowLOD ? 1 : GlobeHelper.LODLevel;

        if (isSound == "" || isSound == "true")//有音�?
        {
            playing_SoundEffect = true;
        }
        else if (isSound == "false")//无音�?
        {
            playing_SoundEffect = false;
        }

        if (isMusic == "" || isMusic == "true")//有音�?
        {
            playing_Background = true;
        }
        else if (isMusic == "false")//无音�?
        {
            playing_Background = false;
        }
    }

    public static void PlayAudio (GameObject sourceObject, string audioName, eAudioType audioType, float volume = -1f) {
        if (isInit == false)
        {
            isInit = true;
            Init();
        }
        if (audioType != eAudioType.eAT_Backround && !playing_SoundEffect) {
            return;
        }
        
        string audioPath = "";
        switch (audioType) {
            case eAudioType.eAT_Backround:
                audioPath = "Audio/Background/" + audioName;
                break;
            case eAudioType.eAT_Scene:
                audioPath = "Audio/Scene/" + audioName;
                break;
            case eAudioType.eAT_Skill:
                audioPath = "Audio/Skill/" + audioName;
                //todo zxf
                //if (Helper.Manager<LevelManager>().isPreLoad) {
                //    Resources.Load(audioPath);
                //    return;
                //}
                break;
            case eAudioType.eAT_Shout:
                audioPath = "Audio/Shout/" + audioName;
                break;
            case eAudioType.eAT_Monster:
                audioPath = "Audio/Monster/" + audioName;
                //if (Helper.Manager<LevelManager>().isPreLoad) {
                //    Resources.Load(audioPath);
                //    return;
                //}
                break;
            case eAudioType.eAT_UI:
                audioPath = "Audio/UI/" + audioName;
                break;
            case eAudioType.eAT_Dialog:
                audioPath = "Audio/Dialog/" + audioName;
                break;
        }

        AudioClip audioClip = Resources.Load(audioPath) as AudioClip;
        if (audioClip) {
            if (audioType == eAudioType.eAT_UI) {
                sourceObject = GetUISourceObj();
            }
            else if (audioType == eAudioType.eAT_Dialog && sourceObject == null)
            {
                sourceObject = GetDialogSourceObj();
            }
            else if (audioType == eAudioType.eAT_Skill) {
                sourceObject = GetSkillSourceObj(sourceObject);
            }

            if (sourceObject == null || audioType == eAudioType.eAT_Backround) {
                sourceObject = Camera.main.gameObject;
            }

            AudioSource audioSource = null;

            if (audioType == eAudioType.eAT_UI || audioType == eAudioType.eAT_Skill) {
                AudioSource[] uiAudioSources = sourceObject.GetComponents<AudioSource>();
                int count = uiAudioSources.Length;
                for (int i = 0; i < count; ++i) {
                    if (!uiAudioSources[i].isPlaying) {
                        audioSource = uiAudioSources[i];
                        break;
                    }
                }

                if (count >= 4 && audioSource == null) {
                    audioSource = uiAudioSources[count - 1];
                }
            }
//             else if (audioType == eAudioType.eAT_Skill)
//             {
//                 audioSource = sourceObject.AddComponent<AudioSource>();
//                 audioSource.maxDistance = 50000;
//             }
            else
            {
                audioSource = sourceObject.GetComponent<AudioSource>();
            }

            if (audioSource == null) {
                audioSource = sourceObject.AddComponent<AudioSource>();
            }

            audioSource.Stop();
            audioSource.playOnAwake = false;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 49000;
            audioSource.maxDistance = 50000;
            audioSource.spatialBlend = 0;

            //mute & volum
            if (audioType == eAudioType.eAT_Backround) {
                audioSource.mute = !playing_Background;
                if (volume >= 0f) {
                    audioSource.volume = volume; 
                }
                else {
                    audioSource.volume = volum_Background; 
                }
                
                audioSource.loop = true;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else {
                audioSource.mute = !playing_SoundEffect;
                if (volume >= 0f) {
                    audioSource.volume = volume;
                }
                else {
                    audioSource.volume = volum_SoundEffect;
                }
                
                audioSource.loop = false;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            
        }
    }

    private static GameObject GetUISourceObj()
    {
        if (sourceObj_UI == null) {
            sourceObj_UI = new GameObject();
            sourceObj_UI.name = "UIAudioObj";
            sourceObj_UI.AddComponent<AudioSource>();
            sourceObj_UI.transform.parent = Camera.main.transform;
            sourceObj_UI.transform.localPosition = Vector3.zero;
        }

        return sourceObj_UI;
    }

    private static GameObject GetDialogSourceObj() {
        if (sourceObj_Story == null) {
            sourceObj_Story = new GameObject();
            sourceObj_Story.name = "StoryDialogAudioObj";
            sourceObj_Story.AddComponent<AudioSource>();
            sourceObj_Story.transform.parent = Camera.main.transform;
            sourceObj_Story.transform.localPosition = Vector3.zero;
        }

        return sourceObj_Story;
    }

    private static GameObject GetSkillSourceObj(GameObject sourcePawn) {
        Transform child = null;
        Transform bestChild = null;
        if (sourcePawn != null) {
            int childCount = sourcePawn.transform.childCount;
            for (int i = 0; i < childCount; i++) {
                if (sourcePawn.transform.GetChild(i).gameObject.activeInHierarchy) {
                    bestChild = sourcePawn.transform.GetChild(i);
                    break;
                } 
            }
        }

        if (bestChild == null) {
            child = Camera.main.transform.FindChild("SkillAudioObj");
            if (child == null) {
                GameObject target = new GameObject();
                target.name = "SkillAudioObj";
                target.AddComponent<AudioSource>();
                target.transform.parent = Camera.main.transform;
                target.transform.localPosition = Vector3.zero;

                child = target.transform;
            }
            return child.gameObject;
        }
        else {
            child = bestChild;
            Transform result = child.FindChild("SkillAudioObj");
            if (result == null) {
                GameObject target = new GameObject();
                target.name = "SkillAudioObj";
                target.AddComponent<AudioSource>();
                target.transform.parent = child;
                target.transform.localPosition = Vector3.zero;

                result = target.transform;
            }

            return result.gameObject; 
        }
    }

    public static void StopDialogAudio()
    {
        GameObject sourceObject = GetDialogSourceObj();
        AudioSource audioSource = sourceObject.GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public static void MuteBackgroundAudio()
    {
        GameObject sourceObject = Camera.main.gameObject;
        if (sourceObject)
        {
            AudioSource audioSource = Camera.main.gameObject.GetComponent<AudioSource>();

            if (audioSource != null)
            {
               audioSource.mute = !playing_Background; ;   
            }
        }
    }

    public static void PauseBackgroundAudio(bool bPause) {
        GameObject sourceObject = Camera.main.gameObject;
        if (sourceObject) {
            AudioSource audioSource = Camera.main.gameObject.GetComponent<AudioSource>();

            if (audioSource != null) {
                if (bPause) {
                    audioSource.Pause();
                }
                else {
                    audioSource.Play();
                }
            }
        }
    }

    public static void PauseListen(bool bPause) {
        AudioListener.pause = bPause;
    }
}