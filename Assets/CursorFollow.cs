using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class CursorFollow : MonoBehaviour
{

    public Vector3 lookPosition = new Vector3();
    public CinemachineVirtualCamera virtualCamera;
    Transform cameraLookPosition;
    GameObject player;
    public float cursorInfluence = 0.01f;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        cameraLookPosition = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        virtualCamera.Follow = cameraLookPosition;

    }

    // Update is called once per frame
    void Update()
    {

         Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 influencedPosition = Vector3.Lerp(player.transform.position, mousePosition, cursorInfluence);


        transform.position = influencedPosition;



    }




    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(Camera.current.ScreenToWorldPoint(Input.mousePosition), 1);



    }
}
