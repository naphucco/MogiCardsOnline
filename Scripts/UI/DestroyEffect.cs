using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour {

    private void Start()
    {
        MotionManager.AddMotion();
    }

    public void Destroy()
    {
        MotionManager.RemoveMotion();
        Destroy(gameObject);
    }
}
