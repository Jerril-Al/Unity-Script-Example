<<<<<<< HEAD
﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;
//Скрипт предназначен для объектов не имеющих компонента Unit, например курицы, скрипт заставит объект отдалсять от игрока, если тот подошёл к нему слишком близко
public class runoutAI : MonoBehaviour 
{
    private NavMeshAgent _agent;
    public float EnemyDistanceRun = 4.0f;
    private float _time = 0f;
    private bool _inRoutine = false;
	//инициализация, подключаемся к компоненту NavMeshAgent нашего объекта, на который помещём скрипт
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
	//выполняется каждый кадр игры, проверяем прошлоли 1,5 секунды с предыдёщей итерации последующих проверок,
	//далее проверяем не выполняется ли сейчас паралленый процесс отдаления от игрока и проверяем не подошёл ли игрок слишком близко
    private void Update()
    {
        if (_time >= 1.5f)
        {
            if (!_inRoutine && CheckPlayer())
            {
                StartCoroutine(DoSomething());
            }
            _time = 0f;
        }
        else
        {
            _time += Time.deltaTime;
        }
    }
    //вычисляем расстояние между игроком и объектом, если расстояние меньше допустимого возращаем true
    private bool CheckPlayer()
    {
        var distance = Vector3.Distance(_agent.transform.position, playerManager.instance.player.transform.position);
        //Debug.Log("Distance: " + distance);
        return distance < EnemyDistanceRun;
    }
	//функция выполняемая в параллельном потоке, спомощью координат место положения игрока и объекта вычисляет вектор противоположнывй вектору между игроком и объектом
	//далее отправляем объект в точку на полученном векторе и делаем задержку 1,2 секунды, чтобы функция не была вызвана повторна
    private IEnumerator DoSomething()
    {
        _inRoutine = true;
        var position = _agent.transform.position;
        var dirToPlayer = position - playerManager.instance.player.transform.position;
        var newPos = position + dirToPlayer;
        _agent.SetDestination(newPos);
        yield return new WaitForSeconds(1.2f);
        _inRoutine = false;
    }
}
=======
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
//Скрипт предназначен для объектов не имеющих компонента Unit, например курицы, скрипт заставит объект отдалсять от игрока, если тот подошёл к нему слишком близко
public class runoutAI : MonoBehaviour 
{
    public float EnemyDistanceRun = 4.0f;
    
    private NavMeshAgent _agent;
    private float _time = 0f;
    private bool _inRoutine = false;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        if (_time >= 1.5f)
        {
            if (!_inRoutine && CheckPlayer())
            {
                StartCoroutine(DoSomething());
            }
            _time = 0f;
        }
        else
        {
            _time += Time.deltaTime;
        }
    }

    private bool CheckPlayer()
    {
        var distance = Vector3.Distance(_agent.transform.position, playerManager.instance.player.transform.position);
        return distance < EnemyDistanceRun;
    }

    private IEnumerator DoSomething()
    {
        _inRoutine = true;
        var position = _agent.transform.position;
        var dirToPlayer = position - playerManager.instance.player.transform.position;
        var newPos = position + dirToPlayer;
        _agent.SetDestination(newPos);
        yield return new WaitForSeconds(1.2f);
        _inRoutine = false;
    }
}
>>>>>>> a9616610dc44cbb437cd532ac539186c5d1e6d4e
