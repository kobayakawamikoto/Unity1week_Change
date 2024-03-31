using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearScript : MonoBehaviour
{
    [SerializeField]
    Canvas gameClearCanvas;
    [SerializeField]
    Camera camera;
    [SerializeField]
    GameObject actor;
    InputHandler inputHandler;
    // Start is called before the first frame update
    void Start()
    {
        inputHandler = actor.GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameClearCanvas.gameObject.SetActive(true);
        inputHandler.IsClear = true;
    }
}
