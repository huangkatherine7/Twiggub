using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject rockPrefab; //serialize field makes object accessible via GUI?
    [SerializeField] public GameObject octocatPrefab;

    [SerializeField] public GameObject boctocatPrefab;
    [SerializeField] public GameObject toctocatPrefab;
    [SerializeField] public GameObject rock2Prefab;


    [SerializeField] public float rockInterval = 50f; //number of seconds it takes for enemy to spawn
    [SerializeField] public float octocatInterval = 5f; 
    [SerializeField] public float maxEnemies = 10; 


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(rockPrefab, 0));
        StartCoroutine(spawnEnemy(octocatPrefab, 1));

        StartCoroutine(spawnEnemy(toctocatPrefab, 1));
        StartCoroutine(spawnEnemy(boctocatPrefab, 1));
        StartCoroutine(spawnEnemy(rock2Prefab, 0));

    }

    private IEnumerator spawnEnemy(GameObject enemy, int id) {
        float interval = 0f;
        if (id == 0) {
            interval = rockInterval;
        } else if (id == 1){
            interval = octocatInterval;
        }
        yield return new WaitForSeconds(interval);
        if (GameManager.numEnemies < maxEnemies) {
            GameManager.numEnemies++;
            Vector3 left = new Vector3(UnityEngine.Random.Range(-16f, -5), UnityEngine.Random.Range(-10, 10f), 0);
            Vector3 center = new Vector3(UnityEngine.Random.Range(-5f, 5), UnityEngine.Random.Range(-14f, -5f), 0);
            Vector3 right = new Vector3(UnityEngine.Random.Range(5f, 16f), UnityEngine.Random.Range(-10f, 10f), 0);
            
            Vector3[] locations = {left, center,center,center,right};

            GameObject newEnemy = Instantiate(enemy, locations[UnityEngine.Random.Range(0,locations.Length)], Quaternion.identity);


            float scale = UnityEngine.Random.Range((id == 0 ? Constants.ROCK_SIZE_SCALE : Constants.CAT_SIZE_SCALE) / 2, (id == 0 ? Constants.ROCK_SIZE_SCALE : Constants.CAT_SIZE_SCALE) * 2);
            newEnemy.transform.localScale += new Vector3(scale, scale,0);
            GameManager.points.Add(newEnemy.transform);
            //Quaternion identity is the rotation of the new object
        }
        StartCoroutine(spawnEnemy(enemy, id));


    }

    // Update is called once per frame
    void Update()
    {
        rockInterval = (float) (1.5 / Math.Sqrt(GameManager.score + 1)) + UnityEngine.Random.Range(0f, 3);
        octocatInterval = (float) (1.0 / Math.Sqrt(GameManager.score + 1)) + UnityEngine.Random.Range(0f, 3);
        maxEnemies = 14f + (float)Math.Sqrt(GameManager.score + 1) * 2;
        //Debug.Log(GameManager.numEnemies);
    }
}
