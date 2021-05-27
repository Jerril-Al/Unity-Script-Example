 using System.Collections;
 using UnityEditor.Experimental.UIElements;
 using UnityEngine;
 using UnityEngine.AI;
 using UnityEngine.Serialization;
 //Класс точки предвижения, имеет 3 параметра: точку, минимальное и максимальное время нахождения на точке
 [System.Serializable] public class MovePoint
 {
     public Transform point;
     public float minAfk;
     public float maxAfk;
  
     public MovePoint(Transform point, float minAfk, float maxAfk)
     {
         this.point = point;
         this.minAfk = minAfk;
         this.maxAfk = maxAfk;
     }
 }

 //Скрипт предназначем для объектов с компонентов Unit(Враждебные и дружелюбные НПС), отвечает за перемещение объекта по заданным точка
 public class PathToPointAI : MonoBehaviour
 {    
     [FormerlySerializedAs("Loop")] public bool loop;
     public bool moveInRandomOrder = false;
     public bool afk = false;
     
     private int _pointIndex = 0;
     private bool _inPath = false;
     private Unit _unit;
     private float _time;
 
     public MovePoint[] point;
     
     private void Awake()
     {
         _unit = GetComponent<Unit>();
     }
     
     private bool InMove()
     {
         return !(_unit.navMeshAgent.remainingDistance < _unit.navMeshAgent.radius*1.50f);
     }
     
     private void Update()
     {
         _time += Time.deltaTime;
         if (_unit._patrol && _time > 0.8f)
         {
             _time = 0f;
             if (loop && (_pointIndex >= point.Length) && !_inPath)
             {
                 _pointIndex = 0;
             }
             else if (!_inPath && !InMove() && (_pointIndex < point.Length))
             {
                 if (moveInRandomOrder)
                     _pointIndex = Random.Range(0, point.Length - 1);
                 StartCoroutine(DoSomething());
             }
         }
     }
     
     private IEnumerator DoSomething()
     {
         _inPath = true;
         
         if(afk)
             yield return new WaitForSeconds(Random.Range(point[_pointIndex].minAfk, point[_pointIndex].maxAfk));
             
         _unit.Move(point[_pointIndex].point.position);
         yield return new WaitForSeconds(0.6f);
         _pointIndex++;
         _inPath = false;
     }
 }
