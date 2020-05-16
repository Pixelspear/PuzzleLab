using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndArea : GamePiece {

    [Header ("Prepend \"STOP.\" to scene name to stop next scene from loading after completion")]
    public float nextSceneTimer;

    private TextMeshPro text;
    private bool finalPuzzle;
    private bool complete;

    // Start is called before the first frame update
    void Start() {
        text = GetComponentInChildren<TextMeshPro>();
        if (SceneManager.GetActiveScene().name.Split('.')[0] == "STOP")
            finalPuzzle = true;
    }

    // Update is called once per frame
    void Update() {
        if (!complete) {
            text.text = "FINISH\n";
            for (int i = 0; i <= 5; i ++) {
                text.text += (i == Mathf.FloorToInt((Time.time * 4) % 5)) ? "v" : "|";
                text.text += "\n";
            }
        } else {
            text.text = "COMPLETE!\n";
            if (!finalPuzzle) {
                text.text += "Next Puzzle In: \n" + Math.Round(nextSceneTimer * 100) / 100;
                if (nextSceneTimer <= 0) {
                    try {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    } catch (Exception e) {
                        Debug.Log(e);
                        text.text += "\nERROR: Could not load next scene";
                    }
                } else {
                    nextSceneTimer = Math.Max (0, nextSceneTimer - Time.deltaTime);
                }
            } else
                text.text += "End Of Puzzles in Current Playlist";
        }
    }

    private void OnTriggerEnter(Collider other) {

        complete = true;
        text.text = "COMPLETE!";
        GameSelection.instance.Deselect();

    }

new public void OnMouseDown () {

        Debug.Log("End Area cannot be directly selected");

    }

}