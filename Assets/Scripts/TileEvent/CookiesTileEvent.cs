using UnityEngine;

public class CookiesTileEvent : TileEvent
{
    private int matchCount;
    private int requiredAmount;
    
    public CookiesTileEvent(int amount)
    {
        requiredAmount = amount;
    }
    
    public override void OnMatch()
    {
        matchCount++;
        Debug.Log("Cookies Matched");
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
