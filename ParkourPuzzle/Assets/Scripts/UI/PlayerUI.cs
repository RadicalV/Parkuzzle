using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance { get; private set; }

    [SerializeField] TextMeshProUGUI velocityText;
    [SerializeField] TextMeshProUGUI timerText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        velocityText.text = "0";
        timerText.text = "00:00:00";
    }

    public void UpdateVelocityUI(float velocity)
    {
        velocityText.text = velocity.ToString("F0");
    }

    public void UpdateTimerUI(float time)
    {
        string formatedTime = FormatTime(time);
        timerText.text = formatedTime;
    }

    private string FormatTime(float time)
    {
        TimeSpan ts = TimeSpan.FromSeconds(time);

        String result = ts.ToString("mm\\:ss\\:ff");

        return result;
    }
}