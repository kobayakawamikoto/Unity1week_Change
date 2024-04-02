using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InputHandlerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextDirectionObj;
    [SerializeField]
    private GameObject _actor;
    [SerializeField]
    private Camera _camera;

    private Animator _anim;

    private Command _upArrow, _downArrow, _leftArrow, _rightArrow;
    [System.NonSerialized] 
    public List<Command> oldCommands = new List<Command>();

    private EnumNextDirectionScript _ends;
    private EnumNextDirectionScript _endsUpArrow, _endsDownArrow, _endsLeftArrow, _endsRightArrow;
    [System.NonSerialized] 
    public List<EnumNextDirectionScript> nextDirections = new List<EnumNextDirectionScript>();

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
        _ends = _nextDirectionObj.GetComponent<EnumNextDirectionScript>();
        _endsUpArrow = new EnumNextDirectionScript();
        _endsUpArrow.nextDirection = EnumNextDirectionScript.NextDirection.Forward;
        _endsDownArrow = new EnumNextDirectionScript();
        _endsDownArrow.nextDirection = EnumNextDirectionScript.NextDirection.Backward;
        _endsLeftArrow = new EnumNextDirectionScript();
        _endsLeftArrow.nextDirection = EnumNextDirectionScript.NextDirection.Left;
        _endsRightArrow = new EnumNextDirectionScript();
        _endsRightArrow.nextDirection = EnumNextDirectionScript.NextDirection.Right;
        _sliderCanvas = GameObject.Find("SliderCanvas");
        Slider[] sliders = _sliderCanvas.GetComponentsInChildren<Slider>();
        // �X���C�_�[��1�ȏ㌩�������ꍇ
        if (sliders.Length > 0)
        {
            foreach (Slider sliderComponent in sliders)
            {
                // �X���C�_�[�̖��O�� "StaminaSlider" �̏ꍇ
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
         * ���������̃R�}���h����
         */
        HandleInput();


        /*
         * ���v���C
         */
        StartReplay();
        
        /*
         * ���g���C
         */
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /*
         * ���S���܂��̓N���A���̃J�������o
         */
        if (isClear || isDeath || _staminaSlider.value <= 0)
        {
            Vector3 offset = new Vector3(0, 1, 2);
            _camera.transform.position = _actor.transform.position + offset;
            _camera.transform.LookAt(_actor.transform);
        }
    }

    /*
     * �L�����̈ړ���ƈړ����Ԃ����߂鏈��
     * �A���h�D�ƈړ��J�n�������
     */
    void HandleInput()
    {
        if (isReplaying) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // oldCommands�ɂ�_upArrow, _downArrow, _leftArrow, _rightArrow�����͂������ԂŒǉ�����Ă���
            // oldCommands���g���ăL��������͂��ꂽ���ԂɈړ���������A�A���h�D�����肷��
            oldCommands.Add(_upArrow);
            // nextDirections�ɂ�_endsUpArrow, _endsDownArrow, _endsLeftArrow, _endsRightArrow�����͂������ԂŒǉ�����Ă���
            // nextDirections���g���ăL�����̉�]���������߂Ă��܂�
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
            // ���v���C
            shouldStartReplay = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // �A���h�D
            UndoLastCommand();
        }
    }

    /*
     * ���͂����L�[���A���h�D���鏈��
     * ���X�g�̍Ō�������Ă��܂�
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
            
            // ���ʉ��s�b�`����/�Đ�
            GetComponent<AudioSource>().pitch = Random.Range(1.05f, 0.95f);
            GetComponent<AudioSource>().Play();

            // EnumNextDirectionScript��nextDirections�̒l��n���āAAnimationScript���玟�����̉�]���Q�Ƃł���悤�ɂ��܂�
            _ends.nextDirection = nextDirections[i].nextDirection;
            // �A�j���[�V�����Đ�
            oldCommands[i].Execute(_anim);
            // �X�^�~�i�����炷s
            _staminaSlider.value -= 0.1f;
            // ��b�ҋ@
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
    }
}
