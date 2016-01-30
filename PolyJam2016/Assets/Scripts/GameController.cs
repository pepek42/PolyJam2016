using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject sun;
    public GameObject[] dudes;

    public float startWait = 2;
    public float spawnWait = 10;
    public Vector3 spawnValues;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnDudes());
    }

    IEnumerator SpawnDudes()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            GameObject hazard = dudes[Random.Range(0, dudes.Length)];
            Vector3 spawnPosition = new Vector3(spawnValues.x, spawnValues.y, Random.Range(0, spawnValues.z));
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(hazard, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }

    // Update is called once per frame
    void Update () {
        // TODO ending and wining
    }

    public void GameOver()
    {
        // TODO
        Debug.Log("Game over! Something ate your player in woods!");
    }
}
