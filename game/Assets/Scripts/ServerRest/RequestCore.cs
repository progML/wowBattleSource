using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Data.ServerData;
using Meta;
using UnityEngine;
using UnityEngine.Networking;

namespace ServerRest
{
    public enum ErrorType
    {
        ServerBadResponse,
        ServerBadData
    }

    public class RequestCore : MonoBehaviour
    {
        [SerializeField] private NetworkConfiguration networkConfiguration;
        [SerializeField] private PopUpMessageChannel messageChannel;

        #region Static Functions

        public static string GetArgument(string name, string value)
        {
            return name + "=" + value;
        }

        public static string GetArguments(Dictionary<string, object> args)
        {
            return args.Aggregate("?", (s, pair) => s + pair.Key + "=" + pair.Value + "&");
        }


        //JS Methods
        [DllImport("__Internal")]
        private static extern void RequestImage();

        [DllImport("__Internal")]
        private static extern void SendImage(string jsonParams);

        [DllImport("__Internal")]
        private static extern void InitForms(string jsonParams);

        [DllImport("__Internal")]
        private static extern void FullUploadImage();

        [DllImport("__Internal")]
        private static extern void InjectUserKey(string userkey);

        #endregion

        #region public Methods

        private static bool _initForms = false;

        private void InjectForms()
        {
            if (!_initForms)
            {
                InitForms(networkConfiguration.MyPhotoRoute);
                _initForms = true;
            }
        }

        public void RequestUserImage()
        {
            InjectForms();
            RequestImage();
        }

        public void UploadImage(string route)
        {
            InjectForms();

            SendImage(route);
        }

        public void UploadImageFull()
        {
            InjectForms();

            FullUploadImage();
        }

        private void InjectUserKeyJs(string key)
        {
            InjectForms();
            InjectUserKey(key);
        }

        [SerializeField] private string userKey = "";


        public void SetUserKey(string token)
        {
            userKey = "Bearer " + token;
            InjectUserKeyJs(token);
        }

        private void InjectHeaders(UnityWebRequest request, string userKey)
        {
            request.SetRequestHeader("Content-type", "application/json");
            if (userKey.Length > 0)
            {
                request.SetRequestHeader("Authorization", userKey);
            }
        }

        public void TextureRequest(string route, Action<ErrorType> onError = null,
            Action<Texture> afterLoad = null, bool postError = true)
        {
            var request = UnityWebRequestTexture.GetTexture(route);
            InjectHeaders(request, userKey);
            StartCoroutine(TextureRequestCoroutine(request, onError, afterLoad, postError));
        }

        public void GetRequest<T>(string route, Action<ErrorType> onError = null,
            Action<T> afterLoad = null, bool postError = true) where T : BasicResponse
        {
            var request = UnityWebRequest.Get(route);
            InjectHeaders(request, userKey);
            StartCoroutine(RequestCoroutine<T>(request, onError, afterLoad));
        }

        public void PostRequest<T>(string route, string postData, Action<ErrorType> onError = null,
            Action<T> afterLoad = null, bool postError = true)
            where T : BasicResponse
        {
            var request = new UnityWebRequest(route)
            {
                method = UnityWebRequest.kHttpVerbPOST,
                uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(postData)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            InjectHeaders(request, userKey);
            StartCoroutine(RequestCoroutine<T>(request, onError, afterLoad, postError));
        }

        public void PutRequest<T>(string route, string putData, Action<ErrorType> onError = null,
            Action<T> afterLoad = null, bool postError = true) where T : BasicResponse
        {
            var request = new UnityWebRequest(route)
            {
                method = UnityWebRequest.kHttpVerbPUT,
                uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(putData)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            InjectHeaders(request, userKey);
            StartCoroutine(RequestCoroutine<T>(request, onError, afterLoad, postError));
        }

        #endregion

        #region Internal Methods

        private bool ResponseError(UnityWebRequest webRequest)
        {
            return !webRequest.isDone || webRequest.responseCode != 200;
        }


        private IEnumerator RequestCoroutine<T>(UnityWebRequest requestInstance, Action<ErrorType> onError,
            Action<T> afterLoad = null, bool postError = true)
            where T : BasicResponse
        {
            yield return requestInstance.SendWebRequest();
            if (ResponseError(requestInstance))
            {
                if (postError)
                {
                    messageChannel.SpawnMessage("Ошибка", "Ошибка при загрузке данных с сервера");
                }

                onError?.Invoke(ErrorType.ServerBadResponse);
                yield break;
            }

            T data;
            var error = "";
            try
            {
                data = JsonUtility.FromJson<T>(requestInstance.downloadHandler.text);
                if (!data.success)
                {
                    error = data.message;
                    throw new Exception();
                }
            }
            catch
            {
                if (postError)
                {
                    messageChannel.SpawnMessage("Ошибка",
                        "Ошибка при загрузке данных с сервера" + (error.Length > 0 ? ": "+error : ""));
                }

                onError?.Invoke(ErrorType.ServerBadData);
                yield break;
            }

            afterLoad?.Invoke(data);
        }

        private IEnumerator TextureRequestCoroutine(UnityWebRequest requestInstance, Action<ErrorType> onError,
            Action<Texture> afterLoad = null, bool postError = true)
        {
            yield return requestInstance.SendWebRequest();
            if (ResponseError(requestInstance))
            {
                if (postError)
                {
                    messageChannel.SpawnMessage("Ошибка", "Ошибка при загрузке данных с сервера");
                }

                onError?.Invoke(ErrorType.ServerBadResponse);
                yield break;
            }

            Texture data;
            try
            {
                data = ((DownloadHandlerTexture) requestInstance.downloadHandler).texture;
            }
            catch
            {
                if (postError)
                {
                    messageChannel.SpawnMessage("Ошибка", "Ошибка при загрузке данных с сервера");
                }

                onError?.Invoke(ErrorType.ServerBadData);
                yield break;
            }

            afterLoad?.Invoke(data);
        }

        #endregion
    }
}