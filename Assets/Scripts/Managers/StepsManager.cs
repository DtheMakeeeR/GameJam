using System;
using UnityEngine;
using System.Collections.Generic;
using MEC;

public class StepsManager : MonoBehaviour
{
    public static StepsManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            FindAllMobs();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    [Header("Ссылки")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private List<IMob> _mobs = new List<IMob>();
    [Header("Настройки")]
    [SerializeField] private float _delayBetweenSteps = 0.5f;


    private IEnumerator<float> _StartEnemyTurnCoroutine()
    {
        yield return Timing.WaitForSeconds(_delayBetweenSteps);
        //yield return Timing.RunCoroutine(_EnemyStepsCoroutine(resetPlayerSteps: false));
        CoroutineHandle enemyStepsCoroutine = Timing.RunCoroutine(_EnemyStepsCoroutine().CancelWith(gameObject));
        while (Timing.IsRunning(enemyStepsCoroutine))
        {
            yield return Timing.WaitForOneFrame;
        }
        //Debug.Log($"Player is {_player.gameObject.name}");
        _player.ResetSteps();
    }

    private IEnumerator<float> _EnemyStepsCoroutine()
    {
        _player.SpecialFlag = false;
        foreach (var mob in _mobs)
        {
            Debug.Log($"Mob {mob} is making a step.");
            mob.MakeStep();
            yield return Timing.WaitForSeconds(_delayBetweenSteps);
        }
        _player.SpecialFlag = true;
    }
    public void RunEnemies(int n = 1)
    {
        for (int i = 0; i < n; i++)
        {
            CoroutineHandle enemyStepsCoroutine = Timing.RunCoroutine(_EnemyStepsCoroutine().CancelWith(gameObject));
            while (Timing.IsRunning(enemyStepsCoroutine))
            {
                
            }
        }
    }
    public void StartEnemyTurn()
    {
        Timing.RunCoroutine(_StartEnemyTurnCoroutine());
    }
    private void FindAllMobs()
    {
        var mobObjects = GameObject.FindGameObjectsWithTag("Mob");

        foreach (var mobObject in mobObjects)
        {
            var mob = mobObject.GetComponent<IMob>();
            if(mob != null) _mobs.Add(mob);
        }
    }
}
