using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI_Rotation : MonoBehaviour
{
    public GameObject target;

    public float value;



    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = target.transform.position + new Vector3(0.2f,0.1f,value);
    }
}
