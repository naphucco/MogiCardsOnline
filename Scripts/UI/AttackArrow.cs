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
    
    public GameObject arrowObject;       
    public Transform arrow;
    public Transform[] dotArrow;

    private bool showing;
    private Vector3 start;
    private Transform selecting;

    public void Display(Transform selecting)
    {
        this.selecting = selecting;
        this.start = selecting.position;
        showing = true;
        arrowObject.SetActive(true);
    }

    public void HideArrow()
    {
        if (Input.GetMouseButtonUp(0))
        {
            showing = false;
            arrowObject.SetActive(false);
        }
    }

    private void CheckCardAlive()
    {
        //hide if card is dead
        if (!selecting)
        {
            HideArrow();
        }
    }

    private void UpdateArrow()
    {
        if (showing)
        {
            Vector3 startPos = new Vector3(start.x, start.y, 0);

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z; // select distance = 10 units from the camera
            Vector3 choosePos = Camera.main.ScreenToWorldPoint(mousePos);
            choosePos.z = 0;

            Vector3 targetPos = choosePos;

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
    }

    #region Unity

    private void Update()
    {
        CheckCardAlive();
        UpdateArrow();
    }

    #endregion
}
