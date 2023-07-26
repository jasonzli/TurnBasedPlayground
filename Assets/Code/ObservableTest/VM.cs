using System;
using System.Collections.Generic;
using System.ComponentModel; //Only used in full implementation of INotifyPropertyChanged
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mono.Collections.Generic; //Only used in full implementation of INotifyPropertyChanged
using UnityEngine;
using Random = System.Random;

/*
 *
 * A lot of what is in here is from a mixture of Rider snippets and MicrosoftLearn and StackOverflow reading
 * but Specifically big credit to this stack writeup: https://stackoverflow.com/questions/10324009/mvvm-modified-model-how-to-correctly-update-viewmodel-and-view
 *
 * From there:
 * From my understanding, [Using the Model to update the ViewModel] is a reversal of responsibilities. The ViewModel is the active part that get's the new Model.
 * How the ViewModel get's the new Model delivered is irrelevant. But the Model is just a dumb data object without any code.
 * Just think of a warehouse. The stuff in the warehouse is the Model. The warehouse manager is the ViewModel. The stuff
 * can't tell that it's old. Someone has to tell the warehouse manager that his stuff is old and he needs to get new stuff.
 * 
 * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-7.0
 * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventhandler?view=net-7.0
 * https://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface
 * https://softwareengineering.stackexchange.com/questions/135413/should-a-view-and-a-model-communicate-or-not
 * https://medium.com/android-news/mvvm-how-view-and-viewmodel-should-communicate-8a386ce1bb42
 * https://forums.swift.org/t/in-mvvm-is-it-ok-for-your-model-to-be-an-observableobject/60355
 * There remains a big question what "the model" is really meant to be in Unity's case.
 * </summary>
 */
namespace Code.ObservableTest
{
    /// <summary>
    /// The Action only version of the INotifyPropertyChanged interface, which normally has a PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e)
    /// </summary>
    public interface ISimpleNotifyPropertyChanged
    {
        public delegate void PropertyChanged();
    }


    /// <summary>
    /// ObservableBase is just a simplified class that has the interface for the PropertyChanged event.
    /// It is very similar to the below implementation of the ViewModelSimpleBase (which includes IDisposable as well)
    /// Generically passes the value through, so you'll need to be aware of what you're expecting
    /// </summary>
    /// <typeparam name="T">Basic Data Types</typeparam>
    public abstract class ObservableBase<T> : ISimpleNotifyPropertyChanged
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



    /// <summary>
    /// This is a simplified implementation version of the INotifyPropertyChanged interface based on my own understanding
    /// This one removes all the additional caller and property names that are passed through, simplifying what the listeners can do
    /// To me, this is an acceptable tradeoff, as the view objects that listen should also have contexts.
    ///
    /// So this viewmodel only knows to update you *that something changed* and also this doesn't stop you from
    /// using Observable<T> in the viewmodel itself too. And that's what you'll do: shove this baby fully of
    /// Observables and then listen to them in the view.
    /// </summary>
    public abstract class ViewModelSimpleBase : IDisposable
    {
        /// <summary>
        /// You should be implementing this! Free up all memory and close up shop
        /// "Deconstructors" aren't in C#, so this is the best we can do
        /// </summary>
        public abstract void Dispose();
    }

    /// <summary>
    /// The full INotifyPropertyChanged interface implementation as given by the snippet.
    /// This one uses the PropertyChangedEventHandler from System.ComponentModel to send the object and property name
    /// back up the chain of calls.
    ///
    /// This can be further inherited by whatever ViewModel classes
    ///
    /// This one is the least *mine* as well
    /// </summary>
    public abstract class ViewModelBase : MonoBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
    }
  

    
    /// <summary>
    /// Example Model definition that uses the Observables
    /// </summary>
    public class PlayerModel
    {
        public int Health { get; set; }
        public string Name { get; set; }
        public bool Guard { get; set; }
        public List<string> Inventory { get; set; }
        
        public PlayerModel(string name, int health, bool guard)
        {
            Health = health;
            Name = name;
            Guard = guard;
            Inventory = new List<string>();
        }
        
        public void RandomValues()
        {
            Random random = new Random();
            Health = random.Next(0,100);
            Name = random.Next(0, 100).ToString();
            Guard = random.Next(0, 100) > 50;
            Inventory.Add(random.Next(0, 100).ToString());
        }
    }
    
    //Only a monobehavior because we need it to be tested here
    public class VM : MonoBehaviour, IDisposable
    {
        private PlayerModel referenceModel;

        public Observable<int> Health { get; set; } = new();
        public Observable<string> Name { get; set; } = new();
        public Observable<bool> Guard { get; set; } = new();
        public Observable<List<string>> Inventory { get; set; } = new();

        [ContextMenu("Initialize")]
        public void Intialize()
        {
            referenceModel = new PlayerModel("Steven", 100, true);
            SetupValuesFromModel(referenceModel);
            Debug.Log("Health: " + Health);
            Debug.Log("Name: " + Name);
            Debug.Log("Guard: " + Guard);
            
            Health.PropertyChanged += HealthAlert;
            Name.PropertyChanged += NameAlert;
            Guard.PropertyChanged += GuardAlert;
            Inventory.PropertyChanged += InventoryAlert;
        }

        private void InventoryAlert(List<string> list)
        {
            Debug.Log("Inventory changed: " + list.Count);
        }

        [ContextMenu("Random")]
        public void Random()
        {
            referenceModel.RandomValues();
            
            //Note that update checks must be done before the update goes through because the alert is only happening after the value changes
            if (Guard.Value != referenceModel.Guard)
            {
                Debug.Log("Guard is changing");
            }
            SetupValuesFromModel(referenceModel);
        }
        
        // Changes to observables *must* be explicit, which is correct
        private bool SetupValuesFromModel(PlayerModel playerModel)
        {
            Health.Value = playerModel.Health;
            Name.Value = playerModel.Name;
            Guard.Value = playerModel.Guard;
            Inventory.Value = playerModel.Inventory;
            
            return true;
        }

        public void HealthAlert(int i)
        {
            Debug.Log("Health Changed: " + i);
        }
        public void NameAlert(string s)
        {
            Debug.Log("Name Changed to " + s);
        }
        
        //Inherently flawed, the alert is only ever called when the property *changes*
        public void GuardAlert(bool guarded)
        {
            Debug.Log("Guard is " + Guard);
        }
        
        private PlayerModel WriteToModel(PlayerModel targetModel)
        {
            targetModel.Health = Health;
            targetModel.Name = Name;
            targetModel.Guard = Guard;
            targetModel.Inventory = Inventory.Value;
            return targetModel;
        }

        public void Dispose()
        {
            referenceModel = null;
            Health.PropertyChanged -= HealthAlert;
            Name.PropertyChanged -= NameAlert;
            Guard.PropertyChanged -= GuardAlert;
            Inventory.PropertyChanged -= InventoryAlert;
            Health = null;
            Name = null;
            Guard = null;
            Inventory = null;
        }
    }

}