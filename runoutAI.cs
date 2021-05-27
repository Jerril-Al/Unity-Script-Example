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
