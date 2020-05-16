using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GamePiece {

    // Start is called before the first frame update
    void Start() {

        base.Start();
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

            Vector3Int targetTile = pos + moveTo;

            if (targetTile.x >= 0 && 
                targetTile.y >= 0 && 
                targetTile.x < PuzzleGrid.instance.array.Length && 
                targetTile.y < PuzzleGrid.instance.array[targetTile.x].row.Length) {

                PuzzleTile target = PuzzleGrid.instance.array[targetTile.x].row[targetTile.z];

                RaycastHit hit;
                Physics.Raycast(transform.parent.position + targetTile + Vector3.up, Vector3.down * 2, out hit);
                Debug.DrawRay(transform.parent.position + targetTile + Vector3.up, Vector3.down * 2, Color.cyan);

                //If the target tile is floor and if no collision with other entities are made on that tile
                if ((target.status == TileStatus.Floor && hit.collider == null) ||
                    //Or if the target tile is being filled by a consolidated bridge
                    (hit.collider && hit.collider.GetComponent<TumbleBridge>() && hit.collider.GetComponent<TumbleBridge>().consolidated)) {
                    pos = targetTile;
                    Debug.DrawRay(transform.parent.position + targetTile, Vector3.up * 0.5f, Color.green);
                } else
                    Debug.DrawRay(transform.parent.position + targetTile, Vector3.up * 0.5f, Color.yellow);

            } else 

                Debug.DrawRay(transform.parent.position + targetTile, Vector3.up * 5, Color.red);

        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * 20);

    }

}