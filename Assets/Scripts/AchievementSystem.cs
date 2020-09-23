using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class AchievementSystem : Observer
{
    public Image achievementBanner;
    public Text achievementText;

    [Space]

    public UnityEvent OnAchievementUnlocked;

    TileEvent cookiesEvent, cakeEvent, vitaminEvent, specialTileEvent;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        
        cookiesEvent = new CookiesTileEvent(3);
        cakeEvent = new CakeTileEvent(10);
        vitaminEvent = new VitaminTileEvent(5);
        specialTileEvent = new SpecialTileEvent(7);
    }

    public void RegisterAllObservers()      // Register all of the observers after the tiles spawned (by Grid.cs)
    {
        foreach (var poi in FindObjectsOfType<PointOfInterest>())
        {
            poi.RegisterObserver(FindObjectOfType<AchievementSystem>());
        }
    }

    public override void OnNotify(string value)
    {
        string key;

        if(value.Equals("Cookies Event"))
        {
            cookiesEvent.OnMatch();
            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match first cookies";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Cake Event"))
        {
            cakeEvent.OnMatch();
            if (cakeEvent.AchievementCompleted())
            {
                key = "Match 10 Cakes";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Vitamin Event"))
        {
            vitaminEvent.OnMatch();
            if (vitaminEvent.AchievementCompleted())
            {
                key = "Match 5 Vitamins";
                NotifyAchievement(key, value);
            }
        }   

        if (value.Equals("SpecialTile Event"))
        {
            vitaminEvent.OnMatch();
            if (vitaminEvent.AchievementCompleted())
            {
                key = "There are Special Tiles Among Us";
                NotifyAchievement(key, value);
            }
        }         
    }

    private void NotifyAchievement(string key, string value)
    {
        if (PlayerPrefs.GetInt(value) == 1)
            return;

        PlayerPrefs.SetInt(value, 1);
        achievementText.text = key + " Unlocked !";
        
        StartCoroutine(ShowAchievementBanner());

        OnAchievementUnlocked.Invoke();
    }

    private void ActivateAchievementBanner(bool active)
    {
        achievementBanner.gameObject.SetActive(active);
    }

    private IEnumerator ShowAchievementBanner()
    {
        ActivateAchievementBanner(true);
        yield return new WaitForSeconds(3f);
        ActivateAchievementBanner(false);
    }
}