using UnityEngine;

public class SpecialTileEvent : TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public SpecialTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
        Debug.Log("Special Matched");
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