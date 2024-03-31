using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    Canvas gameOverCanvas;
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
        Debug.Log("è’ìÀ");
        gameOverCanvas.gameObject.SetActive(true);
        ihs.IsDeath = true;
    }
}
