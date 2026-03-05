using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static private Text         _UI_TEXT;
    static private int          _SCORE = 50;

    private Text txtCom;  //txtCom is a reference to this GO's Text component

    void Awake (){
        _UI_TEXT = GetComponent<Text>();

        //if playerPrefs BestShots exists, read it
        if (PlayerPrefs.HasKey("BestShots")){
            SCORE = PlayerPrefs.GetInt("BestShots");
            }
        //assign the score to BestShots
        PlayerPrefs.SetInt("BestShots", SCORE);
    }

    static public int SCORE{
        get { return _SCORE;}

        private set{
            _SCORE = value;
            PlayerPrefs.SetInt("BestShots", value);

            if (_UI_TEXT != null){
                _UI_TEXT.text = "Least Shots Taken:\n " + value.ToString("#");
            }
        }
    }

    static public void TRY_SET_HIGH_SCORE(int scoreToTry){
        if (scoreToTry >= SCORE) return;  //if scoretotry is too high, return
        SCORE = scoreToTry;
    }

    //following code allows one to easily reset the playerprefs highscore
    [Tooltip("Check this box to reset the BestShots in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    void OnDrawGizmos(){
        if (resetHighScoreNow){
            resetHighScoreNow = false;
            PlayerPrefs.SetInt("BestShots", 50);
            Debug.LogWarning("PlayerPrefs BestShots reset to 50");
        }
    }
}
