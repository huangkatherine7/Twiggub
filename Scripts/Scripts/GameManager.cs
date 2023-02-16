using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;  


public static class Constants { 
    public const float GAME_SPEED = 5;
    public const float ROOT_POS_SCALE = 1.5f;
    public const float ROOT_SIZE_SCALE = 2;
    public const float NODULE_SIZE_SCALE = 3.5f;
    public const float SWITCH_NODULE_POS_SCALE = 2.7f;
    public const float CAT_SIZE_SCALE = 3;
    public const float ROCK_SIZE_SCALE = 6;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject nodulePrefab;
    [SerializeField] public GameObject switchNodulePrefab;
    [SerializeField] public GameObject stickPrefab;
    [SerializeField] public GameObject selectionIndicator;

    //From Testing class
    [SerializeField] public static List<Transform> points; //arraylist
    public static List<(Transform, Vector2)> switchPoints;
    private float preTime;
    public static float score;
    public static int numEnemies = 0;

    private Vector2 mousePos;
    private Vector2 moveDir;
    private float backgrundOffsetX;
    private float backgrundOffsetY;

    public static bool stopped = false;
    public static bool dead = false;
    public TMP_Text scoreText;  
    public TMP_Text endScoreText;  
    public Slider chargeMeter;  
    public GameObject background;
    public GameObject startScreen;
    public GameObject endScreen; 
    public GameObject indicatorArrow;
    public GameObject rootT1;
    public GameObject rootT;

    public AudioSource audio;

    public AudioClip growthSound;
    public AudioClip splitSound;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        endScreen.transform.SetPositionAndRotation(new Vector3(10000,10000, 0), endScreen.transform.rotation);
        points = new List<Transform>();
        switchPoints = new List<(Transform, Vector2)>();
        endScoreText.text = "";
        preTime = 0;
        numEnemies = 0;
        score = 0;
        stopped = false;
        dead = false;
        mousePos = Vector2.zero;
        moveDir = Vector2.zero;

        this.rootT = Instantiate(stickPrefab, new Vector2(0, 1.5f), Quaternion.identity);
        this.rootT.transform.localScale += new Vector3(Constants.ROOT_SIZE_SCALE, Constants.ROOT_SIZE_SCALE,0);
        points.Add(this.rootT.transform);

        this.rootT1 = Instantiate(stickPrefab, new Vector2(0, 4), Quaternion.identity);
        this.rootT1.transform.localScale += new Vector3(Constants.ROOT_SIZE_SCALE, Constants.ROOT_SIZE_SCALE,0);
        points.Add(this.rootT1.transform);

    }

    // Update is called once per frame
    void Update()
    {
        selectionIndicator.transform.SetPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);

        float deltat = Time.time - preTime;

        // Update Score text and charge
        scoreText.text = "Score: " + (int) score;
        if (chargeMeter.value < 100) {
            chargeMeter.value = chargeMeter.value + 10 * deltat * Constants.GAME_SPEED;
        } else {
            stopped = true;
        }

        // Centering mouse and moving indicator arrow

        mousePos = Input.mousePosition;

        mousePos.Set(mousePos.x - Screen.width/2, mousePos.y - Screen.height/2);
        mousePos.Set(mousePos.normalized.x, mousePos.normalized.y);

        if (Input.GetMouseButton(1) || Input.GetKeyDown("space")) {
            Vector2 adjMousePos = new Vector2(mousePos.x, mousePos.y);

            (Transform, Vector2) closest = (null, Vector2.zero); 
            foreach((Transform, Vector2) tup in switchPoints) {
                Vector3 tupPos = tup.Item1.position;
                if (closest.Item1 == null || (adjMousePos - new Vector2(closest.Item1.position.x, closest.Item1.position.y)).magnitude > (adjMousePos - new Vector2(tupPos.x, tupPos.y)).magnitude) {
                    closest = tup;
                }
            }
            if (closest.Item1 != null) {
                selectionIndicator.transform.SetPositionAndRotation(closest.Item1.position, Quaternion.identity);
            }
        }

        // Handle right-click release input
        if((Input.GetMouseButtonUp(1) || Input.GetKeyUp("space")) && chargeMeter.value == 100){


            Vector2 adjMousePos = new Vector2(mousePos.x, mousePos.y);

            (Transform, Vector2) closest = (null, Vector2.zero); 
            foreach((Transform, Vector2) tup in switchPoints) {
                Vector3 tupPos = tup.Item1.position;
                if (closest.Item1 == null || (adjMousePos - new Vector2(closest.Item1.position.x, closest.Item1.position.y)).magnitude > (adjMousePos - new Vector2(tupPos.x, tupPos.y)).magnitude) {
                    closest = tup;
                }
            }
            if (closest.Item1 != null) {
                audio.PlayOneShot(splitSound, 7);

                switchPoints.Remove(closest);
                points.Remove(closest.Item1);

                Transform pos = closest.Item1;
                Vector2 dir = closest.Item2;
                backgrundOffsetX -= Mathf.Repeat(pos.position.x, 1);
                backgrundOffsetY -= Mathf.Repeat(pos.position.y, 1);

                Debug.Log(pos.position);

                foreach (Transform point in points) {
                    point.SetPositionAndRotation(new Vector3(point.position.x - pos.position.x, point.position.y - pos.position.y, 0), point.rotation);
                }

                Destroy(closest.Item1.gameObject);
            }
        }

        // Clamping inputs
        if (mousePos.y > -0.3f) {
            mousePos.Set(Mathf.Sqrt(0.81f) * (mousePos.x > 0 ? 1 : -1), -0.3f);
        }
        if (Mathf.Abs(mousePos.x) < 0.2f) {
            mousePos.Set(0.2f * (mousePos.x > 0 ? 1 : -1), -Mathf.Sqrt(0.96f));
        }   

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, mousePos * -1);
        rotation.eulerAngles += new Vector3(0,0,-20);

        indicatorArrow.transform.SetPositionAndRotation(Vector3.zero, rotation);

        // Handle left-click input
        if(Input.GetMouseButtonDown(0) && chargeMeter.value == 100){

            //audio.PlayOneShot(splitSound, 2);

            stopped = false;
            chargeMeter.value = 0;

            moveDir = mousePos;

            GameObject root = Instantiate(stickPrefab, new Vector2(moveDir.x * Constants.ROOT_POS_SCALE, moveDir.y * Constants.ROOT_POS_SCALE), Quaternion.Euler(0,0, -Mathf.Atan(moveDir.x/moveDir.y) * 180 / Mathf.PI));
            root.transform.localScale += new Vector3(Constants.ROOT_SIZE_SCALE, Constants.ROOT_SIZE_SCALE,0);
            GameObject mirroot = Instantiate(stickPrefab, new Vector2(-moveDir.x * Constants.ROOT_POS_SCALE, moveDir.y * Constants.ROOT_POS_SCALE), Quaternion.Euler(0,0, Mathf.Atan(moveDir.x/moveDir.y) * 180 / Mathf.PI));
            mirroot.transform.localScale += new Vector3(Constants.ROOT_SIZE_SCALE, Constants.ROOT_SIZE_SCALE,0);
           
            points.Add(root.transform);
            points.Add(mirroot.transform);

            GameObject nodule = Instantiate(nodulePrefab, new Vector3(0,0,10), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            nodule.transform.localScale += new Vector3(Constants.NODULE_SIZE_SCALE,Constants.NODULE_SIZE_SCALE, 10);
            GameObject switchNodule = Instantiate(switchNodulePrefab, new Vector3(-moveDir.x * Constants.SWITCH_NODULE_POS_SCALE, moveDir.y * Constants.SWITCH_NODULE_POS_SCALE, 10), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            switchNodule.transform.localScale += new Vector3(Constants.NODULE_SIZE_SCALE,Constants.NODULE_SIZE_SCALE, 10);

            points.Add(nodule.transform);
            points.Add(switchNodule.transform);
            switchPoints.Add((switchNodule.transform, new Vector2(-moveDir.x, moveDir.y)));

            audio.PlayOneShot(growthSound, 3);
        }

        // Make everything move

        // Move the background

        if (startScreen.transform.position.y < Screen.height * 1.6) {
            startScreen.transform.Translate(new Vector2(0, 50f * deltat * Constants.GAME_SPEED));

            backgrundOffsetY = stopped ? backgrundOffsetY : backgrundOffsetY - 0.06f * deltat * Constants.GAME_SPEED;

            Vector2 offset = new Vector2 (backgrundOffsetX, backgrundOffsetY);
            background.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
        else {                    
            backgrundOffsetX = stopped ? backgrundOffsetX : backgrundOffsetX + moveDir.x * 0.03f * deltat * Constants.GAME_SPEED;
            backgrundOffsetY = stopped ? backgrundOffsetY : backgrundOffsetY + moveDir.y  * 0.03f * deltat * Constants.GAME_SPEED;

            Vector2 offset = new Vector2 (backgrundOffsetX, backgrundOffsetY);
            background.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

        }


        



        // Move all the objects
        float xTrans = stopped ? 0 : moveDir.x * 0.3f * deltat * Constants.GAME_SPEED;
        float yTrans = stopped ? 0 : moveDir.y * 0.3f * deltat * Constants.GAME_SPEED;

        score -= yTrans;

        foreach (Transform point in points) {
            point.SetPositionAndRotation(new Vector3(point.position.x - xTrans, point.position.y - yTrans, 0), point.rotation);
        }

        for (int i = 0; i < switchPoints.Count; i++) {
            (Transform, Vector2) tup = switchPoints[i];
            if (tup.Item1.position.y > 6) {
                switchPoints.Remove(tup);
                points.Remove(tup.Item1);
                Destroy(tup.Item1.gameObject);
                i--;
            }
        }

        for (int i = 0; i < points.Count; i++) {
            Transform point  = points[i];
             if (point.transform.position.x < -16 || point.transform.position.x > 16 || point.transform.position.y > 10 || point.transform.position.y < -16){
                if (point.gameObject.tag == "Enemy") numEnemies--;
                foreach ((Transform, Vector2) tup in switchPoints) {
                    if (tup.Item1 == points[i]) {
                        switchPoints.Remove(tup);
                        break;
                    }
                }
                points.Remove(point.transform);
                Destroy(point.gameObject);
                i--;
             }
         }

         // Death stuff
         if (dead) {
            stopped = true;
            endScreen.transform.SetPositionAndRotation(new Vector3(Screen.width/2,Screen.height/2, 0), endScreen.transform.rotation);
            endScoreText.text = "" + (int) score;
            audio.Stop();
            dead = false;

            foreach((Transform, Vector2) tup in switchPoints) {
                Destroy(tup.Item1.gameObject);
            }

            foreach (Transform point in points) {
                Destroy(point.gameObject);
            }

            points = new List<Transform>();
            switchPoints = new List<(Transform, Vector2)>();

            StartCoroutine("death");
         }

        preTime = Time.time;
    }


    private IEnumerator death()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine("starting");
        SceneManager.LoadScene("Start");
        dead = false;
        stopped = false; 
    }

    private IEnumerator starting(){
        yield return new WaitForSeconds((float)1);
        Start();
    }


}