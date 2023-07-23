using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTest
{
    public class ObservableFloat : ScriptableObject
    {
        
        private List<Action<float>> _listeners = new List<Action<float>>();
        
        [SerializeField] private float _value;
        private float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                foreach (Action<float> listener in _listeners)
                {
                    listener.Invoke(value);
                }
            }
        }

        public void AddListener(Action<float> listener) => _listeners.Add(listener);
        public void RemoveListener(Action<float> listener) => _listeners.Remove(listener);
    }

    public class ObservableSO<T> : ScriptableObject
    {
        private List<Action<T>> _listeners = new List<Action<T>>();
        
        [SerializeField] private T _value;
        private T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                foreach (Action<T> listener in _listeners)
                {
                    listener.Invoke(value);
                }
            }
        }

        public void AddListener(Action<T> listener) => _listeners.Add(listener);
        public void RemoveListener(Action<T> listener) => _listeners.Remove(listener);
    }
    
    public class Observable<T>
    {
        private List<Action<T>> _listeners = new List<Action<T>>();
        private T _value;
        
        public Observable(T value)
        {
            _value = value;
        }
        
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                foreach (Action<T> listener in _listeners)
                {
                    listener.Invoke(value);
                }
            }
        }

        public void AddListener(Action<T> listener) => _listeners.Add(listener);
        public void RemoveListener(Action<T> listener) => _listeners.Remove(listener);
    }
}