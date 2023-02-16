using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggeable : MonoBehaviour
{
    Vector2 difference = Vector2.zero; //initialize difference vector

    // private void OnMouseDown(){
    //     difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    // }

    // private void OnMouseDrag(){
    //     transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    // }

    // private void OnMouseMove(){
        
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
