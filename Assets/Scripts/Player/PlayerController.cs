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
    [SerializeField] private AudioClip _walkSound;

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

    [Header("Настройки")]
    [SerializeField] private AudioClip _dieSound;

    private bool CanMove => _currentSteps > 0;
    int CurrentSteps
    {
        get => _currentSteps;
        set
        {
            _currentSteps = value;
            UpdateStepsText();
        }
    }

    private InputActionMap _playerActionMap;
    private Vector2 _moveInput;
    private void Awake()
    {
        _playerActionMap = InputSystem.actions.FindActionMap("Player");
        _playerActionMap.Enable();
        _playerActionMap.FindAction("Move").performed += OnMovePerformed;

        CurrentSteps = _stepsPerCycle;
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
        ChangeDirectionOfSprite();
        if (TilesManager.Instance.CanEnterTile(transform.position.With(z: 0), transform.position.With(z: 0) + new Vector3(_moveInput.x, _moveInput.y, 0)))
        {
            Debug.Log("Path is clear. Moving player.");
            CurrentSteps--;
            SFXManager.Instance.PlaySoundOnce(_walkSound);
            if (CurrentSteps == 0)
            {
                StepsManager.Instance.StartEnemyTurn();
            }
            transform.position = transform.position + new Vector3(_moveInput.x, _moveInput.y, 0);

        }
        else
        {
            Debug.Log("Can't move to the target tile!");
        }
    }

    private void ChangeDirectionOfSprite()
    {
        if (_moveInput.x < 0)
        {

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
    }


    public void ResetSteps()
    {
        CurrentSteps = _stepsPerCycle;
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
        _fieldOfView.SetOrigin(transform.position.With(z: 0));
    }

    public void SetViewDistance(float distance)
    {
        _fieldOfView.SetViewDistance(distance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER TRIGGER");
        Interactble other = collision.GetComponent<Interactble>();
        other?.MakeInteraction(this);
    }

    public void Die()
    {
        //TODO: Add death 
        SFXManager.Instance.PlaySoundOnce(_dieSound);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit;
#endif
    }
}
