using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

//Event
[CreateAssetMenu(fileName = "New Void Event", menuName = "Game Event/Void Event")]
[System.Serializable] public class VoidEvent : BaseGameEvent<Void>{ }


//Unity Event 
[System.Serializable] public class UnityVoidEvent : UnityEvent<Void> { }