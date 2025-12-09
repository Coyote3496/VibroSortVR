using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Scoreboard : MonoBehaviour
{

    private int numObjectsToSort = 12;
    private int numObjectsLeft = 12;

    public int correctScore = 0;

    public TMP_Text textForScore;
    public TMP_Text textForTime;

    public Camera[] cameras;

    public LayerMask normalMask;
    public LayerMask gameMask;

    private bool playingGame = false;

    private float timeStart = 0f;

    public ObjectGenerator objGen;

    public bool debugWithoutLightsout = true;

    public GameObject startPanel;

    // Start is called before the first frame update
    void Start()
    {
        textForScore.text = "Awaiting game start.";
        textForTime.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (playingGame) {
            textForScore.text = numObjectsLeft.ToString() + "/" + numObjectsToSort.ToString() + " objects left on table";
            float currTime = Time.time - timeStart;
            textForTime.text = "Time: " + currTime.ToString("0") + " seconds.";
        }

        if (correctScore == numObjectsToSort) {
            EndGame();
        }
    }

    public void StartGame() {
        textForTime.color = new Color(255, 255, 255, 255);

        if (!debugWithoutLightsout) {
            foreach (Camera c in cameras) {
                c.cullingMask = gameMask;
            }
        }

        playingGame = true;

        timeStart = Time.time;

        startPanel.SetActive(false);

        objGen.AssignBins();
        objGen.InstantiateObjects();
    }

    public void EndGame() {
        textForTime.color = new Color(0, 255, 0, 255);
        
        if (!debugWithoutLightsout) {
            foreach (Camera c in cameras) {
                c.cullingMask = normalMask;
            }
        }

        startPanel.SetActive(true);

        playingGame = false;
    }

    public void SetNumObjectsToSort (int n) {
        numObjectsToSort = n;
        numObjectsLeft = numObjectsToSort;
    }

    public void DecreaseObjectsLeft() {
        numObjectsLeft -= 1;
    }

    public void IncreaseObjectsLeft() {
        numObjectsLeft += 1;
    }

    public void DecreaseCorrectScore() {
        correctScore -= 1;
    }
    
    public void IncreaseCorrectScore() {
        correctScore += 1;
    }
}
