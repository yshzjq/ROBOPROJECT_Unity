using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SliderRunTo1 : MonoBehaviour
{
 
    public bool b=true;


    float[] basePlayerATT;
	public float speed;

	public int typeValue;
	public string[] typeNames;

    float maxValue;

    Slider slider;

    float time =0f;

    DatabaseReference reference;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        typeNames = new string[4]{"ATKUP","HPUP","SPDUP","ATKSPDUP"};
        basePlayerATT = new float[4] { 20f, 100f,  5f, 1f };

        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                maxValue = basePlayerATT[typeValue] + (basePlayerATT[typeValue] * 0.1f * float.Parse(snapshot.Child(typeNames[typeValue]).GetRawJsonValue()));

                Debug.Log(maxValue);
            }
        });

        
    }

    private void Start()
    {
        StartCoroutine("bvalueChange");
    }
    IEnumerator bvalueChange()
    {
        yield return new WaitForSeconds(0.5f);
        b = true;
    }

    void Update()
    {
        if (b)
        {
            time += speed;
            slider.value = time;

            if (time >= maxValue)
            {
                b = false;
            }

        }
        
    }
	
}
