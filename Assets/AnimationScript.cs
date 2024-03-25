using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static EnumNextDirectionScript;

public class AnimationScript : MonoBehaviour
{
    public GameObject nextDirectionObj;
    EnumNextDirectionScript enumNextDirectionScript;
    [SerializeField] GameObject player;
    private bool isMoveForward = false;
    private bool isRotatePlayer = false;
    // 目標先の座標
    private float targetPosition;
    // 現在の座標
    private float newPositionWhileMoving;
    private float cacheNewPositionWhileMoving;
    private float elapsedTime = 0f;
    private float interpolationTime = 0.3f; // 補間にかける時間（秒）
    float rotationSpeed = 800.0f;
    // Start is called before the first frame update
    void Start()
    {
        // 開始時に現在の値を取得
        newPositionWhileMoving = player.transform.position.z;
        enumNextDirectionScript = nextDirectionObj.GetComponent<EnumNextDirectionScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
        RotatePlayer();
    }

    void MoveForward()
    {
        if (isMoveForward == false) return;

        float t = elapsedTime / interpolationTime;

        // 補間処理
        newPositionWhileMoving = Mathf.Lerp(cacheNewPositionWhileMoving, targetPosition, t);

        //float moveSpeed = 5f;
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        switch (enumNextDirectionScript.nextDirection)
        {
            case EnumNextDirectionScript.NextDirection.Forward:
                // オブジェクトの位置を更新
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, newPositionWhileMoving);
                break;
            case EnumNextDirectionScript.NextDirection.Backward:
                // オブジェクトの位置を更新
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, newPositionWhileMoving);
                break;
            case EnumNextDirectionScript.NextDirection.Left:
                // オブジェクトの位置を更新
                player.transform.position = new Vector3(newPositionWhileMoving, player.transform.position.y, player.transform.position.z);
                break;
            case EnumNextDirectionScript.NextDirection.Right:
                // オブジェクトの位置を更新
                player.transform.position = new Vector3(newPositionWhileMoving, player.transform.position.y, player.transform.position.z);
                break;
            default:
                Debug.Log("MoveForward");
                break;
        }

        elapsedTime += Time.deltaTime;

        // 目標値に近づいたら加算を停止する
        if (Mathf.Abs(Mathf.Abs(newPositionWhileMoving) - Mathf.Abs(targetPosition)) <= 0.01f)
        {
            switch (enumNextDirectionScript.nextDirection)
            {
                case EnumNextDirectionScript.NextDirection.Forward:
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, targetPosition);
                    break;
                case EnumNextDirectionScript.NextDirection.Backward:
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, targetPosition);
                    break;
                case EnumNextDirectionScript.NextDirection.Left:
                    player.transform.position = new Vector3(targetPosition, player.transform.position.y, player.transform.position.z);
                    break;
                case EnumNextDirectionScript.NextDirection.Right:
                    player.transform.position = new Vector3(targetPosition, player.transform.position.y, player.transform.position.z);
                    break;
                default:
                    Debug.Log("MoveForward");
                    break;
            }
            isMoveForward = false;
        }
    }

    void ChangeMoveForwardState()
    {
        isRotatePlayer = false;
        isMoveForward = true;
        //Debug.Log(enumNextDirectionScript.nextDirection);
        switch (enumNextDirectionScript.nextDirection)
        {
            case EnumNextDirectionScript.NextDirection.Forward:
                newPositionWhileMoving = player.transform.position.z;
                cacheNewPositionWhileMoving = newPositionWhileMoving;
                targetPosition = newPositionWhileMoving + 1;
                break;
            case EnumNextDirectionScript.NextDirection.Backward:
                newPositionWhileMoving = player.transform.position.z;
                cacheNewPositionWhileMoving = newPositionWhileMoving;
                targetPosition = newPositionWhileMoving - 1;
                break;
            case EnumNextDirectionScript.NextDirection.Left:
                newPositionWhileMoving = player.transform.position.x;
                cacheNewPositionWhileMoving = newPositionWhileMoving;
                targetPosition = newPositionWhileMoving - 1;
                break;
            case EnumNextDirectionScript.NextDirection.Right:
                newPositionWhileMoving = player.transform.position.x;
                cacheNewPositionWhileMoving = newPositionWhileMoving;
                targetPosition = newPositionWhileMoving + 1;
                break;
            default:
                Debug.Log("ChangeMoveForwardState");
                break;
        }
        
        elapsedTime = 0;
    }

    void RotatePlayer()
    {
        if (isRotatePlayer == false) return;

        Vector3 playerForward = player.transform.forward;
        Vector3 nextDirection;
        switch (enumNextDirectionScript.nextDirection)
        {
            case EnumNextDirectionScript.NextDirection.Forward:
                nextDirection = Vector3.forward;
                // 半端な数字にならないように
                if (Mathf.Abs(playerForward.z - nextDirection.z) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0,0,0);
                    break;
                }

                // 外積を使用してプレイヤーを回転する
                if (Cross(playerForward, nextDirection).y < 0)
                {
                    this.transform.Rotate(0, 1 * Time.deltaTime * rotationSpeed, 0);
                }
                else
                {
                    this.transform.Rotate(0, -1 * Time.deltaTime * rotationSpeed, 0);
                }
                break;

            case EnumNextDirectionScript.NextDirection.Backward:
                nextDirection = -Vector3.forward;
                if (Mathf.Abs(playerForward.z - nextDirection.z) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                }

                // 外積を使用してプレイヤーを回転する
                if (Cross(playerForward, nextDirection).y < 0)
                {
                    this.transform.Rotate(0, 1 * Time.deltaTime * rotationSpeed, 0);
                }
                else
                {
                    this.transform.Rotate(0, -1 * Time.deltaTime * rotationSpeed, 0);
                }
                break;
            case EnumNextDirectionScript.NextDirection.Left:
                nextDirection = -Vector3.right;
                if (Mathf.Abs(playerForward.x - nextDirection.x) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                }

                // 外積を使用してプレイヤーを回転する
                if (Cross(playerForward, nextDirection).y < 0)
                {
                    this.transform.Rotate(0, 1 * Time.deltaTime * rotationSpeed, 0);
                }
                else
                {
                    this.transform.Rotate(0, -1 * Time.deltaTime * rotationSpeed, 0);
                }
                break;
            case EnumNextDirectionScript.NextDirection.Right:
                nextDirection = Vector3.right;
                if (Mathf.Abs(playerForward.x - nextDirection.x) <= 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                }

                // 外積を使用してプレイヤーを回転する
                if (Cross(playerForward, nextDirection).y < 0)
                {
                    this.transform.Rotate(0, 1 * Time.deltaTime * rotationSpeed, 0);
                }
                else
                {
                    this.transform.Rotate(0, -1 * Time.deltaTime * rotationSpeed, 0);
                }
                break;
            default:
                nextDirection = transform.forward;
                break;
        }
        Debug.DrawRay(this.transform.position, playerForward * 100, Color.green, 2);
        Debug.DrawRay(this.transform.position, nextDirection * 10, Color.red, 2);
    }

    void ChangeRotatePlayerState()
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
}
