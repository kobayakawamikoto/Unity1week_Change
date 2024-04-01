using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyLogScript : MonoBehaviour
{
    private GameObject _endObj;
    private EnumNextDirectionScript _ends;
    private GameObject _inputHandlerObj;
    private InputHandlerScript _ihs;
    private int _cacheOldCommandsCount = 0;
    private TextMeshProUGUI _KeyCommandText;
    // Start is called before the first frame update
    void Start()
    {
        _endObj = GameObject.Find("EnumNextDirectionObj");
        _ends = _endObj.GetComponent<EnumNextDirectionScript>();

        _inputHandlerObj = GameObject.Find("InputHandler");
        _ihs = _inputHandlerObj.GetComponent<InputHandlerScript>();

        _KeyCommandText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        PrintLog();
    }

    /*
     * 入力したキーを表示する
     */
    void PrintLog()
    {
        // キーを入力した後一度だけ処理が走る
        if (_ihs.nextDirections.Count == _cacheOldCommandsCount) return;

        _KeyCommandText.text = "";
        for (int i = 0; i < _ihs.nextDirections.Count; i++)
        {
            if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Forward)
            {
                _KeyCommandText.text += "↑\n";
            }
            else if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Backward)
            {
                _KeyCommandText.text += "↓\n";
            }
            else if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Left)
            {
                _KeyCommandText.text += "←\n";
            }
            else if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScript.NextDirection.Right)
            {
                _KeyCommandText.text += "→\n";
            }
        }

        _cacheOldCommandsCount = _ihs.nextDirections.Count;
    }
}
