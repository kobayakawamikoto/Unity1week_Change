using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputHandler : MonoBehaviour
{

    public GameObject nextDirectionObj;
    EnumNextDirectionScript enumNextDirectionScript;
    EnumNextDirectionScript ENDupArrow, ENDdownArrow, ENDleftArrow, ENDrightArrow;
    public GameObject actor;
    Animator anim;
    Command keyQ, keyW, keyE, upArrow, downArrow, leftArrow, rightArrow;
    public List<Command> oldCommands = new List<Command>();
    public List<EnumNextDirectionScript> saveNextDirections = new List<EnumNextDirectionScript>();

    Coroutine replayCoroutine;
    bool shouldStartReplay;
    bool isReplaying;

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
        //Camera.main.GetComponent<CameraFollow360>().player = actor.transform;
        /*
         Animator anim = GetComponent<Animator>();
         keyQ = new Command(()=>{  anim.SetTrigger("isJumping"});
         */
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReplaying)
        {
            HandleInput();
        }
        else if (shouldStartReplay)
        {

        }
        StartReplay();
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
            Command c = oldCommands[oldCommands.Count - 1];
            EnumNextDirectionScript e = saveNextDirections[saveNextDirections.Count - 1];
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
            // Œø‰Ê‰¹
            GetComponent<AudioSource>().pitch = Random.Range(1.05f, 0.95f);
            GetComponent<AudioSource>().Play();
            enumNextDirectionScript.nextDirection = saveNextDirections[i].nextDirection;
            Debug.Log(saveNextDirections[i].nextDirection);
            oldCommands[i].Execute(anim);
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
    }
}
