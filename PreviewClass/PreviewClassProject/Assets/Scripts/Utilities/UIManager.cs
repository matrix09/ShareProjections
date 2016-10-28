using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{

    public class UIManager : MonoBehaviour
    {

        private string uiRootStr = "UI Root(3D)";
        private string uiAnchorStr = "AutoScale";
        protected GameObject uiroot;
        protected GameObject uianchor;
        protected GameObject uirootUp;
        protected GameObject uianchorUp;
        private string uiPrefabStr = "UI/Prefabs/";

        private GameObject UIRoot
        {
            get
            {
                if (uiroot == null)
                {
                    uiroot = GameObject.Find(uiRootStr);
                    if (uiroot == null)
                    {
                        GameObject obj = Resources.Load(uiPrefabStr + uiRootStr) as GameObject;
                        if (null != obj)
                        {
                            uiroot = Instantiate(obj) as GameObject;
                        }

                        return null;
                    }

                }
                return uiroot;
            }
        }
        private GameObject UIAnchor
        {
            get
            {
                if (null == uianchor)
                {
                    uianchor = GameObject.Find(uiAnchorStr);
                }
                return uianchor;
            }

        }

        public T UI<T>() where T : Component
        {
            var scene = FindSceneByName(typeof(T).Name);
            if (scene)
            {
                return scene.GetComponent<T>();
            }

            return null;
        }

        public T UI<T>(string sceneName) where T : Component
        {
            var scene = FindSceneByName(sceneName);
            if (scene)
            {
                return scene.GetComponent<T>();
            }

            return null;
        }

        public T UIAndActive<T>() where T : Component
        {
            var scene = FindSceneByName(typeof(T).Name);
            if (scene)
            {
                T target = scene.GetComponent<T>();
                if (target.gameObject.activeInHierarchy)
                {
                    return target;
                }
            }

            return null;
        }

        private GameObject FindSceneByName(string sceneName)
        {


            if (null == UIAnchor)
            {
                return null;
            }

            if (UIAnchor.transform.childCount == 0)
            {
                return null;
            }

            Transform child = UIAnchor.transform.FindChild(sceneName);

            return child == null ? null : child.gameObject;


        }

        public GameObject OpenUISceneByName(string sceneName, string indexStr = "")
        {

            GameObject uiScene = FindSceneByName(sceneName + indexStr);
            if (null != uiScene)
            {
                if (!uiScene.activeInHierarchy)
                    uiScene.SetActive(true);
                return uiScene;
            }


            if (UIRoot)
            {
                GameObject obj = Resources.Load(uiPrefabStr + sceneName) as GameObject;
                if (null != obj)
                {
                    uiScene = Instantiate(obj) as GameObject;
                    uiScene.transform.parent = UIAnchor.transform;
                    uiScene.transform.localScale = Vector3.one;
                    uiScene.transform.localPosition = Vector3.zero;
                    uiScene.name = obj.name;
                    uiScene.name = obj.name + indexStr;
                   
                    return uiScene;
                }
            }



            return null;
        }

        public void CloseAllUIScene ()
        {
            while(UIAnchor.transform.childCount > 0)
            {
                DestroyImmediate(UIAnchor.transform.GetChild(0).gameObject);
            }


        }



    }

}
