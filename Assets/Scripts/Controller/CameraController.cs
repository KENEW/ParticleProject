using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoSingleton<CameraController>
{ 
    public Camera cameraObject;
    public Transform target = null;

    public float Speed = 5.0f;
    public float Scale = 1.0f;
    
    private float shakePower = 0.0f;
    private float shakeAmount = 7.5f;

    private bool isXShake = true;
    private bool isYShake = true;

    private float angleAmount = 0.0f;
    public float scaleAmount = 1.0f;

    public Vector3 Offset = Vector3.zero;
    private Vector3 anchorPoint = Vector3.zero;

    private Vector3 defaultPos;
    private void Start()
    {
        defaultPos = transform.position;
    }
    
    private void LateUpdate()
    {
        anchorPoint = Vector3.Lerp(anchorPoint, defaultPos, Speed * Time.deltaTime);
        shakePower -= shakePower / shakeAmount;
        
        Vector3 shakeVec = Vector3.zero;
        shakeVec.x = (isXShake) ? Random.Range(-shakePower, shakePower) : 0.0f;
        shakeVec.y = (isYShake) ? Random.Range(-shakePower, shakePower) : 0.0f;
        transform.position = anchorPoint + Offset + shakeVec;
        
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0.0f, 0.0f, angleAmount), 5.0f * Time.deltaTime);
        cameraObject.orthographicSize = Mathf.Lerp(cameraObject.orthographicSize, Scale * scaleAmount, 5.0f * Time.deltaTime);
    }
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    
    public void OnShake(float power, float amount, bool xshake = true, bool yshake = true)
    {
        shakePower = power;
        shakeAmount = amount;
        isXShake = xshake;
        isYShake = yshake;
    }
}