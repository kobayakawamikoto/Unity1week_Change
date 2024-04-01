using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static EnumNextDirectionScripts;

public class AnimationScripts : MonoBehaviour
{
    private GameObject _endObj;
    private EnumNextDirectionScripts _ends;

    [SerializeField]
    private GameObject _actor;

    private bool isMoveForward = false;
    private bool isRotatePlayer = false;

    private float _targetPosition;

    private float _positionWhileMoving;
    private float _cacheNewPositionWhileMoving;
    private float _elapsedTime = 0f;
    private float _interpolationTime = 0.3f; // 補間にかける時間（秒）

    private float ROTATION_SPEED = 800.0f;
    // Start is called before the first frame update
    void Start()
    {
        _positionWhileMoving = _actor.transform.position.z;

        _endObj = GameObject.Find("EnumNextDirectionObj");
        _ends = _endObj.GetComponent<EnumNextDirectionScripts>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
        RotatePlayer();
    }

    /*
     * キャラを移動させる処理
     * _interpolationTimeで指定した秒数で移動を完了する
     */
    void MoveForward()
    {
        if (isMoveForward == false) return;

        float t = _elapsedTime / _interpolationTime;

        // 補間処理
        _positionWhileMoving = Mathf.Lerp(_cacheNewPositionWhileMoving, _targetPosition, t);

        switch (_ends.nextDirection)
        {
            case EnumNextDirectionScripts.NextDirection.Forward:
                // オブジェクトの位置を更新
                _actor.transform.position = new Vector3(_actor.transform.position.x, _actor.transform.position.y, _positionWhileMoving);
                break;
            case EnumNextDirectionScripts.NextDirection.Backward:
                _actor.transform.position = new Vector3(_actor.transform.position.x, _actor.transform.position.y, _positionWhileMoving);
                break;
            case EnumNextDirectionScripts.NextDirection.Left:
                _actor.transform.position = new Vector3(_positionWhileMoving, _actor.transform.position.y, _actor.transform.position.z);
                break;
            case EnumNextDirectionScripts.NextDirection.Right:
                _actor.transform.position = new Vector3(_positionWhileMoving, _actor.transform.position.y, _actor.transform.position.z);
                break;
            default:
                break;
        }

        _elapsedTime += Time.deltaTime;

        // _targetPositionに近づいたら最終的に到達してほしい座標にワープさせる
        if (Mathf.Abs(Mathf.Abs(_positionWhileMoving) - Mathf.Abs(_targetPosition)) <= 0.01f)
        {
            switch (_ends.nextDirection)
            {
                case EnumNextDirectionScripts.NextDirection.Forward:
                    _actor.transform.position = new Vector3(_actor.transform.position.x, _actor.transform.position.y, _targetPosition);
                    break;
                case EnumNextDirectionScripts.NextDirection.Backward:
                    _actor.transform.position = new Vector3(_actor.transform.position.x, _actor.transform.position.y, _targetPosition);
                    break;
                case EnumNextDirectionScripts.NextDirection.Left:
                    _actor.transform.position = new Vector3(_targetPosition, _actor.transform.position.y, _actor.transform.position.z);
                    break;
                case EnumNextDirectionScripts.NextDirection.Right:
                    _actor.transform.position = new Vector3(_targetPosition, _actor.transform.position.y, _actor.transform.position.z);
                    break;
                default:
                    break;
            }
            isMoveForward = false;
        }
    }

    /*
     * Spaceキーを押すと再生されるアニメーションからトリガーされる関数
     * 移動を始める前に各変数の初期化をしています
     */
    void InitializeVariableBeforeMoveForward()
    {
        isRotatePlayer = false;
        isMoveForward = true;

        switch (_ends.nextDirection)
        {
            case EnumNextDirectionScripts.NextDirection.Forward:
                _positionWhileMoving = _actor.transform.position.z;
                _cacheNewPositionWhileMoving = _positionWhileMoving;
                _targetPosition = _positionWhileMoving + 1;
                break;
            case EnumNextDirectionScripts.NextDirection.Backward:
                _positionWhileMoving = _actor.transform.position.z;
                _cacheNewPositionWhileMoving = _positionWhileMoving;
                _targetPosition = _positionWhileMoving - 1;
                break;
            case EnumNextDirectionScripts.NextDirection.Left:
                _positionWhileMoving = _actor.transform.position.x;
                _cacheNewPositionWhileMoving = _positionWhileMoving;
                _targetPosition = _positionWhileMoving - 1;
                break;
            case EnumNextDirectionScripts.NextDirection.Right:
                _positionWhileMoving = _actor.transform.position.x;
                _cacheNewPositionWhileMoving = _positionWhileMoving;
                _targetPosition = _positionWhileMoving + 1;
                break;
            default:
                break;
        }
        
        _elapsedTime = 0;
    }

    /*
     * キャラを回転させる処理
     */
    void RotatePlayer()
    {
        if (isRotatePlayer == false) return;

        Vector3 playerForwardVec = _actor.transform.forward;
        Vector3 nextDirectionVec;

        switch (_ends.nextDirection)
        {
            case EnumNextDirectionScripts.NextDirection.Forward:
                nextDirectionVec = Vector3.forward;

                // 最終的に到達してほしい回転値までワープさせる
                if (Mathf.Abs(playerForwardVec.z - nextDirectionVec.z) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0,0,0);
                    break;
                }

                // 外積を使用してプレイヤーを回転させる
                RotatePerFrame(playerForwardVec, nextDirectionVec);
                break;

            case EnumNextDirectionScripts.NextDirection.Backward:
                nextDirectionVec = -Vector3.forward;
                if (Mathf.Abs(playerForwardVec.z - nextDirectionVec.z) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                }

                RotatePerFrame(playerForwardVec, nextDirectionVec);
                break;

            case EnumNextDirectionScripts.NextDirection.Left:
                nextDirectionVec = -Vector3.right;
                if (Mathf.Abs(playerForwardVec.x - nextDirectionVec.x) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                }

                RotatePerFrame(playerForwardVec, nextDirectionVec);
                break;

            case EnumNextDirectionScripts.NextDirection.Right:
                nextDirectionVec = Vector3.right;
                if (Mathf.Abs(playerForwardVec.x - nextDirectionVec.x) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                }

                RotatePerFrame(playerForwardVec, nextDirectionVec);
                break;

            default:
                nextDirectionVec = transform.forward;
                break;
        }
        Debug.DrawRay(this.transform.position, playerForwardVec * 100, Color.green, 2);
        Debug.DrawRay(this.transform.position, nextDirectionVec * 10, Color.red, 2);
    }

    /*
     * Spaceキーを押すと再生されるアニメーションからトリガーされる関数
     * 回転を始める前に変数の初期化をしています
     */
    void InitializeVariableBeforeRotatePlayer()
    {
        isRotatePlayer = true;
    }

    /*
     * 外積を返す関数
     * 回転に使います
     */
    Vector3 Cross(Vector3 v, Vector3 w)
    {
        // 右手座標系
        Vector3 cross = new Vector3(v.y * w.z - v.z * w.y, v.z * w.x - v.x * w.z, v.x * w.y - v.y * w.x);
        // 左手座標系
        Vector3 cross2 = new Vector3(v.z * w.y - v.y * w.z, v.x * w.z - v.z * w.x, v.y * w.x - v.x * w.y);

        return cross2;
    }

    /*
     * フレーム毎にキャラを回転させる処理
     * 二つのベクトルの外積の結果から回転する方向を決めています
     * ROTATION_SPEEDで回転速度を変えられます
     */
    void RotatePerFrame(Vector3 playerForwardVec, Vector3 nextDirectionVec)
    {
        if (Cross(playerForwardVec, nextDirectionVec).y < 0)
        {
            this.transform.Rotate(0, 1 * Time.deltaTime * ROTATION_SPEED, 0);
        }
        else
        {
            this.transform.Rotate(0, -1 * Time.deltaTime * ROTATION_SPEED, 0);
        }
    }

    /*
    void WarpTargetRotation(Vector3 playerForwardVec, Vector3 nextDirectionVec, float RotationValue)
    {
        if(nextDirectionVec == Vector3.forward && Mathf.Abs(playerForwardVec.z - nextDirectionVec.z) <= 0.1f)
        { 
            transform.rotation = Quaternion.Euler(0, RotationValue, 0);
        }
        else if (nextDirectionVec == -Vector3.forward && (Mathf.Abs(playerForwardVec.z - nextDirectionVec.z) <= 0.1f))
        {
            transform.rotation = Quaternion.Euler(0, RotationValue, 0);
        }
        else if (nextDirectionVec == -Vector3.right && (Mathf.Abs(playerForwardVec.x - nextDirectionVec.x) <= 0.1f))
        {
            transform.rotation = Quaternion.Euler(0, RotationValue, 0);
        }
        else if (nextDirectionVec == -Vector3.right && (Mathf.Abs(playerForwardVec.x - nextDirectionVec.x) <= 0.1f))
        {
            transform.rotation = Quaternion.Euler(0, RotationValue, 0);
        }
    }
    */
}
