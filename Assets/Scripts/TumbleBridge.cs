using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;

public enum TumbleBridgeStatus {
    Upright,
    Sideways
}

[RequireComponent (typeof(MeshRenderer))]
public class TumbleBridge : GamePiece {

    public TumbleBridgeStatus status;
    public Vector3Int tallAxis = Vector3Int.up;
    public Vector3 floatPos; //Unfortunately, this piece will often land inbetween tiles, so pos must allow for increments of 0.5

    private Vector3 animStartPos;
    private Quaternion animStartRot;
    private Vector3 animEndPos;
    private Quaternion animEndRot;
    private MeshRenderer renderer;
    private Color fillCol;
    private Color fillConsolidatedCol;

    public bool consolidated;

    // Start is called before the first frame update
    void Start() {

        base.Start();

        floatPos = transform.localPosition;

        renderer = GetComponent<MeshRenderer>();
        fillConsolidatedCol = renderer.materials[1].GetColor("_EmissionColor");

    }

    // Update is called once per frame
    void Update() {

        base.Update();

        moveTo = Vector3Int.zero;

        if (selected) {
            if (Input.GetKeyDown(KeyCode.W))
                moveTo = new Vector3Int(1, 0, 0);
            if (Input.GetKeyDown(KeyCode.A))
                moveTo = new Vector3Int(0, 0, 1);
            if (Input.GetKeyDown(KeyCode.S))
                moveTo = new Vector3Int(-1, 0, 0);
            if (Input.GetKeyDown(KeyCode.D))
                moveTo = new Vector3Int(0, 0, -1);
        }


        if (moveTo != Vector3Int.zero) {

            Vector3 moveAxis = moveTo;
            PuzzleTile target1 = null;
            PuzzleTile target2 = null;

            //Fortunately, target tile detection is simple; if the bridge is upright, it will ALWAYS take up the next two tiles in the selected direction, and if it is upright, it will only take up one tile in the selected direction
            if (status == TumbleBridgeStatus.Upright) {

                //Determine which tiles will be landed on
                try {
                    target1 = PuzzleGrid.instance.array[Mathf.RoundToInt(moveTo.x + floatPos.x)].row[Mathf.RoundToInt(moveTo.z + floatPos.z)];
                    target2 = PuzzleGrid.instance.array[Mathf.RoundToInt(moveTo.x * 2 + floatPos.x)].row[Mathf.RoundToInt(moveTo.z * 2 + floatPos.z)];
                    //Cancel the turn if either tile is out of bounds, or cannot be obtained for any reason
                } catch { Debug.LogError(name + " attempted to move out of bounds.", this); return; }

                Debug.DrawRay(target1.transform.position, Vector3.up);
                Debug.DrawRay(target2.transform.position, Vector3.up);

                //If the two tiles it will try to land on are different, or if either are walls, cancel this turn
                if (target1.status != target2.status || target1.status == TileStatus.Wall || target2.status == TileStatus.Wall)
                    return;

                //If both tiles are chasms, proceed with the turn, but now the bridge will be consolidated
                if (target1.status == TileStatus.Chasm && target1.status == TileStatus.Chasm)
                    consolidated = true;
                //If upright, define which axis will be longer (x or z)
                tallAxis = new Vector3Int(Math.Abs(moveTo.x), 0, Math.Abs(moveTo.z));
                moveAxis *= 0.5f;

            } else {

                //Determine which tile will be landed on
                try {
                    //If bridge is on its side and its going to roll on its side, we must check the TWO adjacent tiles' status
                    if (Mathf.Abs(moveTo.x) != tallAxis.x && Mathf.Abs(moveTo.z) != tallAxis.z) {
                        //Check if x has a decimal (if x axis will be different for each tile)
                        if (Math.Abs(floatPos.x - Mathf.RoundToInt(floatPos.x)) > Mathf.Epsilon) {
                            target1 = PuzzleGrid.instance.array[Mathf.FloorToInt(moveTo.x + floatPos.x)].row[Mathf.RoundToInt(moveTo.z + floatPos.z)];
                            target2 = PuzzleGrid.instance.array[Mathf.CeilToInt(moveTo.x + floatPos.x)].row[Mathf.RoundToInt(moveTo.z + floatPos.z)];
                            //Otherwise assume z has a decimal
                        } else {
                            target1 = PuzzleGrid.instance.array[Mathf.RoundToInt(moveTo.x + floatPos.x)].row[Mathf.FloorToInt(moveTo.z + floatPos.z)];
                            target2 = PuzzleGrid.instance.array[Mathf.RoundToInt(moveTo.x + floatPos.x)].row[Mathf.CeilToInt(moveTo.z + floatPos.z)];
                        }
                        //Otherwise, only check the tile that is 1.5 tiles away in the targetted direction
                    } else {
                        target1 = PuzzleGrid.instance.array[Mathf.RoundToInt(moveTo.x * 1.5f + floatPos.x)].row[Mathf.RoundToInt(moveTo.z * 1.5f + floatPos.z)];
                    }
                    //Cancel the turn if either tile is out of bounds, or cannot be obtained for any reason
                } catch { Debug.LogError(name + " attempted to move out of bounds.", this); return; }

                Debug.DrawRay(target1.transform.position, Vector3.up);
                if (target2)
                    Debug.DrawRay(target2.transform.position, Vector3.up);

                //For now, I'm planning for TumbleBridges to not work if they enter a chasm upright, and can only become a bridge if they land on a chasm sideways. In other words, when becoming upright, the target tile must be floor
                //If there is only one target tile (will become upright) only step on floor tiles
                if (!target2 && target1.status != TileStatus.Floor)
                    return;
                //If both target tiles are not the same, or if they are both walls 
                if (target2)
                    if (target1.status != target2.status || target1.status == TileStatus.Wall)
                        return;
                    else if (target1.status == TileStatus.Chasm && target1.status == TileStatus.Chasm)
                        consolidated = true;

                if (Mathf.Abs(moveTo.x) == tallAxis.x && Mathf.Abs(moveTo.z) == tallAxis.z) {
                    //If the bridge is about to tilt onto its square face, use double-tile axis and define the tall axis as facing up
                    tallAxis = Vector3Int.up;
                } else
                    //If the bridge is "Rolling" sideways, keep the same tall axis (orientation is the same) but use single-tile axis
                    moveAxis *= 0.5f;

            }

            Debug.DrawRay(Vector3.Scale(transform.position, new Vector3(1, 0, 1)) + moveAxis, DirToAxis(moveTo), Color.red);

            status = tallAxis.y != 0 ? TumbleBridgeStatus.Upright : TumbleBridgeStatus.Sideways;

            transform.RotateAround(Vector3.Scale(transform.position, new Vector3(1, 0, 1)) + moveAxis, DirToAxis(moveTo), 90);

            //If this bridge has landed in a gap
            if (consolidated) {
                //Put it inside the gap
                transform.position += Vector3.down;
                //Deselect it
                GameSelection.instance.Deselect();
            }

            floatPos = Vector3.Scale(transform.localPosition, new Vector3(1, 0, 1));

        }

        if (consolidated) {
            fillCol = Color.Lerp(fillCol, fillConsolidatedCol, Time.deltaTime * 10);
            renderer.materials[0].SetFloat("grid", -1);
        }
        
        renderer.materials[1].SetColor("_EmissionColor", fillCol);

    }

    public void OnMouseDown() {
        //Only allow selection if this bridge hasn't already consolidated
        if (!consolidated)
            base.OnMouseDown();
    }

    public static Vector3Int DirToAxis (Vector3Int vec) {
        return new Vector3Int(vec.z, vec.y, vec.x * -1);
    }

}