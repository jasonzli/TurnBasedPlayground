using System;
using System.Collections.Generic;

namespace Code.ProtoVM
{
    /// <summary>
    /// ObservableBase is just a simplified class that has the interface for the PropertyChanged event.
    /// It is very similar to the below implementation of the ViewModelSimpleBase (which includes IDisposable as well)
    /// Generically passes the value through, so you'll need to be aware of what you're expecting
    /// </summary>
    /// <typeparam name="T">Basic Data Types, this is a protoVM so dont' worry about advanced collections yet</typeparam>
    public abstract class ObservableBase<T> : ObservableTest.ISimpleNotifyPropertyChanged
    {
        public Action<T> PropertyChanged;
        protected T _value;

        //Idea taken from https://github.com/vovgou/loxodon-framework/blob/master/Loxodon.Framework/Assets/LoxodonFramework/Runtime/Framework/Observables/ObservableProperty.cs
        protected ObservableBase() : this(default(T))
        {
        }

        public virtual Type Type => typeof(T);

        public ObservableBase(T value)
        {
            this._value = value;
        }
        
        protected virtual void NotifyPropertyChanged(T value)
        {
            PropertyChanged?.Invoke(value);
        }

        protected bool SetField(ref T field, T value)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(value);
            return true;
        }
        
    }
}