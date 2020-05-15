using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndArea : GamePiece {

    private TextMeshPro text;
    private bool complete;

    // Start is called before the first frame update
    void Start() {
        text = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update() {
        if (!complete) {
            text.text = "FINISH\n";
            for (int i = 0; i <= 5; i ++) {
                text.text += (i == Mathf.FloorToInt((Time.time * 4) % 5)) ? "v" : "|";
                text.text += "\n";
            }
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