using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance = null;

    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<EffectManager>();
            }
            return instance;
        }
    }

    public GameObject[] effectsPrefab;

    public void Instantiate(string prefabName,Vector3 position)
    {
        for (int i = 0; i < effectsPrefab.Length; i++)
        {
            if (effectsPrefab[i].name == prefabName)
            {
                Instantiate(effectsPrefab[i], position, Quaternion.identity);
            }
        }
    }
}