using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using MEC;

public class PlayerController : MonoBehaviour
{
    [Header("Шаги")]
    [SerializeField] private int _stepsPerCycle;
    [SerializeField] private int _currentSteps;

    //[Header("Спрайты")]
    //[SerializeField] private GameObject _sideSprite;
    //[SerializeField] private GameObject _upSprite;
    //[SerializeField] private GameObject _downSprite;

    [Header("Рейкаст")]
    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private float _visionDistance;
    private List<GameObject> _visibleObjects;

    [Header("Ссылки")]
    [SerializeField] private TMP_Text _stepsText;
    [SerializeField] private FieldOfView _fieldOfView;

    private bool CanMove => _currentSteps > 0;

    private InputActionMap _playerActionMap;
    private Vector2 _moveInput;
    private void Awake()
    {
        _playerActionMap = InputSystem.actions.FindActionMap("Player");
        _playerActionMap.Enable();
        _playerActionMap.FindAction("Move").performed += OnMovePerformed;

        _currentSteps = _stepsPerCycle;
        UpdateStepsText();
    }

    private void UpdateStepsText()
    {
        _stepsText.text = $"Steps: {_currentSteps}";
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
        if (!CanMove) return;
        if (_moveInput.x < 0) {

            Vector3 newLocalScale = gameObject.transform.localScale;
            newLocalScale.x = -Mathf.Abs(newLocalScale.x);
            gameObject.transform.localScale = newLocalScale;
        }
        else if (_moveInput.x > 0)
        {
            Vector3 newLocalScale = gameObject.transform.localScale;
            newLocalScale.x = Mathf.Abs(newLocalScale.x);
            gameObject.transform.localScale = newLocalScale;
        }
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveInput, _raycastDistance, _raycastMask);
        //if (hit)
        //{
        //    Debug.Log($"Hit: {hit.collider.name}. Can't move!");
        //}
        //else
        //{
        //    Debug.Log("Path is clear. Moving player.");
        //    _currentSteps--;
        //    UpdateStepsText();
        //    if ( _currentSteps == 0 )
        //    {
        //        Timing.RunCoroutine(_ResetStepsCoroutine().CancelWith(gameObject));
        //    }
        //    transform.position = transform.position + new Vector3(_moveInput.x, _moveInput.y, 0);
        //}
        if (TilesManager.Instance.CanEnterTile(transform.position.With(z: 0), transform.position.With(z: 0) + new Vector3(_moveInput.x, _moveInput.y, 0)))
        {
            Debug.Log("Path is clear. Moving player.");
            _currentSteps--;
            UpdateStepsText();
            if (_currentSteps == 0)
            {
                Timing.RunCoroutine(_ResetStepsCoroutine().CancelWith(gameObject));
            }
            transform.position = transform.position + new Vector3(_moveInput.x, _moveInput.y, 0);
            
        }
        else
        {
            Debug.Log("Can't move to the target tile!");
        }
    }

    public IEnumerator<float> _ResetStepsCoroutine()
    {
        yield return Timing.WaitForSeconds(5f);
        _currentSteps = _stepsPerCycle;
        UpdateStepsText();
    }

    
    private void OnDisable()
    {
        if (_playerActionMap != null)
        {
            _playerActionMap.FindAction("Move").performed -= OnMovePerformed;
            _playerActionMap.Disable();
        }
    }
    private void Update()
    {
        _fieldOfView.SetOrigin(transform.position.With(z: -1));
    }
}
