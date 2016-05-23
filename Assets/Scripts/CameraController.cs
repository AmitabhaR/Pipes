using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public bool canMove = true;

	// Update is called once per frame
	void Update ()
    {
        //if (!Application.isMobilePlatform && Application.platform != RuntimePlatform.WSAPlayerX64 && Application.platform != RuntimePlatform.WSAPlayerX86)
            if ((((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.WSAPlayerX64 || Application.platform == RuntimePlatform.WSAPlayerX86) && Input.GetKeyDown(KeyCode.RightArrow) && GameController.gameMode == GameController.GAME_MODE_CLASSIC) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.x >= 1)) && canMove)
            {
                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Pipe"))
                    if (gameObject.GetComponent<PipeController>() != null)
                    {
                        gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        gameObject.GetComponent<PipeController>().isInFieldOfView = false;
                    }

                GameObject.FindWithTag("FieldOfView").GetComponent<BoxCollider2D>().enabled = true;
                transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
            }
          //  else;
        //else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.x >= 5) 
          
        if (Input.GetKeyDown(KeyCode.C)) { GameController.gameMode = GameController.GAME_MODE_CLASSIC; Application.LoadLevel("ClassicMode"); }
        else if (Input.GetKeyDown(KeyCode.S)) { GameController.gameMode = GameController.GAME_MODE_SURPRISE; Application.LoadLevel("SurpriseMode"); }
        else if (Input.GetKeyDown(KeyCode.E)) { GameController.difficultyLevel = GameController.DIFFICULTY_LEVEL_NORMAL; Application.LoadLevel(Application.loadedLevelName); }
        else if (Input.GetKeyDown(KeyCode.H)) { GameController.difficultyLevel = GameController.DIFFICULTY_LEVEL_HARD; Application.LoadLevel(Application.loadedLevelName); }
	}
}
