using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InputHandler : MonoBehaviour
{

    public GameObject nextDirectionObj;
    EnumNextDirectionScript enumNextDirectionScript;
    EnumNextDirectionScript ENDupArrow, ENDdownArrow, ENDleftArrow, ENDrightArrow;
    public GameObject actor;
    Animator anim;
    public Camera camera;
    Command keyQ, keyW, keyE, upArrow, downArrow, leftArrow, rightArrow;
    public List<Command> oldCommands = new List<Command>();
    public List<EnumNextDirectionScript> saveNextDirections = new List<EnumNextDirectionScript>();

    private GameObject _sliderCanvas;
    private Slider _staminaSlider;

    Coroutine replayCoroutine;
    bool shouldStartReplay;
    bool isReplaying;
    private bool isDeath;
    private bool isClear;

    public bool IsDeath
    {
        get { return isDeath; }
        set { isDeath = value; }
    }
    public bool IsClear
    {
        get { return isClear; }
        set { isClear = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        keyQ = new PerformJump();
        keyW = new PerformKick();
        keyE = new PerformPunch();
        upArrow = new PerformForward();
        downArrow = new PerformBackward();
        leftArrow = new PerformLeft();
        rightArrow = new PerformRight();
        anim = actor.GetComponent<Animator>();
        enumNextDirectionScript = nextDirectionObj.GetComponent<EnumNextDirectionScript>();
        ENDupArrow = new EnumNextDirectionScript();
        ENDupArrow.nextDirection = EnumNextDirectionScript.NextDirection.Forward;
        ENDdownArrow = new EnumNextDirectionScript();
        ENDdownArrow.nextDirection = EnumNextDirectionScript.NextDirection.Backward;
        ENDleftArrow = new EnumNextDirectionScript();
        ENDleftArrow.nextDirection = EnumNextDirectionScript.NextDirection.Left;
        ENDrightArrow = new EnumNextDirectionScript();
        ENDrightArrow.nextDirection = EnumNextDirectionScript.NextDirection.Right;
        _sliderCanvas = GameObject.Find("SliderCanvas");
        Slider[] sliders = _sliderCanvas.GetComponentsInChildren<Slider>();
        // スライダーが1つ以上見つかった場合
        if (sliders.Length > 0)
        {
            foreach (Slider sliderComponent in sliders)
            {
                // スライダーの名前が "Slider2" の場合
                if (sliderComponent.gameObject.name == "StaminaSlider")
                {
                    // 該当のスライダーをdesiredSliderに代入する
                    _staminaSlider = sliderComponent;
                    break; // 必要なスライダーが見つかったのでループを抜ける
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReplaying)
        {
            HandleInput();
        }

        StartReplay();
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopCoroutine(replayCoroutine);
        }
        if(isClear || isDeath)
        {
            Vector3 offset = new Vector3(0, 1, 2);
            camera.transform.position = actor.transform.position + offset;
            camera.transform.LookAt(actor.transform);
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyQ.Execute(anim);
            oldCommands.Add(keyQ);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            keyW.Execute(anim);
            oldCommands.Add(keyW);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            keyE.Execute(anim);
            oldCommands.Add(keyE);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //upArrow.Execute(anim);
            oldCommands.Add(upArrow);
            //enumNextDirectionScript.nextDirection = EnumNextDirectionScript.NextDirection.Forward;
            saveNextDirections.Add(ENDupArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //downArrow.Execute(anim);
            oldCommands.Add(downArrow);
            //enumNextDirectionScript.nextDirection = EnumNextDirectionScript.NextDirection.Backward;
            saveNextDirections.Add(ENDdownArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //leftArrow.Execute(anim);
            oldCommands.Add(leftArrow);
            //enumNextDirectionScript.nextDirection = EnumNextDirectionScript.NextDirection.Left;
            saveNextDirections.Add(ENDleftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //rightArrow.Execute(anim);
            oldCommands.Add(rightArrow);
            //enumNextDirectionScript.nextDirection = EnumNextDirectionScript.NextDirection.Right;
            saveNextDirections.Add(ENDrightArrow);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldStartReplay = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoLastCommand();
        }
    }

    void UndoLastCommand()
    {
        if (oldCommands.Count > 0)
        {
            //Command c = oldCommands[oldCommands.Count - 1];
            //EnumNextDirectionScript e = saveNextDirections[saveNextDirections.Count - 1];
            //c.Execute(anim);
            oldCommands.RemoveAt(oldCommands.Count - 1);
            saveNextDirections.RemoveAt(saveNextDirections.Count - 1);
        }
    }

    void StartReplay()
    {
        if (shouldStartReplay && oldCommands.Count > 0)
        {
            shouldStartReplay = false;
            if (replayCoroutine != null)
            {
                StopCoroutine(replayCoroutine);
            }
            replayCoroutine = StartCoroutine(ReplayCommands());
        }
    }

    IEnumerator ReplayCommands()
    {
        isReplaying = true;

        for (int i = 0; i < oldCommands.Count; i++)
        {
            if(isClear || isDeath)
            {
                break;
            }
            
            // 効果音
            GetComponent<AudioSource>().pitch = Random.Range(1.05f, 0.95f);
            GetComponent<AudioSource>().Play();
            enumNextDirectionScript.nextDirection = saveNextDirections[i].nextDirection;
            Debug.Log(saveNextDirections[i].nextDirection);
            oldCommands[i].Execute(anim);
            _staminaSlider.value -= 0.1f;
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
    }
}
