using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OffsetScrolling : MonoBehaviour {
    // public float scrollSpeed = 1;

    
    
    private Renderer renderer;
    private Vector2 oldPos;
    private Vector2 pos;
    private float currX;
    private float currY;
    private float preTime;
    private List<Vector2> positions;

    void Start () {
        renderer = GetComponent<Renderer>();
        positions = new List<Vector2>();
        pos = new Vector2(0,0);
        currX = 0;
        currY = 0;
        preTime = 0;
    }

    void Update () {
        // if (Input.GetMouseButtonDown(0) && chargeMeter.value == 100) {
        //     pos = Input.mousePosition;
        //     pos.Set(pos.x - Screen.width/2, pos.y - Screen.height/2);
        //     if (pos.y > 0) {
        //         pos.Set(pos.x, 0);
        //     }
        // }

        // float deltat = Time.time - preTime;

        // // Debug.Log((pos.x - Screen.width/2) + " " + (pos.y - Screen.height/2));
        // currX = GameManager.stopped ? currX : Mathf.Repeat(currX + pos.normalized.x * deltat * 0.3f, 1);
        // currY = GameManager.stopped ? currY : Mathf.Repeat(currY + pos.normalized.y * deltat * 0.3f, 1);

        // preTime = Time.time;

        // // Debug.Log(x + " " + y);
        // Vector2 offset = new Vector2 (currX, currY);
        // renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}