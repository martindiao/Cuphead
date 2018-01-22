﻿using System;
using UnityEngine;

public class Angel : MonoBehaviour {

    private Camera _main;
    
    public float speed = 1.0F;
    private float _startTime;
    private float _journeyLength;

    private Vector3 _offset;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    [SerializeField] private GameObject arrow;

    enum AngelStatus {
        MovingIn,
        Arrow,
        MovingOut
    }

    private AngelStatus status = AngelStatus.MovingIn;
    
    private void Start() {
        _main = Camera.main;
        _offset = _main.transform.position - transform.position;
        _offset.z = 0;
        _startTime = Time.time;
        
        _startPosition = transform.position;
        _endPosition = _startPosition;
        _endPosition.x += 2;
        _endPosition.z = 0;
        
        _journeyLength = Vector3.Distance(_startPosition, _endPosition);
    }

    private void Update() {
        switch (status) {
            case AngelStatus.MovingIn:
                MovingIn();
                break;
            case AngelStatus.Arrow:
                ShootArrow();
                break;
            case AngelStatus.MovingOut:
                MovingOut();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }

    void MovingIn() { // TODO: Refactor This Lerp Into One Function For Both Moving in/out
        Vector3 pos = Camera.main.transform.position;
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.nearClipPlane));
        pos.x = p.x - 1.5f;
        pos.z = 0;
        
        float distCovered = (Time.time - _startTime) * speed;
        float fracJourney = distCovered / _journeyLength;
        Vector3 endPos = Vector3.Lerp(_startPosition, pos, fracJourney);
        
        transform.position = endPos;

        if (Vector3.Distance(transform.position, pos) <= 0.1f) {
            status = AngelStatus.Arrow;
        }   
    }

    void MovingOut() { // TODO: Refactor This Lerp Into One Function For Both Moving in/out
        Vector3 pos = Camera.main.transform.position;
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.nearClipPlane));
        pos.x = p.x + 1.5f;
        pos.z = 0;
        
        float distCovered = (Time.time - _startTime) * speed;
        float fracJourney = distCovered / _journeyLength;
        Vector3 endPos = Vector3.Lerp(_startPosition, pos, fracJourney);
        
        transform.position = endPos;
        
        if (Vector3.Distance(transform.position, pos) <= 0.1f) {
            Destroy(gameObject);
        }
    }

    void ShootArrow() {
        Transform go = Instantiate(arrow, transform.position, Quaternion.identity).transform;
        go.transform.LookAt(Boss._player.transform.position);

        status = AngelStatus.MovingOut;
        _startTime = Time.time;
        _startPosition = transform.position;

    }
    
    float ClampAngle(float angle, float from, float to) {
        if(angle > 180) angle = 360 - angle;
        angle = Mathf.Clamp(angle, from, to);
        if(angle < 0) angle = 360 + angle;

        return angle;
    }
}
