using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionManager : MonoBehaviour {

	public static bool running { get { return runTotal > 0; } }
    private static int runTotal = 0;

    public static void AddMotion()
    {
        runTotal++;
    }

    public static void RemoveMotion()
    {
        runTotal--;
    }
}
