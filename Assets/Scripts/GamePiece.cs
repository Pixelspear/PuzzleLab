using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    public bool selected;

    protected Vector3Int pos;
    public Vector3Int moveTo;

    private MeshRenderer renderer;
    private float grid = 0;

    protected void Start() {

        pos = Vector3Int.RoundToInt(transform.localPosition);

        TryGetComponent(out renderer);
        if (!renderer)
            try {
                renderer = GetComponentInChildren<MeshRenderer>();
            } catch { }
    }

    protected void Update() {

        if (renderer) {
            grid = Mathf.Lerp(grid, selected ? 1 : 0, Time.deltaTime * 5);
            renderer.materials[0].SetFloat("grid", grid);
        }
    }

    protected void OnMouseDown() {

        if (GameSelection.instance.selection == this)
            GameSelection.instance.Deselect();
        else
            GameSelection.instance.Select(this);

    }

}