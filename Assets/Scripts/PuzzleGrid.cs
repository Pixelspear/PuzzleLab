using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[System.Serializable]
public class PuzzleRow {
    public PuzzleTile[] row;
    public PuzzleRow () {
        row = new PuzzleTile[0];
    }
    public PuzzleRow (int count) {
        row = new PuzzleTile[count];
    }
}

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class PuzzleGrid : MonoBehaviour {

    public static PuzzleGrid instance;

    public Mesh[] meshes ;
    public Material[] materials;
    public Vector2Int gridSize;
    public PuzzleRow[] array;
    public int tileSize;

    public readonly int MESH_CHASM    = 0;
    public readonly int MESH_EMPTY    = 1;
    public readonly int MESH_NONE     = 2;

    private Vector2Int gridSize_old;
    private BoxCollider coll { get { if (!_coll) _coll = GetComponent<BoxCollider>(); return _coll; } }
    private BoxCollider _coll;

    // Start is called before the first frame update
    void Start() {

        if (array == null || array.Length == 0) {
            Debug.Log("Array is " + (array == null ? "null" : (array.ToString() + " with a length of " + array.Length)) + ", creating new array");
            array = new PuzzleRow[] { new PuzzleRow () };
        }

        instance = this;

        gridSize_old = gridSize;

    }

    private void Update() {

        gridSize.x = Math.Max(gridSize.x, 1);
        gridSize.y = Math.Max(gridSize.y, 0);

        if (!Application.isPlaying) {

            transform.position = -Make2DtoTopDown3D(gridSize - Vector2Int.one) * tileSize / 2f;
            coll.center = Make2DtoTopDown3D(gridSize - Vector2Int.one) * tileSize / 2f;
            coll.size = Make2DtoTopDown3D(gridSize) * tileSize + (Vector3.up);

            if (gridSize_old != gridSize)
                ResizeGrid();

            gridSize_old = gridSize;

        }

        //Print2DArray(array);

    }

    private void ResizeGrid() {

        if (array == null || array.Length == 0) {
            Debug.Log("Array is " + (array == null ? "null" : (array.ToString() + " with a length of " + array.Length)) + ", creating new array");
            array = new PuzzleRow[] { new PuzzleRow () };
        }

        //The largest x and y values between the new size and previous size;
        PuzzleRow[] newArray = new PuzzleRow[gridSize.x];

        for (int x = 0; x < newArray.Length; x++) {
            newArray[x] = new PuzzleRow(gridSize.y);
            for (int y = 0; y < newArray[x].row.Length; y++) {

                if (x < array.Length && y < array[x].row.Length && array[x].row[y])
                    //Reuse available tile if it is within existing bounds, and is not null
                    newArray[x].row[y] = array[x].row[y];
                else
                    //Otherwise create a new tile
                    newArray[x].row[y] = CreateTile(x, y);

            }
        }

        List<PuzzleTile> trash = new List<PuzzleTile>();
        for (int x = 0; x < array.Length; x++)
            for (int y = 0; y < array[x].row.Length; y++)
                if (x >= newArray.Length || y >= newArray[x].row.Length)
                    trash.Add(array[x].row[y]);

        Debug.Log(newArray.Length - array.Length + ", " + (newArray[0].row.Length - array[0].row.Length));
        Debug.Log("Destroying " + trash.Count + " tile(s)");
        //Empty trash
        while (trash.Count > 0) {
            if (trash[0]) {
                GameObject target = trash[0].gameObject;
                DestroyImmediate(target);
            }
            trash.RemoveAt(0);
        }

        array = newArray;

    }

    PuzzleTile CreateTile(int x, int y) {

        return CreateTile(new Vector2Int(x, y));

    }

    PuzzleTile CreateTile(Vector2Int pos) {

        PuzzleTile newTile = new GameObject("[-, -] Tile (NEW)").AddComponent<PuzzleTile>();

        newTile.transform.parent = transform.GetChild(0);

        newTile.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        newTile.gameObject.GetComponent<MeshFilter>().mesh = meshes[MESH_NONE];
        newTile.gameObject.GetComponent<MeshRenderer>().materials = materials;

        newTile.transform.localPosition = Make2DtoTopDown3D(pos * tileSize);

        newTile.pos = pos;
        newTile.grid = this;

        return newTile;

    }

    public static Vector3 Make2DtoTopDown3D (Vector2 v) {

        return new Vector3(v.x, 0, v.y);

    }

    [System.Obsolete("This took more than two minutes to make work with generic lists so... TODO: Fix this")]
    public void Print2DArray<T> (List<List<T>> array) {
        string log = "";
        for (int i = 0; i < array.Count; i++) {
            for (int j = 0; j < array[i].Count; j++)
                log += (array[i][j] + "\t");
            log += "\n";
        }

        Debug.Log(log);

    }

}