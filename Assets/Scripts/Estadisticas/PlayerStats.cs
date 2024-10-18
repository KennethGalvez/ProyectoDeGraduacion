using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int heartPoints;
    public int mindPoints;

    public void ResetStats()
    {
        heartPoints = 0;
        mindPoints = 0;
        SaveStats();
    }


    public void SaveStats()
    {
        PlayerPrefs.SetInt("HeartPoints", heartPoints);
        PlayerPrefs.SetInt("MindPoints", mindPoints);
        PlayerPrefs.Save();
    }

    public void LoadStats()
    {
        heartPoints = PlayerPrefs.GetInt("HeartPoints", 0);
        mindPoints = PlayerPrefs.GetInt("MindPoints", 0);
    }
}
