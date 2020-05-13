using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBKey : MonoBehaviour {

    public KeyCode key;

    private Vector3 lerpTo;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        lerpTo = transform.position;
        lerpTo.y = !Input.GetKey(key) ? 0.5f : -0.25f;

        transform.position = Vector3.Lerp(transform.position, lerpTo, Time.deltaTime * 20);

    }

}