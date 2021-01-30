using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    [SerializeField] Slider timerSlider;
    [SerializeField] float gameTime;
    private bool stopTimer;


    private static TimeController _instance;
    public static TimeController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeController>();
            }
            return _instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    // Update is called once per frame
    void Update()
    {
        float time = gameTime - Time.time;
        

        if(time <= 0)
        {
            stopTimer = true;
        }

        if (stopTimer == false)
        {
            timerSlider.value = time;
        }


    }
}
