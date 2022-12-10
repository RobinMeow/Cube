using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SeedWork
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        readonly List<GameEventSubscriber> subscribers = new List<GameEventSubscriber>();

        public void Raise()
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                subscribers[i].OnEventRaised();
            }
        }

        public void Subscribe(GameEventSubscriber subscriber)
        {
            if (!subscribers.Contains(subscriber))
                subscribers.Add(subscriber);
        }
        public void Unsubscribe(GameEventSubscriber subscriber)
        {
            if(subscribers.Contains(subscriber))
                subscribers.Remove(subscriber);
        }
    }
}
