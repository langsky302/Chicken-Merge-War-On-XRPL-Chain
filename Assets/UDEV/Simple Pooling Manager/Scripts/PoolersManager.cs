using UnityEngine;
using System.Collections.Generic;

namespace UDEV.SPM
{
    public class PoolersManager : Singleton<PoolersManager>
    {
        public bool dontDestroyOnloadPool;

        public ObjectPooler[] objectPoolers;

        private List<PoolerItem> m_poolerItems;

        [System.Serializable]
        private class PoolerItem
        {
            public PoolerTarget target;
            public List<GameObject> pooledObjects;
            public PoolerItem()
            {

            }
            public PoolerItem(PoolerTarget _target, List<GameObject> _pooledObjects)
            {
                target = _target;
                pooledObjects = _pooledObjects;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init()
        {
            m_poolerItems = new List<PoolerItem>();

            if (objectPoolers == null || objectPoolers.Length <= 0) return;

            for (int i = 0; i < objectPoolers.Length; i++)
            {
                if (objectPoolers[i] == null) continue;
                m_poolerItems.Add(new PoolerItem(objectPoolers[i].target, new List<GameObject>()));
                var pools = objectPoolers[i].pools;
                var poolerItem = m_poolerItems[i];
                for (int j = 0; j < pools.Count; j++)
                {
                    var pool = pools[j];
                    if (pool == null) continue;
                    PoolsInitialize(pool, poolerItem);
                }
            }
        }

        private void PoolsInitialize(Pool pool, PoolerItem poolerItem)
        {
            for (int i = 0; i < pool.startingQuantity; i++)
            {
                GameObject poolObject = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, pool.startingParent ? pool.startingParent : null);
                Pooler pooler = poolObject.GetComponent<Pooler>();
                if (!pooler)
                {
                    poolObject.AddComponent<Pooler>();
                    pooler = poolObject.GetComponent<Pooler>();
                }
                pooler.id = pool.id;
                poolObject.SetActive(false);
                poolerItem.pooledObjects.Add(poolObject);
            }
        }

        public GameObject Spawn(PoolerTarget target, string poolId, Vector3 position, Quaternion rotation, Transform parentTransform = null)
        {
            var poolers = GetPoolers(objectPoolers, target);

            GameObject pool = null;

            if (poolers != null && poolers.Count > 0)
            {
                for (int i = 0; i < poolers.Count; i++)
                {
                    if (poolers[i] == null) continue;
                    var poolerObjs = GetPooleds(poolers[i].target);
                    pool = poolers[i].Spawn(poolerObjs, poolId, position, rotation, parentTransform);

                    if (dontDestroyOnloadPool && pool)
                        DontDestroyOnLoad(pool);

                    if (pool == null) continue;
                    return pool;
                }
            }
            else
            {
                Debug.LogWarning("Pooler for " + target.ToString() + " is Null.Please add it to Pooler Manager!.");
            }

            if (pool == null)
            {
                Debug.LogWarning("Pool key does not exist in pooler for " + target.ToString());
            }

            return pool;
        }

        public GameObject GetPrefab(PoolerTarget target, string poolId)
        {
            var poolers = GetPoolers(objectPoolers, target);

            GameObject pool = null;

            if (poolers != null && poolers.Count > 0)
            {
                for (int i = 0; i < poolers.Count; i++)
                {
                    Pool[] findeds = new Pool[] { };
                    var pooler = poolers[i];
                    if (pooler == null) continue;
                    pool = GetPoolPrefab(pooler.pools, poolId);
                    if (pool == null) continue;
                    return pool;
                }
            }
            else
            {
                Debug.LogWarning("Pooler for " + target.ToString() + " is Null.Please add it to Pooler Manager!.");
            }

            if (pool == null)
            {
                Debug.LogWarning("Pool key does not exist in pooler for " + target.ToString());
            }

            return pool;
        }

        private List<ObjectPooler> GetPoolers(ObjectPooler[] objectPoolers, PoolerTarget target)
        {
            List<ObjectPooler> poolers = new List<ObjectPooler>(); ;
            if (objectPoolers == null || objectPoolers.Length <= 0) return null;

            for (int i = 0; i < objectPoolers.Length; i++)
            {
                var objectPooler = objectPoolers[i];
                if (objectPooler == null || objectPooler.target != target) continue;
                poolers.Add(objectPooler);
            }
            return poolers;
        }

        private GameObject GetPoolPrefab(List<Pool> pools, string poolId)
        {
            if (pools == null || pools.Count <= 0) return null;

            for (int i = 0; i < pools.Count; i++)
            {
                var pool = pools[i];
                if (pool == null) continue;
                if (string.Compare(pool.id, poolId) == 0) return pool.prefab;
            }
            return null;
        }

        public void Clear(PoolerTarget target)
        {
            var poolers = GetPoolers(objectPoolers, target);

            if (poolers == null || poolers.Count <= 0) return;
            for (int i = 0; i < poolers.Count; i++)
            {
                if (poolers[i] == null) continue;
                var poolerObjs = GetPooleds(poolers[i].target);
                poolers[i].ClearPooledObjects(ref poolerObjs);
            }
        }

        public void ClearAll()
        {
            if (objectPoolers == null || objectPoolers.Length <= 0) return;
            for (int i = 0; i < objectPoolers.Length; i++)
            {
                if (objectPoolers[i] == null) return;
                var poolerObjs = GetPooleds(objectPoolers[i].target);
                objectPoolers[i].ClearPooledObjects(ref poolerObjs);
            }
        }

        public void RollBackAllToPools()
        {
            if (objectPoolers == null || objectPoolers.Length <= 0) return;
            for (int i = 0; i < objectPoolers.Length; i++)
            {
                if (objectPoolers[i] == null) return;
                var poolerObjs = GetPooleds(objectPoolers[i].target);
                objectPoolers[i].BackToPools(poolerObjs);
            }
        }

        public List<GameObject> GetPooleds(PoolerTarget target)
        {
            if (m_poolerItems == null || m_poolerItems.Count <= 0) return null;
            for (int i = 0; i < m_poolerItems.Count; i++)
            {
                var poolerItem = m_poolerItems[i];
                if (poolerItem == null || poolerItem.target != target) continue;
                return poolerItem.pooledObjects;
            }

            return null;
        }
    }
}