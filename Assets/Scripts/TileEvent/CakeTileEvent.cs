using UnityEngine;

public class CakeTileEvent : TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public CakeTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
        Debug.Log("Cake Matched");
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
