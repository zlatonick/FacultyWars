using System.Collections.Generic;
using UnityEngine;

namespace Preparing.pools
{
    public class ShopPickedItemsPool : MonoBehaviour
    {
        public GameObject prefab;

        private readonly Stack<GameObject> _inactiveInstances = new Stack<GameObject>();

        public GameObject GetObject()
        {
            GameObject spawnedGameObject;

            if (_inactiveInstances.Count > 0)
            {
                spawnedGameObject = _inactiveInstances.Pop();
            }
            else
            {
                spawnedGameObject = (GameObject) GameObject.Instantiate(prefab);
                var pooledObject = spawnedGameObject.AddComponent<PickedPooledObject>();
                pooledObject.pool = this;
            }

            spawnedGameObject.transform.SetParent(null);
            spawnedGameObject.SetActive(true);

            return spawnedGameObject;
        }

        public void ReturnObject(GameObject toReturn)
        {
            var pooledObject = toReturn.GetComponent<PickedPooledObject>();

            if (pooledObject != null && pooledObject.pool == this)
            {
                toReturn.transform.SetParent(transform);
                toReturn.SetActive(false);
                _inactiveInstances.Push(toReturn);
            }
            else
            {
                Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
                Destroy(toReturn);
            }
        }
    }
}