using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScripts : MonoBehaviour
{
    [SerializeField]
    Canvas gameOverCanvas;
    [SerializeField]
    Camera camera;
    [SerializeField]
    GameObject actor;
    InputHandlerScripts ihs;
    // Start is called before the first frame update
    void Start()
    {
        ihs = actor.GetComponent<InputHandlerScripts>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�Փ�");
        gameOverCanvas.gameObject.SetActive(true);
        ihs.IsDeath = true;
    }
}