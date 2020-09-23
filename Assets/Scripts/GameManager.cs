using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;     // Singleton for global access with only 1 instance
    
    [Header("Numbers")]
    public int playerScore;
    public float matchTimer = 60f; 
    private float currMatchTimer;

    public float maxMultiplier = 20f;
    private float currMultiplier;
    public float comboDuration = 3f;
    private float comboTimer;
    [HideInInspector]public float swapCooldownTimer;
    [Space]

    [Header("UI")]
    public Text[] scoreTexts;
    public Text matchTimerText;
    public Text multiplierText;
    public Image multiplierDurationBar;
    public GameObject uiGameOver;
    public GameObject uiMultiplier;
    [Space]

    [Header("Booleans")]
    public bool onSwapCooldown;
    public bool onGameOver;
    public bool isPaused;
    public bool isNearEnd;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        uiMultiplier.SetActive(false);
        uiGameOver.SetActive(false);
        onGameOver = false;

        currMatchTimer = matchTimer;
        currMultiplier = 1f;

        isPaused = true;
    }

    private void Update()
    {
        if (!onGameOver)
            HandleMatchTimer();

        if (currMultiplier > 0)
            HandleTileCombo();

        HandleSwapCooldown();

        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 10f;
        }
        else
            Time.timeScale = 1f;
    }

    public void GetScore(int point)
    {
        int pointsAdded = (int)(point * currMultiplier);
        playerScore += pointsAdded;
        Debug.Log("Added Points  :  " + pointsAdded.ToString());
        
        scoreTexts[0].text = playerScore.ToString();
        scoreTexts[1].text = playerScore.ToString();
    }

    private void HandleMatchTimer()
    {
        if (currMatchTimer <= 0)    // If the timer is up, gameover
        {
            onSwapCooldown = true;      // Player can't swipe tiles on gameover

            scoreTexts[1].text = playerScore.ToString();    // Update score text only on gameover
            uiGameOver.SetActive(true);     // Show GameOver UI on timer ends

            onGameOver = true;

            FindObjectOfType<Menu>().OnGameOver.Invoke();
        }
        else if (isPaused)
        {
            // Do nothing
        }
        else
        {
            matchTimerText.text = currMatchTimer.ToString("F0");   

            if (currMatchTimer < 15f) 
            {
                matchTimerText.color = new Color32(255,100,100,255);

                if (!isNearEnd)
                {
                    FindObjectOfType<Menu>().OnTimerNearEnd.Invoke();

                    isNearEnd = true;
                }
            }
                
            currMatchTimer -= Time.deltaTime;
        }
    }

    private void HandleTileCombo()
    {
        if (comboTimer >= comboDuration )
        {
            uiMultiplier.SetActive(false);

            ResetMultiplier();
        }
        else
        {
            multiplierDurationBar.fillAmount = comboTimer / comboDuration;
            comboTimer += Time.deltaTime;
        }
    }

    public void AddMultiplier(float value)
    {
        if (currMultiplier < maxMultiplier)
            currMultiplier += value;

        if (currMultiplier > maxMultiplier)
            currMultiplier = maxMultiplier;
    }

    public void ResetMultiplier()
    {
        currMultiplier = 1f;
        uiMultiplier.SetActive(false);
        multiplierText.text = "x" + currMultiplier.ToString("F0");
    }

    public void ResetComboTimer()
    {
        comboTimer = 0;
    }

    public void IncreaseMultiplierOnMatched()
    {
        ResetComboTimer();
        AddMultiplier(1f);
        uiMultiplier.SetActive(true);
        multiplierText.text = "x" + currMultiplier.ToString("F0");
    }

    private void HandleSwapCooldown()
    {
        if (swapCooldownTimer <= 0)
        {
            onSwapCooldown = false;
        }
        else
        {
            swapCooldownTimer -= Time.deltaTime;
        }
    }
}
