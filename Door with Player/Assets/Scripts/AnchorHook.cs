using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorHook : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _anchorHook;
    [SerializeField] private Transform _anchorHookEndPoint;
    [SerializeField] private Transform _handPose;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _anchorLayer;
    [SerializeField] private float _maxAnchorDistance;
    [SerializeField] private float _hookSpeed;
    [SerializeField] private Vector3 _offset;

    private bool isShooting, isGrabbing;
    private Vector3 _hookPoint;

    private void Start()
    {
        isShooting = false;
        isGrabbing = false;
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        if(_anchorHook.parent == _handPose)
        {
            // _anchorHook.localPosition = Vector3.Lerp(_anchorHook.localPosition, Vector3.zero, 100f);
            _anchorHook.localPosition = Vector3.zero;
            _anchorHook.localRotation = Quaternion.Euler(new Vector3( 90, 0, 0));
        }
            //if (Input.GetKeyDown(KeyCode.G))
            if (Input.GetMouseButtonDown(1))
        {
            ShootAnchor();
        }

        if (isGrabbing)
        {
            _anchorHook.position = Vector3.Lerp(_anchorHook.position, _hookPoint, _hookSpeed * Time.deltaTime);
            if(Vector3.Distance(_anchorHook.position, _hookPoint) < 0.5f)
            {
                _controller.enabled = false;
                _playerBody.position = Vector3.Lerp(_playerBody.position, _hookPoint - _offset, _hookSpeed * Time.deltaTime);
                if (Vector3.Distance(_anchorHook.position, _hookPoint - _offset) < 0.5f)
                {
                    _controller.enabled = true;
                    isGrabbing = false;
                    _anchorHook.SetParent(_handPose);
                    _lineRenderer.enabled = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (_lineRenderer.enabled)
        {
            _lineRenderer.SetPosition(0, _anchorHookEndPoint.position);
            _lineRenderer.SetPosition(1, _handPose.position);
        }
    }
    private void ShootAnchor()
    {
        if (isShooting || isGrabbing) return;

        isShooting = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit , _maxAnchorDistance, _anchorLayer))
        {
            _hookPoint = hit.point;
            isGrabbing = true;
            _anchorHook.parent = null;
            _anchorHook.LookAt(_hookPoint);
            _lineRenderer.enabled = true;
            print("HIT");
        }

        isShooting = false;
    }

}