using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour {

    //Shuffle ST
    public static List<E> Shuffle<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
        int randomIndex = 0;

        if (inputList != null)
        {
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }
        }

        return randomList; //return the new random list
    }
}
