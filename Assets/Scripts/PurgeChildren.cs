using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PurgeChildren : MonoBehaviour {
    public bool purge;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (purge) {
            purge = false;
            while (transform.childCount > 0)
                DestroyImmediate(transform.GetChild(0).gameObject);
            try {
                transform.GetComponentInParent<PuzzleGrid>().gridSize = Vector2Int.right;
            } catch { };
        }

    }

}
