using UnityEngine;
using System.Collections;

public class PipeController : MonoBehaviour
{
    public GameController baseController;
    public int perfectAngle;
    public bool isMatched;
    public bool isInFieldOfView;
    public bool angleFix;
    private int rotationAngle; // Required for safety in operation purposes.
    private bool isEmphasizeOn, isEmphasizing;
    private Vector3 originScale = new Vector3(0.3933388f, 0.3933388f, 0.3933388f);
    private Vector3 emphasizedScale = new Vector3(0.5933388f , 0.5933388f , 0.5933388f);
    private float emphasizingSpeed = 25.00f;

    // Use this for initialization
    void Start () {
        rotationAngle = Mathf.Abs( (int) transform.rotation.eulerAngles.z );
        emphasizedScale = new Vector3(transform.localScale.x * emphasizedScale.x / originScale.x, transform.localScale.y * emphasizedScale.y / originScale.y, transform.localScale.z * emphasizedScale.z / originScale.z);
        originScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isEmphasizeOn)
            if (isEmphasizing)
                if (transform.localScale == emphasizedScale) isEmphasizing = false;
                else transform.localScale = Vector3.Lerp(transform.localScale, emphasizedScale, emphasizingSpeed * Time.fixedDeltaTime);
            else
                if (transform.localScale == originScale) isEmphasizeOn = false;
                else transform.localScale = Vector3.Lerp(transform.localScale, originScale, emphasizingSpeed * Time.fixedDeltaTime);

        if (!isMatched /* Required for peformance improvement.*/ && ((rotationAngle == perfectAngle) || (angleFix && ((rotationAngle / 90) % 2) == 0)))
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
                gameObject.transform.Rotate(new Vector3(0, 0, ((angleFix) ? 90.00f : 180.00f) ));
                if (rotationAngle + ((angleFix) ? 90 : 180) >= 360) rotationAngle -= ((angleFix) ? 90 : 180); else rotationAngle += ((angleFix) ? 90 : 180);
                //if (GameController.gameMode == GameController.GAME_MODE_CLASSIC) baseController.pipe_counter--;
                break;
            }
    }

    void EmphasizePipe()
    {
        isEmphasizeOn = isEmphasizing = true;
    }
}
