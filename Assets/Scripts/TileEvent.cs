using UnityEngine;

public abstract class TileEvent
{
    //Abstract class for base event from tile
    
    public abstract void OnMatch();

    public abstract bool AchievementCompleted();
}
