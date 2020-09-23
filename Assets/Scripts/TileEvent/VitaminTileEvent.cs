using UnityEngine;

public class VitaminTileEvent : TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public VitaminTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
        Debug.Log("Vitamin Matched");
    }
    
    public override bool AchievementCompleted()
    {
        if(matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}