using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GamePiece {

    public Vector3Int moveTo;
    public Vector3Int pos;

    // Start is called before the first frame update
    void Start() {
        pos = Vector3Int.RoundToInt(transform.localPosition);
    }

    // Update is called once per frame
    void Update() {

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

            Vector3Int targetTile = pos + moveTo;

            Debug.Log(targetTile);
            Debug.Log(PuzzleGrid.instance.array.Length);

            if (targetTile.x >= 0 && 
                targetTile.y >= 0 && 
                targetTile.x < PuzzleGrid.instance.array.Length && 
                targetTile.y < PuzzleGrid.instance.array[targetTile.x].row.Length) {

                PuzzleTile target = PuzzleGrid.instance.array[targetTile.x].row[targetTile.z];

                Debug.Log(target, target);

                if (target.status == TileStatus.Floor) {
                    pos = targetTile;
                    Debug.DrawRay(transform.parent.position + targetTile, Vector3.up * 5, Color.green);
                } else
                    Debug.DrawRay(transform.parent.position + targetTile, Vector3.up * 5, Color.yellow);

            } else 

                Debug.DrawRay(transform.parent.position + targetTile, Vector3.up * 5, Color.red);
            
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * 20);

    }

}