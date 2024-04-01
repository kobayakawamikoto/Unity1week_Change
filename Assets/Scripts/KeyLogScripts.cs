using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyLogScripts : MonoBehaviour
{
    private GameObject _endObj;
    private EnumNextDirectionScripts _ends;
    private GameObject _inputHandlerObj;
    private InputHandlerScripts _ihs;
    private int _cacheOldCommandsCount = 0;
    private TextMeshProUGUI _KeyCommandText;
    // Start is called before the first frame update
    void Start()
    {
        _endObj = GameObject.Find("EnumNextDirectionObj");
        _ends = _endObj.GetComponent<EnumNextDirectionScripts>();

        _inputHandlerObj = GameObject.Find("InputHandler");
        _ihs = _inputHandlerObj.GetComponent<InputHandlerScripts>();

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
            if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScripts.NextDirection.Forward)
            {
                _KeyCommandText.text += "↑\n";
            }
            else if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScripts.NextDirection.Backward)
            {
                _KeyCommandText.text += "↓\n";
            }
            else if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScripts.NextDirection.Left)
            {
                _KeyCommandText.text += "←\n";
            }
            else if (_ihs.nextDirections[i].nextDirection == EnumNextDirectionScripts.NextDirection.Right)
            {
                _KeyCommandText.text += "→\n";
            }
        }

        _cacheOldCommandsCount = _ihs.nextDirections.Count;
    }
}
