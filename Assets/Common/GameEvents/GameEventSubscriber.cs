using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace SeedWork
{
    public class GameEventSubscriber : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Action;

        void Awake()
        {
            Assert.IsNotNull(Event);
            Assert.IsNotNull(Action);
        }

        void OnEnable()
        {
            Event.Subscribe(this);
        }

        void OnDisable()
        {
            Event.Unsubscribe(this);
        }

        public void OnEventRaised()
        {
            Action.Invoke();
        }
    }
}
