using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _cameraMoveSpeed = 30.0f;
    [SerializeField] private float _cameraRotationSpeed = 40.0f;
    [SerializeField] private Vector3 _cameraMoveVector;
    [SerializeField] private float _cameraRotation;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventManager.MoveCamera += UpdateMoveVector;
        EventManager.RotateCamera += UpdateCameraRotation;
    }

    private void UnsubscribeEvents()
    {
        EventManager.MoveCamera -= UpdateMoveVector;
        EventManager.RotateCamera -= UpdateCameraRotation;
    }

    private void UpdateMoveVector(Vector3 vector)
    {
        _cameraMoveVector += vector;
        if (_cameraMoveVector == Vector3.zero)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        _rigidbody.AddRelativeForce(_cameraMoveSpeed * 10.0f * vector.normalized);
    }

    private void UpdateCameraRotation(float rotation)
    {
        _cameraRotation += rotation;
        if (_cameraRotation == 0.0f)
        {
            _rigidbody.angularVelocity = Vector3.zero;
            return;
        }
        _rigidbody.AddRelativeTorque(0.0f, _cameraRotationSpeed * rotation, 0.0f);
    }
}
