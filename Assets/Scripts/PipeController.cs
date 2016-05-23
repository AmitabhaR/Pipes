using UnityEngine;
using System.Collections;

public class PipeController : MonoBehaviour
{
    public GameController baseController;
    public int perfectAngle;
    public bool isMatched;
    public bool isInFieldOfView;
    private int rotationAngle; // Required for safety in operation purposes.
	// Use this for initialization
	void Start () {
        rotationAngle = Mathf.Abs( (int) transform.rotation.eulerAngles.z );
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (rotationAngle == perfectAngle)
            foreach (GameController.SpriteEntry spritePack in baseController.spriteStack)
                if (spritePack.baseSprite == gameObject.GetComponent<SpriteRenderer>().sprite)
                {
                    baseController.pipe_counter++; // Increment the pipe counter in the game controller.
                    gameObject.GetComponent<SpriteRenderer>().sprite = spritePack.changeSprite;
                    isMatched = true;
                    //if (GameController.gameMode == GameController.GAME_MODE_CLASSIC) baseController.gameObject.SendMessage("RandomReset");
                    break;
                }
    }

#if (UNITY_STANDALONE || UNITY_EDITOR)
    void OnMouseDown()
    {
#elif (UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA)
    void OnTriggerEnter2D(Collider2D baseCollider)
    {
        if (baseCollider.gameObject.tag == "Input")
        {
#endif
        if (!isMatched && isInFieldOfView)
        {
            transform.Rotate(new Vector3(0, 0, 90.00f));
            if (rotationAngle == 270) rotationAngle = 0;
            else rotationAngle += 90;
        }

#if ((UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA) && !UNITY_EDITOR)
         baseCollider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
#endif
    }

    void RandomReset()
    {
        isMatched = false;
        foreach (GameController.SpriteEntry spritePack in baseController.spriteStack)
            if (spritePack.changeSprite == gameObject.GetComponent<SpriteRenderer>().sprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = spritePack.baseSprite;
                gameObject.transform.Rotate(new Vector3(0, 0, 180.00f));
                if (rotationAngle + 180 >= 360) rotationAngle -= 180; else rotationAngle += 180;
                //if (GameController.gameMode == GameController.GAME_MODE_CLASSIC) baseController.pipe_counter--;
                break;
            }
    }
}
