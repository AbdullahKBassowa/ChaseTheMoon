using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static bool isMain = true;


    public static bool turnBlack = false;
    public static bool goBack = false;


    public static bool isBlack = false;
    public static bool isGameOver = false;
    public static bool isPaused = false;
    public static int Score;
    public static int HighScore;

    [Header("Screens")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public TMP_Text[] scoreTexts;
    public TMP_Text HighScoreText;
    public GameObject panel;



    [Header("Settings menu")]
    [SerializeField] Slider slider;
    public AudioMixer audioMixer;


    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);

        if (isMain)
        {
            //quick fade in for main menu
            StartCoroutine(FadeToZeroAlpha(0.5f));

            HighScoreText.text = Convert.ToString(HighScore) + "mm";
            
            /*if there is no variable called "volume" i.e
             the player hasn't set any value to the variable volume,
             it will automatically set it to 1. Else, it will load the 
            values previously set.*/
            if (!PlayerPrefs.HasKey("volume"))
            {
                PlayerPrefs.SetFloat("volume", 1);
            }
            else
            {
                Load();
            }


            BlackHole.blackLock = false;
            SpaceshipScript.lockControls = false;
            SpaceshipScript.autoCorrect = false;
            SpaceshipScript.isRotating = false;

        }
        //if it isnt the main menu OR the black
        //hole dimension, this is called.
        else if (!isBlack)
        {
            //slow fade in for main game
            StartCoroutine(FadeToZeroAlpha(2f));
        }

    }







    //global volume things

    //This function is called anytime the volume slider is changed.
    public void ChangeVolume(float sliderValue)
    {
        /*the reason that the slider value is logged to base 10
        and multiplied by 20 is that the values given by the
        volume slider are different to what the values the
        audio master takes, so some math is done to convert the values 
        equally.*/
        audioMixer.SetFloat("AudVol", Mathf.Log10(sliderValue) * 20);
        Save();
    }


    //saves the value of the slider to the variable volume.
    public void Save()
    {
        PlayerPrefs.SetFloat("volume", slider.value);
    }

    //loads the saved value to the slider.
    public void Load()
    {
        slider.value = PlayerPrefs.GetFloat("volume", slider.value);
    }








    void Update()
    {
        if(isMain || isBlack)
        {
            ObstacleManager.UniversalCounter = 0;
        }

        //black hole dimension
        if(turnBlack)
        {
            //loads the black hole scene and sets 
            //isBlack to true and turnBlack to false
            SceneManager.LoadScene("BlackHoleScene");
            isBlack = true;
            turnBlack = false;
        }
        if(goBack)
        {
            //reloads the game scene and does vice
            //versa of the above
            SceneManager.LoadScene("GameScene");
            isBlack = false;
            goBack = false;
        }






        //score                                  mm is MegaMeters
        foreach (TMP_Text t in scoreTexts)
        {
            t.text = Convert.ToString(SpaceshipScript.score) + "mm";
        }
        Score = SpaceshipScript.score;






        if (Input.GetKeyDown(KeyCode.R) && !isMain && isGameOver)
        {
            SpaceshipScript.allowFlip = false;
            FindObjectOfType<Audio>().Play("Click");
            SceneManager.LoadScene("GameScene");
            isGameOver = false;
            isBlack = false;


            SpaceshipScript.score = 0;

            BlackHole.blackLock = false;
            SpaceshipScript.lockControls = false;
            SpaceshipScript.autoCorrect = false;
            SpaceshipScript.isRotating = false;
        }



        if (isGameOver)
        {
            //High score checking and updating
            if(Score >= HighScore)
            {
                HighScore = Score;
            } 
            HighScoreText.text = Convert.ToString(HighScore) + "mm";



            gameOverScreen.SetActive(true);
            pauseScreen.SetActive(false);

            ObstacleManager.UniversalCounter = 0;
            
        }



        if (isPaused && !isGameOver)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
    }



    public static void LoadMain()
    {
        FindObjectOfType<Manager>().StartCoroutine(FadeScreenOut(0.5f, 1));
        isPaused = false;
    }
    
    public static void LoadGame()
    {
        FindObjectOfType<Manager>().StartCoroutine(FadeScreenOut(0.5f, 0));
    }


    public static void ExitGame()
    {
        Application.Quit();
    }


    public static void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }



    public static void ClickSound()
    {
        FindObjectOfType<Audio>().Play("Click");
    }



    // fading
    public static IEnumerator FadeScreenOut(float t, int scene)
    {
        //this long line has to be done again since its a static function
        //(so i can call it from other scripts easily)
        FindObjectOfType<Manager>().panel.SetActive(true);

        //does the opposite as the other function pretty much just with the long
        //bit added
        Color x = FindObjectOfType<Manager>().panel.GetComponent<Image>().color;
        Color i = new Color(x.r, x.g, x.b, 0);


        //does the opposite as the other one.
        while (i.a < 1.0f)
        {
            i = new Color(i.r, i.g, i.b, i.a + (Time.deltaTime / t));
            FindObjectOfType<Manager>().panel.GetComponent<Image>().color = i;
            yield return null;
        }

        //loads the scene from here as I found it easier to do that here instead
        switch (scene)
        {
            case 0:
                SceneManager.LoadScene("GameScene");
                isMain = false;
                isBlack = false;
                break;
            case 1:
                Time.timeScale = 1;
                isPaused = false;
                SceneManager.LoadScene("MainMenuScene");
                isMain = true;
                isBlack = false;
                isGameOver = false;
                break;
        }

    }





    public  IEnumerator FadeToZeroAlpha(float t)
    {
        //Activates/shows the black screen
        panel.SetActive(true);

        //gets the current color of the panel
        Color x = panel.GetComponent<Image>().color;

        //a new color is made using all the colors
        //of x, but with a Alpha of 1 (to automatically
        //make it fully appear)
        Color i = new Color(x.r, x.g, x.b, 1);


        //this repeats while the alpha of the color i
        //is greater than 0.
        while (i.a > 0.0f)
        {
            //it sets the color of i to the same color as before,
            //just with a subtracted alpha value
            //divided by Time.deltaTime so it fades at the same speed
            //no matter what computer runs it
            i = new Color(i.r, i.g, i.b, i.a - (Time.deltaTime / t));

            //sets the color of the panel to i
            panel.GetComponent<Image>().color = i;

            //returns null as IEnumerators need a return value
            yield return null;
        }


        //deactivates the panel so buttons can be pressed.
        panel.SetActive(false);
    }
}
