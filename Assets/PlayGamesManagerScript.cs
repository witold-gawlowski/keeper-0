using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class PlayGamesManagerScript : MonoBehaviour
{
    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn();

    }
    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    public void SubmitScoreToLeaderboard()
    {
        AddScoreToLeaderboard(GPGSIds.leaderboard_completed, 334);
        Social.ShowLeaderboardUI();
    }

    public static void AddScoreToLeaderboard(string leaderboardID, int score)
    {
        Debug.Log("tring to send score");
        Social.ReportScore(score, leaderboardID, success => {Debug.Log("callback worked"); });
    }
}
