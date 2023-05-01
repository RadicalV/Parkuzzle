using System.Collections;
using TMPro;
using UnityEngine;

public class LeaderboardListSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI timeText;


    public void SetTextValues(string position, string playerName, string time)
    {
        positionText.text = position;
        playerNameText.text = playerName;
        timeText.text = time;
    }
}
