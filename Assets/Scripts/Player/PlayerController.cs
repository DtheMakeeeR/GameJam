using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private float _raycastDistance;

    private InputActionMap _playerActionMap;
    private Vector2 _moveInput;
    private void Awake()
    {
        _playerActionMap = InputSystem.actions.FindActionMap("Player");
        _playerActionMap.Enable();
        _playerActionMap.FindAction("Move").performed += OnMovePerformed;
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {_moveInput}");
        if (Mathf.Abs(_moveInput.x) < 1 && Mathf.Abs(_moveInput.y) < 1) _moveInput = new Vector2(0, 0);
        Debug.Log($"Move Input: {_moveInput}");
        Move();
    }

    private void Move()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveInput, _raycastDistance, _raycastMask);
        if (hit)
        {
            Debug.Log($"Hit: {hit.collider.name}. Can't move!");
        }
        else
        {
            Debug.Log("Path is clear. Moving player.");
            transform.position = transform.position + new Vector3(_moveInput.x, _moveInput.y, 0);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        if (_playerActionMap != null)
        {
            _playerActionMap.Disable();
        }
    }

}
