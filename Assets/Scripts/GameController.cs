using UnityEngine;
//using UnityEditor;
using System.IO;
using System.Collections;

public class GameController : MonoBehaviour
{
    public struct SpriteEntry
    {
       public Sprite baseSprite;
       public Sprite changeSprite;
    };

    public int maxPipeReset;
    public int pipe_counter = 0;
    public int max_pipes;
    public int timeCounter;
    public int maxDelay;
    public int delayRate;
    public int maxWaves;
    public int roundsPerWave;
    public SpriteEntry[] spriteStack;
    public Sprite[] baseSprites;
    public Sprite[] changeSprites;
    public Vector3 levelLoadPosition;
    public int TotalPipes;
    public static int levelNumber = 5, gameMode = GAME_MODE_SURPRISE, difficultyLevel = DIFFICULTY_LEVEL_NORMAL;
    private int delayCounter;
    private int roundCounter;
    private int waveCounter;
    private int waitTime;

    public const int GAME_MODE_CLASSIC = 0xFA,
                     GAME_MODE_SURPRISE = 0xFF,
                     DIFFICULTY_LEVEL_NORMAL = 0x2F,
                     DIFFICULTY_LEVEL_HARD = 0xAF;

    void Awake( )
    {
        spriteStack = new SpriteEntry[baseSprites.Length];

        for (int cntr = 0; cntr < baseSprites.Length; cntr++)
        {
            SpriteEntry sprite;

            sprite.baseSprite = baseSprites[cntr];
            sprite.changeSprite = changeSprites[cntr];

            spriteStack[cntr] = sprite;
        }

        // Load our level into the game.
        GameObject gameObject = (GameObject)Resources.Load(((gameMode == GAME_MODE_CLASSIC) ? "Maps/ClassicMode/Level0" : "Maps/SurpriseMode/Level0") + levelNumber, typeof(GameObject));

        for (int cntr = 0; cntr < gameObject.transform.childCount; cntr++)
            if (gameObject.transform.GetChild(cntr).gameObject.GetComponent<PipeController>() != null) gameObject.transform.GetChild(cntr).gameObject.GetComponent<PipeController>().baseController = this;

        Instantiate(gameObject, levelLoadPosition, Quaternion.Euler(0, 0, 0));
        TotalPipes = gameObject.transform.childCount;
        
        TypeDefinations.LevelData levelData = LevelList.FindLevelWithId(gameMode, levelNumber);
        timeCounter = levelData.levelTime;

        if (gameMode == GAME_MODE_SURPRISE)
        {
            maxWaves = ((TypeDefinations.LevelSurpriseModeData)levelData).maxWaves;
            roundsPerWave = ((TypeDefinations.LevelSurpriseModeData)levelData).roundsPerWave;
            maxPipeReset = ((TypeDefinations.LevelSurpriseModeData)levelData).pipeResetCount;
            max_pipes = ((TypeDefinations.LevelSurpriseModeData)levelData).maxPipesOnEntry;
        }

        waitTime = timeCounter;
    }

    // Use this for initialization
    void Start()
    {
        if (!Application.isMobilePlatform) Destroy(GameObject.FindWithTag("Input"));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timeCounter == 0) Debug.Log("You loose!");

        if (pipe_counter == max_pipes)
            if (gameMode == GAME_MODE_CLASSIC) Debug.Log("You won!");
            else
                if (waveCounter == maxWaves) Debug.Log("You won!");
                else if (roundCounter == roundsPerWave) { waveCounter++; if (difficultyLevel == DIFFICULTY_LEVEL_HARD) maxPipeReset++; roundCounter = pipe_counter = 0; RandomReset(); }
                else { roundCounter++; if (difficultyLevel == DIFFICULTY_LEVEL_HARD && Random.Range(0,1) == 1 && ((TotalPipes - maxPipeReset) > 1)) maxPipeReset++; pipe_counter = 0; RandomReset(); }

        if (delayCounter >= maxDelay)
        {
            Debug.Log("Time : " + timeCounter--);
            delayCounter = 0;
        }
        else delayCounter += delayRate;

        if (Application.isMobilePlatform && Application.platform != RuntimePlatform.WSAPlayerX64 && Application.platform != RuntimePlatform.WSAPlayerX86)
            if (Input.touchCount > 0)
            {
                Touch curTouch = Input.GetTouch(0);

                if (curTouch.phase == TouchPhase.Began)
                {
                    GameObject.FindWithTag("Input").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(curTouch.position.x, curTouch.position.y, 10));
                    GameObject.FindWithTag("Input").GetComponent<BoxCollider2D>().enabled = true;
                }
            }
            else;
        else if (Application.platform == RuntimePlatform.WSAPlayerX64 || Application.platform == RuntimePlatform.WSAPlayerX86)
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.FindWithTag("Input").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                GameObject.FindWithTag("Input").GetComponent<BoxCollider2D>().enabled = true;
            }

        // Check if any of the object is in the field of view.
        if (GameObject.FindWithTag("FieldOfView").GetComponent<BoxCollider2D>().enabled)
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Pipe"))
                if (gameObject.GetComponent<PipeController>() != null && gameObject.GetComponent<PipeController>().isInFieldOfView)
                {
                    GameObject.FindWithTag("FieldOfView").GetComponent<BoxCollider2D>().enabled = false;
                    if (gameMode == GAME_MODE_CLASSIC)
                        foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Pipe"))
                            if (gameObj.GetComponent<PipeController>() != null)
                                if (!gameObject.GetComponent<PipeController>().isInFieldOfView) gameObj.GetComponent<BoxCollider2D>().enabled = false;
                    break;
                }
    }

    void FixedUpdate()
    {
     /*   if (delayCounter >= maxDelay)
        {
            Debug.Log("Time : " + timeCounter--);
            delayCounter = 0;
        }
        else delayCounter += delayRate;

        if (Application.isMobilePlatform && Application.platform != RuntimePlatform.WSAPlayerX64 && Application.platform != RuntimePlatform.WSAPlayerX86)
            if (Input.touchCount > 0)
            {
                Touch curTouch = Input.GetTouch(0);

                if (curTouch.phase == TouchPhase.Began)
                {
                    GameObject.FindWithTag("Input").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(curTouch.position.x, curTouch.position.y, 10));
                    GameObject.FindWithTag("Input").GetComponent<BoxCollider2D>().enabled = true;
                }
            }
            else;
        else if (Application.platform == RuntimePlatform.WSAPlayerX64 || Application.platform == RuntimePlatform.WSAPlayerX86)
            if (Input.GetMouseButtonDown(0))
            { 
                GameObject.FindWithTag("Input").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                GameObject.FindWithTag("Input").GetComponent<BoxCollider2D>().enabled = true;
            }

        // Check if any of the object is in the field of view.
        if (GameObject.FindWithTag("FieldOfView").GetComponent<BoxCollider2D>().enabled)
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Pipe"))
                if (gameObject.GetComponent<PipeController>() != null && gameObject.GetComponent<PipeController>().isInFieldOfView)
                {
                    GameObject.FindWithTag("FieldOfView").GetComponent<BoxCollider2D>().enabled = false;
                    if (gameMode == GAME_MODE_CLASSIC)
                        foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Pipe"))
                            if (gameObj.GetComponent<PipeController>() != null)  
                                    if (!gameObject.GetComponent<PipeController>().isInFieldOfView) gameObj.GetComponent<BoxCollider2D>().enabled = false;
                    break;
                }*/
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width / Screen.height * 100, Screen.width / Screen.height * 100), "Time : " + timeCounter);
        if (timeCounter <= 0) GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / Screen.height * 100, Screen.width / Screen.height * 100), "You Lost!");
    }

    GameObject findGameObjectWithId(int index)
    {
        int cntr = 0;

        foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Pipe")) if (cntr == index) return gameObj; else cntr++;

        return null;
    }

    void RandomReset()
    {
        int maxReset = /*(gameMode == GAME_MODE_CLASSIC) ? ((difficultyLevel == DIFFICULTY_LEVEL_HARD) ? Random.Range(-maxPipeReset, maxPipeReset) : Random.Range(0, maxPipeReset)) :*/ Random.Range(1, maxPipeReset + 1);
        int[] usedIndexes = new int[maxReset];

        for (int cntr = 0; cntr < usedIndexes.Length; cntr++) usedIndexes[cntr] = -1;

        // Add AI to random reset functionality.
        if (maxReset > 0)
            for (int cntr = 1; cntr <= maxReset; cntr++)
            {
            repeat_iteration:;
                int curIndex = 0;

                while (true)
                {
                    bool isSuccess = true;

                    curIndex = Random.Range(0, TotalPipes);

                    foreach (int index in usedIndexes) if (index == curIndex) { isSuccess = false; break; }

                    if (isSuccess) break;
                }

                GameObject selectedObject = findGameObjectWithId(curIndex);

                if (selectedObject != null && selectedObject.GetComponent<PipeController>() != null && selectedObject.GetComponent<PipeController>().isInFieldOfView)
                {
                    //selectedObject.GetComponent<PipeController>().isMatched = false;
                    selectedObject.SendMessage("RandomReset");
                    selectedObject.SendMessage("EmphasizePipe");
                    usedIndexes[cntr - 1] = curIndex;
                }
                else /*if (difficultyLevel == DIFFICULTY_LEVEL_HARD || gameMode == GAME_MODE_SURPRISE)*/ goto repeat_iteration;
            }

        /*if (gameMode == GAME_MODE_SURPRISE)*/ max_pipes = maxReset;
        timeCounter = (max_pipes * LevelList.FindLevelWithId(gameMode, levelNumber).levelTime / ((TypeDefinations.LevelSurpriseModeData)LevelList.FindLevelWithId(gameMode, levelNumber)).maxPipesOnEntry) + ((difficultyLevel == DIFFICULTY_LEVEL_NORMAL) ? ((max_pipes * 6 / ((TypeDefinations.LevelSurpriseModeData)LevelList.FindLevelWithId(gameMode, levelNumber)).maxPipesOnEntry) + 1) : 0); // Relative time calculation.
    }
}
