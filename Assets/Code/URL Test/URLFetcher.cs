using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.URL_Test
{
    public class URLFetcher : MonoBehaviour
    {
        [SerializeField] private string url;

        [Serializable]
        private class ActionData
        {
            public string moveName;
            public int hpDamage;
            public int healAmount;
            public bool doesApplyGuard;
        }

        void Start()
        {
            StartCoroutine(FetchResult(url));
        }

        [ContextMenu("FetchURL")]
        public void Fetch()
        {
            StartCoroutine(FetchResult(url));
        }

        IEnumerator FetchResult(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    yield break;
                }

                ActionData data = null;
                try
                {
                    data = JsonUtility.FromJson<ActionData>(webRequest.downloadHandler.text);
                }
                catch
                {
                    if (data == null)
                    {
                        Debug.Log("Data is null" + webRequest.downloadHandler.text);
                    }
                    else
                    {
                        //Print all the values of data
                        Debug.Log("moveName:" + data.moveName);
                        Debug.Log("hpDamage:" + data.hpDamage);
                        Debug.Log("healAmount:" + data.healAmount);
                        Debug.Log("doesApplyGuard:" + data.doesApplyGuard);
                    }
                }
            }



        }
    }
}