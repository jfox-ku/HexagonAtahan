using UnityEngine;
using System.Collections;
using UnityEngine.Events;

//Event
[CreateAssetMenu(fileName = "New Int Event", menuName = "Game Event/Int Event")]
[System.Serializable] public class IntEvent : BaseGameEvent<int> { }


//Unity Event 
[System.Serializable] public class UnityIntEvent : UnityEvent<int> { }

