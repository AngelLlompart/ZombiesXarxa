using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 0.5f;
    private Vector2 look;
    private float lookRotation;
    //private Vector3 _angles = Vector3.zero;
    private float _maxVertivalAngle = 90;
    public GameObject camHolder;

    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*float horizontalRotation= Input.GetAxis("Mouse X");
        float verticalRotation= Input.GetAxis("Mouse Y");
        _angles.y += horizontalRotation * mouseSensibility * Time.deltaTime;
        
        _angles.x -= verticalRotation * mouseSensibility * Time.deltaTime;
        _angles.x = Mathf.Clamp(_angles.x, -_maxVertivalAngle, _maxVertivalAngle);
        transform.rotation = Quaternion.Euler(_angles);*/
    }
    
    
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    
    void Look()
    {
        
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        
        //Turn
        transform.Rotate(Vector3.up * look.x * sensitivity);
        
        //Look
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y,
            camHolder.transform.eulerAngles.z);
    }
    
    void LateUpdate()
    {
        Look();
    }
}
