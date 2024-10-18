using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Awake()
    {
    
        playerStats.LoadStats();
    }

    // Function to add heart points
    public void AddHeartPoints(int points)
    {
        playerStats.heartPoints += points;
        playerStats.SaveStats();
    }

    // Function to add mind points
    public void AddMindPoints(int points)
    {
        playerStats.mindPoints += points;
        playerStats.SaveStats();
    }

    public void ResetStats()
    {
        playerStats.ResetStats();
    }
}
