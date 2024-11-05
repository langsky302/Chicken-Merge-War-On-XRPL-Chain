using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pooler
/// - allows the reuse of frequently "spawned" objects for optimization
/// </summary>

namespace UDEV.SPM
{
    [System.Serializable]
    public class PoolerCategory
    {
        public string name;
        [UniqueId]
        public string id;


        public PoolerCategory() { }
        public PoolerCategory(string _name, string _id)
        {
            name = _name;
            id = _id;
        }
    }

    [System.Serializable]
    public class Pool
    {
        public string poolName;
        [UniqueId]
        public string id;
        [HideInInspector]
        public GameObject prefab;
        public Transform startingParent;
        public int startingQuantity = 10;
        [PoolerCategory]
        public string category;

        public Pool(string _poolName, GameObject _prefab, Transform _startingParent, int _startingQuantity, string _category)
        {
            poolName = _poolName;
            prefab = _prefab;
            startingParent = _startingParent;
            startingQuantity = _startingQuantity;
            category = _category;
        }
    }

    [CreateAssetMenu(fileName = "NEW_POOLER", menuName = "UDEV/SPM/Create Pooler")]
    public class ObjectPooler : ScriptableObject
    {
        [UniqueId]
        public string id;
        public PoolerTarget target;

        public List<PoolerCategory> categories = new List<PoolerCategory>();
        public List<Pool> pools;

        public GameObject Spawn(List<GameObject> pooledObjects, string poolId, Vector3 position, Quaternion rotation, Transform parentTransform = null)
        {
            if (!IsPoolerExist(poolId)) return null;

            int poolIndex = GetPoolIndex(poolId);

            var poolObject = GetPool(pooledObjects, poolIndex);
            if (poolObject != null)
            {
                poolObject.SetActive(true);
                poolObject.transform.localPosition = position;
                poolObject.transform.localRotation = rotation;
                if (parentTransform)
                {
                    poolObject.transform.SetParent(parentTransform, false);
                }
                return poolObject;
            }

            poolObject = CreateNewPool(pooledObjects, poolIndex);

            if (poolObject)
            {
                poolObject.transform.localPosition = position;
                poolObject.transform.localRotation = rotation;
                return poolObject;
            }
            return null;
        }

        private GameObject CreateNewPool(List<GameObject> pooledObjects, int poolIndex)
        {
            if (pools == null || pools.Count <= 0) return null;

            GameObject poolObject = null;
            if (pools[poolIndex].prefab == null) return null;

            poolObject = Instantiate(pools[poolIndex].prefab);
            Pooler pooler = poolObject.GetComponent<Pooler>();
            if (!pooler)
            {
                poolObject.AddComponent<Pooler>();
                poolObject.GetComponent<Pooler>().id = pools[poolIndex].id;
            }
            pooledObjects.Add(poolObject);
            return poolObject;
        }

        private GameObject GetPool(List<GameObject> pooledObjects, int poolIndex)
        {
            if (pooledObjects == null || pooledObjects.Count <= 0
                || pools == null || pools.Count <= 0
                ) return null;

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                var pooledObject = pooledObjects[i];
                if (pooledObject == null) continue;
                var pooler = pooledObject.GetComponent<Pooler>();
                if (pooler == null) continue;
                var pool = pools[poolIndex];
                if (pool == null || string.Compare(pool.id, pooler.id) != 0 || pooledObject.activeInHierarchy) continue;
                return pooledObject;
            }

            return null;
        }

        private int GetPoolIndex(string poolId)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (string.Compare(pools[i].id, poolId) == 0)
                {
                    return i;
                }
                if (i == pools.Count - 1)
                {
                    Debug.LogError("There's no pool named \"" + poolId + "\"! Check the spelling or add a new pool with that name.");
                    return 0;
                }
            }
            return 0;
        }

        public void ClearPooledObjects(ref List<GameObject> pooledObjects)
        {
            pooledObjects.Clear();
        }

        public void BackToPools(List<GameObject> pooledObjects)
        {
            if (pooledObjects == null || pooledObjects.Count <= 0) return;
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (pooledObjects[i] == null) continue;
                pooledObjects[i].SetActive(false);
            }
        }

        public Dictionary<string, string> GetPoolIds()
        {
            Dictionary<string, string> ids = new Dictionary<string, string>();

            if (pools == null || pools.Count <= 0) return null;
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i] == null || string.IsNullOrEmpty(pools[i].id)) continue;
                ids[pools[i].id] = GetCategoryName(pools[i].category) + "/" + pools[i].poolName;
            }

            return ids;
        }

        string GetCategoryName(string id)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i] != null && string.Compare(categories[i].id, id) == 0)
                {
                    return categories[i].name;
                }
            }

            return "";
        }

        bool IsPoolerExist(string id)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i] != null && string.Compare(pools[i].id, id) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

