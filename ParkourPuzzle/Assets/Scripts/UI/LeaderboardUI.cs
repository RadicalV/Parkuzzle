using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public static LeaderboardUI Instance { get; private set; }

    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private Transform leaderboardTemplate;
    [SerializeField] private Button backButton;

    private float getLeaderboardTimer = 0f;

    private void Awake()
    {
        Instance = this;

        backButton.onClick.AddListener(() =>
        {
            Hide();
        });

        leaderboardTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (SurfGameManager.Instance != null)
            UpdateLeaderboardList(SurfGameManager.Instance.GetLeaderboard());
    }

    private void Update()
    {
        getLeaderboardTimer -= Time.deltaTime;

        if (getLeaderboardTimer <= 0f)
        {
            float listLobbiesTimerMax = 5f;
            getLeaderboardTimer = listLobbiesTimerMax;

            Dictionary<string, string> leaderboard = SurfGameManager.Instance.GetLeaderboard();
            UpdateLeaderboardList(leaderboard);
        }
    }

    private void UpdateLeaderboardList(Dictionary<string, string> leaderboard)
    {
        foreach (Transform child in leaderboardContainer)
        {
            if (child == leaderboardTemplate) continue;

            Destroy(child.gameObject);
        }

        int index = 0;

        foreach (var item in leaderboard)
        {
            string position = (index + 1).ToString();

            Transform leaderboardTransform = Instantiate(leaderboardTemplate, leaderboardContainer);

            leaderboardTransform.gameObject.SetActive(true);
            leaderboardTransform.GetComponent<LeaderboardListSingleUI>().SetTextValues(position, item.Key, item.Value);

            index++;
        }
    }
}