using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    #region Singleton

    private static CollectibleManager instance;
    public static CollectibleManager Instance
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType<CollectibleManager>();
            return instance;
        }
    }

    #endregion



    #region Properties

    [SerializeField]
    private List<Collectible> _collectiblePool;

    #endregion



    #region MonoBehaviour

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    #endregion



    #region Controller

    private Collectible CreateCollectible(Collectible collectible, Vector3 position)
    {
        Collectible obj = Instantiate(collectible, position, Quaternion.identity);
        return obj;
    }

    #endregion
}
