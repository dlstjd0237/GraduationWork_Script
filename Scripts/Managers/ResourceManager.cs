using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using UnityEngine.AddressableAssets;

namespace BIS.Manager
{
    public class ResourceManager
    {
        private Dictionary<string, Object> _resources = new Dictionary<string, Object>(); 
        private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
        public bool IsLoaded { get; private set; }

        #region Load Resource

        public T Load<T>(string key) where T : Object                   // Key와 이름이 같은 리소스 반환
        {
            if (_resources.TryGetValue(key, out Object resource))       // 리소스가 존재하는지 확인
                return resource as T; 

            return null;
        }

        public GameObject Instantiate(string key, Transform parent = null, bool pooling = false) // Key와 이름이 같은 리소스 생성 및 반환
        {
            GameObject prefab = Load<GameObject>(key); // prefab을 로드
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab : {key}");
                return null;
            }

            //if (pooling)
            //    return Managers.Pool.Pop(prefab);

            GameObject go = Object.Instantiate(prefab, parent); // prefab을 부모 아래에 인스턴스화
            go.name = prefab.name;

            return go;
        }

        public GameObject Instantiate(string key, Vector3 SetPos) // Key와 이름이 같은 리소스 생성 및 반환
        {
            GameObject prefab = Load<GameObject>(key); // prefab을 로드
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab : {key}");
                return null;
            }

            //if (pooling)
            //    return Managers.Pool.Pop(prefab);

            GameObject go = Object.Instantiate(prefab, SetPos, Quaternion.identity);
            go.name = prefab.name;

            return go;
        }

        public void Destroy(GameObject go) // GameObject 삭제
        {
            if (go == null)
                return;

            if (go.TryGetComponent<IPoolable>(out IPoolable mono))
            {
                //���⼭ �ٽ� ��ȯ�������
            }
            //if (Managers.Pool.Push(go))
            //    return;

            Object.Destroy(go);
        }

        #endregion

        #region Addressable

        private void LoadAsync<T>(string key, Action<T> callback = null) where T : Object // 비동기 로드
        {
            // key에 해당하는 리소스가 이미 로드되어 있는지 확인
            if (_resources.TryGetValue(key, out Object resource))
            {
                callback?.Invoke(resource as T);
                return;
            }

            string loadKey = key;
            if (key.Contains(".sprite"))
                loadKey = $"{key}[{key.Replace(".sprite", "")}]";

            // Addressables.LoadAssetAsync<T>를 사용하여 비동기 로드 시작
            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            // 비동기 로드가 완료되면 콜백을 호출
            asyncOperation.Completed += (op) =>
            {
                _resources.Add(key, op.Result);
                _handles.Add(key, asyncOperation);
                callback?.Invoke(op.Result);
            };
        }

        /// <summary>
        /// (count == total) is true = AllComplet 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label">Defualt : PreLoad</param>
        /// <param name="callback"></param>
        public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
        {
            // label에 해당하는 리소스가 이미 로드되어 있는지 확인
            var onHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
            // 비동기 로드가 완료되면 콜백을 호출
            onHandle.Completed += (op) =>
            {
                int loadCount = 0;
                int totalCount = op.Result.Count;

                foreach (var result in op.Result)
                {
                    // result.PrimaryKey에 ".sprite"가 포함되어 있는지 확인
                    if (result.PrimaryKey.Contains(".sprite"))
                    {
                        // sprite 리소스를 로드
                        LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                        {
                            loadCount++;
                            callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                            if (loadCount == totalCount)
                            {
                                IsLoaded = true;
                            }
                        });
                    }
                    else
                    {
                        // 기타 리소스를 로드
                        LoadAsync<T>(result.PrimaryKey, (obj) =>
                        {
                            loadCount++;
                            callback?.Invoke(result.PrimaryKey, loadCount, totalCount);

                            if (loadCount == totalCount)
                            {
                                IsLoaded = true;
                            }
                        });
                    }
                }
            };
        }

        #endregion
    }
}