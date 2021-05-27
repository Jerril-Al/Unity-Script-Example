using System.Collections;
 using UnityEditor.Experimental.UIElements;
 using UnityEngine;
 using UnityEngine.AI;
 using UnityEngine.Serialization;
 //Класс точке предвижения, имеет 3 параметра: точку, минимальное и максимальное время нахождения на точке
 [System.Serializable] public class MovePoint
 {
     public Transform point;
     public float minAfk;
     public float maxAfk;
     //конструктор класса
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
     [FormerlySerializedAs("Loop")] public bool loop;//повторять передвижение в цикле?
     
     public bool moveInRandomOrder = false;//перемещаться по точкам в случайном порядке?
 
     public bool afk = false;//нужно ли стоят на точке?
     private int _pointIndex = 0;
     private bool _inPath = false;
     private Unit _unit;
     private float _time;
 
     public MovePoint[] point;//создаём массив типа MovePoint
     
     //подключаемся к компоненту Unit при инициализации объекта
     private void Awake()
     {
         _unit = GetComponent<Unit>();
     }
     //проверяем в пути ли объект, если расстояние до конечной точке мало, возвращаем false
     private bool InMove()
     {
         return !(_unit.navMeshAgent.remainingDistance < _unit.navMeshAgent.radius*1.50f);
     }
     //срабатывает каждый кадр игры
     private void Update()
     {
         _time += Time.deltaTime; // овеличивает переменную с временем на разницу времени между кадрами
         if (_unit._patrol && _time > 0.8f)//если объект должен патрулировать и прошло больше 0,8 секунды
         {
             //Debug.Log(_time);
             _time = 0f;//сбрасываем счётчик времени
             if (loop && (_pointIndex >= point.Length) && !_inPath)//если мы движемся в цикле и индекс точки вышел за пределы массива, то обнуляем его
             {
                 _pointIndex = 0;
                 //Debug.Log("Index set zero");
             }
             else if (!_inPath && !InMove() && (_pointIndex < point.Length))//если мы не в функции пермещения, и не движемся, и индекс не вышел за пределы массива
             {
                 if (moveInRandomOrder)//если задано перемещение в случайном порядке, то генерирем индекс
                     _pointIndex = Random.Range(0, point.Length - 1);
                 //Debug.Log("DoSomething " + _pointIndex + " / " + point.Length);
                 StartCoroutine(DoSomething());//запускаем параллельную функцию перемещения
             }
         }
     }
     //функция перемещения
     private IEnumerator DoSomething()
     {
         _inPath = true;
         if(afk)//если надо ждать на точке, то рандомим значение для ожидания от минимально до максимально заданного
             yield return new WaitForSeconds(Random.Range(point[_pointIndex].minAfk, point[_pointIndex].maxAfk));
         _unit.Move(point[_pointIndex].point.position);//отправляем объект на точку
         yield return new WaitForSeconds(0.6f);//небольшая задержка
         _pointIndex++;//увеличиваем индекс для массива точек
         _inPath = false;
     }
 }