using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraControls : MonoBehaviour
{
    public bool PlayerControl { get; set; }
    
    [SerializeField] private int _zoomLevels;
    [SerializeField] private float _moveCamFrame;
    [SerializeField] private float _moveCamSpeed;
    
    private Camera _cam;
    private PixelPerfectCamera _pixelPerfectCam;

    private Vector2Int _startResolution;
    private Vector3 _startCamPos;
    private int _currentZoomLevel;
    private Vector2 _screenCenter;
    
    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _pixelPerfectCam = GetComponent<PixelPerfectCamera>();
        _moveCamFrame *= _moveCamFrame;
        _startResolution = new(_pixelPerfectCam.refResolutionX, _pixelPerfectCam.refResolutionY);
        _startCamPos = _cam.transform.position;
        _currentZoomLevel = _zoomLevels;
        _screenCenter = new(Screen.width / 2f, Screen.height / 2f);

    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneStart;
        PlayerControl = true;
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        _currentZoomLevel = _zoomLevels;
        _cam.transform.position = _startCamPos;
        PlayerControl = true;
    }

    private void LateUpdate()
    {
        
        if(_currentZoomLevel >= _zoomLevels || !PlayerControl) return;

        Vector3 moveDir = Vector3.zero;
        Vector2 mousePos = Mouse.current.position.value;

        mousePos.x = 2f *(mousePos.x / Screen.width) - 1f;
        moveDir.x = mousePos.x * mousePos.x > _moveCamFrame ? mousePos.x : 0f;
        
        mousePos.y = 2f *(mousePos.y / Screen.height) - 1f;
        moveDir.y = mousePos.y * mousePos.y > _moveCamFrame ? mousePos.y : 0f;

        _cam.transform.Translate(Time.deltaTime * _moveCamSpeed * moveDir);
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        float scroll = ctx.ReadValue<Vector2>().y;
        if (scroll == 0f) return;
        _currentZoomLevel += scroll > 0f ? -1 : 1;
        Zoom();
    }

    public void ZoomToRat(Vector2 pos)
    {
        PlayerControl = false;
        transform.position = new(pos.x, pos.y, transform.position.z);
        _currentZoomLevel = 1;
        Zoom();
    }

    private void Zoom()
    {
        _currentZoomLevel = Mathf.Clamp(_currentZoomLevel, 1, _zoomLevels);
        _pixelPerfectCam.refResolutionX = _currentZoomLevel * _startResolution.x / _zoomLevels ;
        _pixelPerfectCam.refResolutionY = _currentZoomLevel * _startResolution.y / _zoomLevels ;
        if (_currentZoomLevel == _zoomLevels) _cam.transform.position = _startCamPos;
    }
    
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneStart;
    }
    
    
}
