using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone_Boss_sparkRangeController : MonoBehaviour
{
    public GameObject beforeRange;
    public GameObject afterRange;

    public float rangeSpeed = 0.1f;

    float maxScale;
    float minScale;

    private void OnEnable()
    {
        maxScale = afterRange.transform.localScale.x;
        minScale = 0f;

        beforeRange.transform.localScale = new Vector3(minScale,minScale,minScale);

        beforeRange.SetActive(true);
        afterRange.SetActive(true);

        StartCoroutine("ExpendAttackRange");
    }

    IEnumerator ExpendAttackRange()
    {


        while (true)
        {
            minScale += rangeSpeed;
            yield return new WaitForSeconds(0.01f);

            if (minScale >= maxScale) break;

            beforeRange.transform.localScale = new Vector3(minScale, minScale, minScale);
        }

        beforeRange.SetActive(false);
        afterRange.SetActive(false);
        gameObject.SetActive(false);
    }
}
