using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    public bool selected;

    protected void OnMouseDown() {

        if (GameSelection.instance.selection == this)
            GameSelection.instance.Deselect();
        else
            GameSelection.instance.Select(this);

    }

}