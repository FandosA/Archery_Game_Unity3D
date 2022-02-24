using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform player;
    private GameObject arrow_icon;
    private GameObject arrow;
    private GameObject bow_icon;
    private GameObject player_icon;
    private GameObject bow;
    private GameObject mainCamera;

    void Start()
    {
        arrow = GameObject.Find("Arrow");
        arrow_icon = GameObject.Find("Arrow_icon");
        bow = GameObject.Find("/CameraParent/Camera/Bow");
        bow_icon = GameObject.Find("Bow_icon");
        player_icon = GameObject.Find("Player_icon");
        mainCamera = GameObject.Find("CameraParent");
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 CameraFollowPos = player.position;
        CameraFollowPos.y = transform.position.y;
        transform.position = CameraFollowPos;

        if (arrow.transform.IsChildOf(Camera.main.transform))
        {
            arrow_icon.transform.position = new Vector3(arrow.transform.position.x, 100.0f, arrow.transform.position.z);
            arrow_icon.transform.eulerAngles = new Vector3(0.0f, mainCamera.transform.eulerAngles.y, 0.0f);
        }
        else
        {
            arrow_icon.transform.position = new Vector3(arrow.transform.position.x, 100.0f, arrow.transform.position.z);
            arrow_icon.transform.eulerAngles = new Vector3(arrow_icon.transform.eulerAngles.x, 0.0f, arrow_icon.transform.eulerAngles.z);
        }

        if (bow.transform.IsChildOf(Camera.main.transform))
        {
            bow_icon.transform.position = new Vector3(mainCamera.transform.position.x - 17.37f, 100.0f, mainCamera.transform.position.z - 5.21f);
            bow_icon.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        }
        else
        {
            bow_icon.transform.position = new Vector3(bow.transform.position.x, 100.0f, bow.transform.position.z);
            bow_icon.transform.eulerAngles = bow_icon.transform.eulerAngles;
        }
    }
}
