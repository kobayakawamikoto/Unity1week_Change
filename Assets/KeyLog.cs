using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ブランチテスト

public class KeyLog : MonoBehaviour
{
    public GameObject nextDirectionObj;
    EnumNextDirectionScript enumNextDirectionScript;
    GameObject inputHandlerObj;
    InputHandler inputHandler;
    int cacheOldCommands = 0;
    [SerializeField]
    private TextMeshProUGUI KeyCommandText;
    // Start is called before the first frame update
    void Start()
    {
        enumNextDirectionScript = nextDirectionObj.GetComponent<EnumNextDirectionScript>();
        inputHandlerObj = GameObject.Find("InputHandler");
        inputHandler = inputHandlerObj.GetComponent<InputHandler>();
        KeyCommandText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.saveNextDirections.Count != cacheOldCommands)
        {
            KeyCommandText.text = "";
            for (int i = 0; i < inputHandler.saveNextDirections.Count; i++)
            {
                Debug.Log(inputHandler.saveNextDirections[i]);
                //KeyCommandText.text = "↑";
                if(inputHandler.saveNextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Forward)
                {
                    KeyCommandText.text += "↑\n";
                }
                else if (inputHandler.saveNextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Backward)
                {
                    KeyCommandText.text += "↓\n";
                }
                else if (inputHandler.saveNextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Left)
                {
                    KeyCommandText.text += "←\n";
                }
                else if (inputHandler.saveNextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Right)
                {
                    KeyCommandText.text += "→\n";
                }
            }
        }
        cacheOldCommands = inputHandler.saveNextDirections.Count;
    }
}
