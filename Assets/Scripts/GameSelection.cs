using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSelection : MonoBehaviour {

    public static GameSelection instance;

    public GamePiece selection;

    private TextMeshProUGUI selectedLabel;
    private Vector2 labelPos;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        selectedLabel = GameObject.Find("SelectionMarkerUI").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() {

        if (selection) {
            selectedLabel.text = selection.name + " SELECTED";
            labelPos = Camera.main.WorldToScreenPoint(selection.transform.position + (Vector3.up * 3.5f));
        } else {
            selectedLabel.text = "NO SELECTION\n(CLICK ON PLAYER)";
            labelPos = Input.mousePosition;
        }

        selectedLabel.rectTransform.position = Vector3.Lerp(selectedLabel.rectTransform.position, labelPos, Time.deltaTime*15);

    }

    public void Deselect () {
        if (!selection)
            return;
        selection.selected = false;
        selection = null;
    }

    public void Select (GamePiece piece) {
        Deselect();
        selection = piece;
        piece.selected = true;
    }

}