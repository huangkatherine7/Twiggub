using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    private LineRenderer lr;
    private List<Transform> points;

    private void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    public void SetUpLine(List<Transform> points){ //points that line renderer will use to draw lines
        lr.positionCount = points.Count;
        this.points = points;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // for(int i = 0; i<points.Count; i++) {
        //     lr.SetPosition(i, points[i].position);
        // }
    }
}
