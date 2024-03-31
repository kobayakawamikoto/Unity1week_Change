using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearScript : MonoBehaviour
{
    // TODO: SetActive(false)となっているコンポーネントの取得方法を調べる
    [SerializeField]
    Canvas gameClearCanvas;
    [SerializeField]
    Camera camera;
    [SerializeField]
    GameObject actor;
    InputHandlerScript ihs;
    // Start is called before the first frame update
    void Start()
    {
        ihs = actor.GetComponent<InputHandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameClearCanvas.gameObject.SetActive(true);
        ihs.IsClear = true;
    }
}
