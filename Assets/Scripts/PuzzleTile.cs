using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TileStatus {
    Wall,
    Floor,
    Chasm
}

[ExecuteInEditMode]
[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(BoxCollider))]
public class PuzzleTile : MonoBehaviour {

    public TileStatus status;

    public PuzzleGrid grid;
    public Vector2Int pos;

    private MeshFilter mesh { get { if (!_mesh) _mesh = GetComponent<MeshFilter>();return _mesh; } }
    private MeshFilter _mesh;
    private BoxCollider coll { get { if (!_coll) _coll = GetComponent<BoxCollider>(); return _coll; } }
    private BoxCollider _coll;



    void Update() {

        name = "[" + pos.x + ", " + pos.y + "] Tile";
        transform.localPosition = PuzzleGrid.Make2DtoTopDown3D(pos * grid.tileSize);
        
        switch (status) {
            case TileStatus.Chasm:
                mesh.mesh = grid.meshes[grid.MESH_CHASM];
                break;
            case TileStatus.Floor:
                mesh.mesh = grid.meshes[grid.MESH_EMPTY];
                break;
            case TileStatus.Wall:
                mesh.mesh = grid.meshes[grid.MESH_NONE];
                break;
        }
        //Only enable the collider if this is a wall tile
        coll.enabled = status == TileStatus.Wall;
        coll.size = Vector3.one * grid.tileSize + (Vector3.up * 10);

    }

}