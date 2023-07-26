using System;

namespace Code.ProtoVM
{
    
    /// <summary>
    /// Observable is the essential class for databinding *in the model* imo. It can also be used for the viewmodel.
    /// Observable is a class that wraps a basic data type and implements the ISimpleNotifyPropertyChanged interface.
    /// It also has a lot of implicit conversions to and from basic data types for use in basic evaluation.
    ///
    /// Lists and stuff will have to come later
    /// </summary>
    /// <typeparam name="T">Basic data type</typeparam>
    public class Observable<T> : ObservableBase<T>
    {
        public Observable() : this(default(T)) { }
        public Observable(T value) : base(value) { }
        
        public T Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        //Using Observables in basic evaluations is fine, unsure if the other way around is worth doing
        //public static implicit operator T(Observable<T> data) => data.Value; // works but didn't seem to work right with bools?
        
        // casting from Observable<T> to basic data types
        public static implicit operator int(Observable<T> data) => Convert.ToInt32(data.Value);
        public static implicit operator float(Observable<T> data) => Convert.ToSingle(data.Value);
        public static implicit operator double(Observable<T> data) => Convert.ToDouble(data.Value);
        public static implicit operator string(Observable<T> data) => Convert.ToString(data.Value);
        public static implicit operator bool(Observable<T> data) => Convert.ToBoolean(data.Value);
        //public static implicit operator Observable<T>(T data) => new Observable<T>(data);


    }
}