using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    private GameManager gameManager;
    private StateManager state;
    public Transform playerTransform;

    public Vector3 cameraOffset = new Vector3(12, 16, 0);
    public float cameraMoveSpeed = 30.0f;
    public float edgePadding = 5;

    // depends on the map size and needs to be set so the camera does not leave the game area (30/120 matches map1 size)
    private static float cameraScrollLimitX = 30;
    private static float cameraScrollLimitZ = 120;

    private void Awake()
    {
        // initialize camera position based on default offset
        transform.rotation = Quaternion.Euler(50, -90, 0);
        CameraToPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = gameManager.state;
        state.SettingsCameraLockedToPlayer = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            SwitchCameraLockState();

        // if camera is not locked to the player, move camera based on key arrow key input and hitting the edges of the screen / window with the mouse
        if (!state.SettingsCameraLockedToPlayer)
            HandleCameraMovement();

    }

    private void LateUpdate()
    {
        if (state.SettingsCameraLockedToPlayer)
            CameraToPlayer();

        KeepCameraInBounds();

    }

    private void SwitchCameraLockState()
    {
        // if camera is not locked to the player, jump to the player
        if (!state.SettingsCameraLockedToPlayer)
            CameraToPlayer();
        // switch lock state
        state.SettingsCameraLockedToPlayer = !state.SettingsCameraLockedToPlayer;
    }

    private void HandleCameraMovement()
    {
        // jump to player if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
            CameraToPlayer();

        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - edgePadding)
            transform.Translate(new Vector3(-cameraMoveSpeed * Time.deltaTime, 0, 0), Space.World);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= 0 + edgePadding)
            transform.Translate(new Vector3(-cameraMoveSpeed * Time.deltaTime, 0, 0));
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= 0 + edgePadding)
            transform.Translate(new Vector3(cameraMoveSpeed * Time.deltaTime, 0, 0), Space.World);
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - edgePadding)
            transform.Translate(new Vector3(cameraMoveSpeed * Time.deltaTime, 0, 0));

    }

    private void KeepCameraInBounds()
    {
        if (transform.position.x >= cameraScrollLimitX)
            transform.position = new Vector3(cameraScrollLimitX, transform.position.y, transform.position.z);
        if (transform.position.x <= -cameraScrollLimitX)
            transform.position = new Vector3(-cameraScrollLimitX, transform.position.y, transform.position.z);
        if (transform.position.z >= cameraScrollLimitZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, cameraScrollLimitZ);
        if (transform.position.z <= -cameraScrollLimitZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, -cameraScrollLimitZ);
    }

    private void CameraToPlayer()
    {
        transform.position = playerTransform.position + cameraOffset;
    }
}
