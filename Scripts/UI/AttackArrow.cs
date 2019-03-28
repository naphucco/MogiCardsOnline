using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrow : MonoBehaviour {

    //Use 1 unique instance
    private static AttackArrow instance = null;

    public static AttackArrow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AttackArrow>();
            }
            return instance;
        }
    }

    public bool showing { get; private set; }

    public GameObject arrowObject;       
    public Transform arrow;
    public Transform[] dotArrow;

    private Vector3 start;
    private Vector3 end;

    public void Display(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
        showing = true;
        UpdateArrow();
        arrowObject.SetActive(true);
    }

    public void Hide()
    {
        showing = false;
        arrowObject.SetActive(false);
    }

    private void UpdateArrow()
    {
        Vector3 startPos = new Vector3(start.x, start.y, 0);        
        Vector3 targetPos = new Vector3(end.x, end.y, 0);

        //Vector3 midPointVector = (targetPos + startPos) / 2;
        //arrow.position = midPointVector;

        arrow.position = targetPos;

        for (int i = 0; i < dotArrow.Length; i++)
        {
            float lerp = (float)i / dotArrow.Length;
            dotArrow[i].position = Vector3.Lerp(startPos, targetPos, lerp);
        }

        Vector3 relative = targetPos - startPos;
        //float maggy = relative.magnitude;


        //arrow.localScale = new Vector3(maggy / 2, 1, 0);
        //        Quaternion rotationVector = Quaternion.LookRotation (relative);
        //        rotationVector.z = 0;
        //        rotationVector.w = 0;
        //        transform.rotation = rotationVector - 90;

        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.rotation = q;
    }

    // Update is called once per frame
    private void Update()
    {   
        UpdateArrow();
    }
}
