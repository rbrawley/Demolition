using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //private singleton

    [Header("Inscribed")]
    public Text                 uitLevel; //uitext_level text
    public Text                 uitShots; //uitext_shots text
    public Vector3              castlePos; //place to put castles
    public GameObject[]         castles;  //array of castles

    [Header("Dynamic")]
    public int                  level;  //current level
    public int                  levelMax;  //number of levels
    public int                  shotsTaken; 
    public GameObject           castle;  //current castle
    public GameMode             mode = GameMode.idle;
    public string               showing = "Show Slingshot"; //FollowCam mode

    void Start()
    {
        S = this; //define singleton

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //get rid of old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //destroy old projectiles if they exist
        Projectile.DESTROY_PROJECTILES();

        //instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        //reset goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        //zoom out to show both
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        //show the data in GUITexts
        uitLevel.text = "Level: "+(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        //check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            //change mode to stop checking for level end
            mode = GameMode.levelEnd;

            //zoom out to show both
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            //Start next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level >= levelMax)
        {
            HighScore.TRY_SET_HIGH_SCORE(shotsTaken);
            PlayerPrefs.SetInt("ShotsTaken", shotsTaken);
            SceneManager.LoadScene("_Game_Over");
            //level = 0;
            //shotsTaken = 0;

        }
        else{StartLevel();}
    }

    //static method that allows code anywehre to incremet shotsTaken
    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }

    //static method that allows code anywhere to get a reference to S.castle
    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}
