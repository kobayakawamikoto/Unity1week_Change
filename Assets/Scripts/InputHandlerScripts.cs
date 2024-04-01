using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InputHandlerScripts : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextDirectionObj;
    [SerializeField]
    private GameObject _actor;
    [SerializeField]
    private Camera _camera;

    private Animator _anim;

    private Commands _upArrow, _downArrow, _leftArrow, _rightArrow;
    [System.NonSerialized] 
    public List<Commands> oldCommands = new List<Commands>();

    private EnumNextDirectionScripts _ends;
    private EnumNextDirectionScripts _endsUpArrow, _endsDownArrow, _endsLeftArrow, _endsRightArrow;
    [System.NonSerialized] 
    public List<EnumNextDirectionScripts> nextDirections = new List<EnumNextDirectionScripts>();

    private GameObject _sliderCanvas;
    private Slider _staminaSlider;

    Coroutine replayCoroutine;
    private bool shouldStartReplay;
    private bool isReplaying;

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
        _upArrow = new PerformForward();
        _downArrow = new PerformBackward();
        _leftArrow = new PerformLeft();
        _rightArrow = new PerformRight();
        _anim = _actor.GetComponent<Animator>();
        _ends = _nextDirectionObj.GetComponent<EnumNextDirectionScripts>();
        _endsUpArrow = new EnumNextDirectionScripts();
        _endsUpArrow.nextDirection = EnumNextDirectionScripts.NextDirection.Forward;
        _endsDownArrow = new EnumNextDirectionScripts();
        _endsDownArrow.nextDirection = EnumNextDirectionScripts.NextDirection.Backward;
        _endsLeftArrow = new EnumNextDirectionScripts();
        _endsLeftArrow.nextDirection = EnumNextDirectionScripts.NextDirection.Left;
        _endsRightArrow = new EnumNextDirectionScripts();
        _endsRightArrow.nextDirection = EnumNextDirectionScripts.NextDirection.Right;
        _sliderCanvas = GameObject.Find("SliderCanvas");
        Slider[] sliders = _sliderCanvas.GetComponentsInChildren<Slider>();
        // スライダーが1つ以上見つかった場合
        if (sliders.Length > 0)
        {
            foreach (Slider sliderComponent in sliders)
            {
                // スライダーの名前が "StaminaSlider" の場合
                if (sliderComponent.gameObject.name == "StaminaSlider")
                {
                    _staminaSlider = sliderComponent;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * ↑↓→←のコマンド入力
         */
        HandleInput();


        /*
         * リプレイ
         */
        StartReplay();
        
        /*
         * リトライ
         */
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /*
         * 死亡時またはクリア時のカメラ演出
         */
        if (isClear || isDeath || _staminaSlider.value <= 0)
        {
            Vector3 offset = new Vector3(0, 1, 2);
            _camera.transform.position = _actor.transform.position + offset;
            _camera.transform.LookAt(_actor.transform);
        }
    }

    /*
     * キャラの移動先と移動順番を決める処理
     * アンドゥと移動開始もあるよ
     */
    void HandleInput()
    {
        if (isReplaying) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // oldCommandsには_upArrow, _downArrow, _leftArrow, _rightArrowが入力した順番で追加されていく
            // oldCommandsを使ってキャラを入力された順番に移動させたり、アンドゥしたりする
            oldCommands.Add(_upArrow);
            // nextDirectionsには_endsUpArrow, _endsDownArrow, _endsLeftArrow, _endsRightArrowが入力した順番で追加されていく
            // nextDirectionsを使ってキャラの回転方向を決めています
            nextDirections.Add(_endsUpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            oldCommands.Add(_downArrow);
            nextDirections.Add(_endsDownArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            oldCommands.Add(_leftArrow);
            nextDirections.Add(_endsLeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            oldCommands.Add(_rightArrow);
            nextDirections.Add(_endsRightArrow);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // リプレイ
            shouldStartReplay = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // アンドゥ
            UndoLastCommand();
        }
    }

    /*
     * 入力したキーをアンドゥする処理
     * リストの最後を消しています
     */
    void UndoLastCommand()
    {
        if (oldCommands.Count > 0)
        {
            oldCommands.RemoveAt(oldCommands.Count - 1);
            nextDirections.RemoveAt(nextDirections.Count - 1);
        }
    }

    void StartReplay()
    {
        if (shouldStartReplay && oldCommands.Count > 0)
        {
            shouldStartReplay = false;
            replayCoroutine = StartCoroutine(ReplayCommands());
        }
    }

    IEnumerator ReplayCommands()
    {
        isReplaying = true;

        for (int i = 0; i < oldCommands.Count; i++)
        {
            if(isClear || isDeath || _staminaSlider.value <= 0)
            {
                break;
            }
            
            // 効果音ピッチ調整/再生
            GetComponent<AudioSource>().pitch = Random.Range(1.05f, 0.95f);
            GetComponent<AudioSource>().Play();

            // EnumNextDirectionScriptにnextDirectionsの値を渡して、AnimationScriptから次方向の回転を参照できるようにします
            _ends.nextDirection = nextDirections[i].nextDirection;
            // アニメーション再生
            oldCommands[i].Execute(_anim);
            // スタミナを減らすs
            _staminaSlider.value -= 0.1f;
            // 一秒待機
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
    }
}
