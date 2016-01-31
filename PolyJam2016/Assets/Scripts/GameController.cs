using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum DudeEliminationType
{
    Runner,
    KilledOk,
    KilledNotOk
}

public class GameController : MonoBehaviour {

    public GameObject[] dudes;

    public int requaierdNumberOfDudes = 10;

    public float startWait = 2;
    public float spawnWait = 10;
    /// <summary>
    /// Linear equation responsible for spawning dudes (z = a * x + b) - x limits for rand
    /// </summary>
    public Vector2 spawnXLimits;
    /// <summary>
    /// Linear equation responsible for spawning dudes (z = a * x + b) - a param
    /// </summary>
    public float a;
    /// <summary>
    /// Linear equation responsible for spawning dudes (z = a * x + b) - b param
    /// </summary>
    public float b;

    private int dudesRunningAway;
    private int dudesKilledOK;
    private int dudesKilledNotOK;

    private bool running;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnDudes());

        dudesRunningAway = 0;
        dudesKilledOK = 0;
        dudesKilledNotOK = 0;

        running = true;
    }

    IEnumerator SpawnDudes()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            GameObject hazard = dudes[Random.Range(0, dudes.Length)];
            float randomX = Random.Range(spawnXLimits.x, spawnXLimits.y);
            Vector3 spawnPosition = new Vector3(randomX, 2, randomX * a + b);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(hazard, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnWait);
            if (!running)
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        // TODO ending and wining
        if(!running && Input.GetKey(KeyCode.Space))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void GameOver()
    {
        string msg;
        if (dudesKilledOK > requaierdNumberOfDudes)
        {
            msg = "You won! Sun will rise tomorrow!";
        }
        else
        {
            msg = "You lose! Sun will not rise tomorrow!";
        }
        msg += "\nPress space to restart!";
        GameObject.FindGameObjectWithTag("EndMessage").GetComponent<Text>().text = msg;
        running = false;
    }

    public void addKilledOrRunningAwayDude(DudeEliminationType dudeType)
    {
        switch (dudeType)
        {
            case DudeEliminationType.Runner:
                ++dudesRunningAway;
                break;
            case DudeEliminationType.KilledOk:
                ++dudesKilledOK;
                break;
            case DudeEliminationType.KilledNotOk:
                ++dudesKilledNotOK;
                break;
        }
        UpdateGUI();
    }

    void UpdateGUI()
    {
        //TODO
    }
}
