using System;
using System.Collections.Generic;
using System.ComponentModel; //Only used in full implementation of INotifyPropertyChanged
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices; //Only used in full implementation of INotifyPropertyChanged
using UnityEngine;
using Random = System.Random;

/*
 *
 * A lot of what is in here is from a mixture of Rider snippets and MicrosoftLearn and StackOverflow reading
 * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-7.0
 * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventhandler?view=net-7.0
 * https://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface
 * https://softwareengineering.stackexchange.com/questions/135413/should-a-view-and-a-model-communicate-or-not
 * https://medium.com/android-news/mvvm-how-view-and-viewmodel-should-communicate-8a386ce1bb42
 * https://forums.swift.org/t/in-mvvm-is-it-ok-for-your-model-to-be-an-observableobject/60355
 * There remains a big question what "the model" is really meant to be in Unity's case.
 * </summary>
 */
namespace Code.ObservableTest2
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
    /// </summary>
    /// <typeparam name="T">Basic data type</typeparam>
    public class Observable<T> : ObservableBase<T>
    {
        
        private T _value;
        public T Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        //Using Observables in basic evaluations is fine, we cannot do the same the other way.
        public static implicit operator int(Observable<T> i) => i.Value as int? ?? 0;
        public static implicit operator bool(Observable<T> i) => i.Value as bool? ?? false;
        public static implicit operator float(Observable<T> i) => i.Value as float? ?? 0f;
        public static implicit operator string(Observable<T> i) => i.Value.ToString();
        public static implicit operator char(Observable<T> i) => i.Value as char? ?? ' ';
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
    public abstract class ViewModelSimpleBase : MonoBehaviour, IDisposable
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
        public PlayerModel(string name, int health, bool guard)
        {
            Health = health;
            Name = name;
            Guard = guard;
        }
        
        public void RandomValues()
        {
            Random random = new Random();
            Health = random.Next(0,100);
            Name = random.Next(0, 100).ToString();
            Guard = random.Next(0, 100) > 50;
        }
    }
    
    public class VM : ViewModelSimpleBase
    {
        private PlayerModel referenceModel;

        public Observable<int> Health { get; set; } = new Observable<int>();
        public Observable<string> Name { get; set; } = new Observable<string>();
        public Observable<bool> Guard { get; set; } = new Observable<bool>();

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
        }
        
        [ContextMenu("Random")]
        public void Random()
        {
            referenceModel.RandomValues();
            SetupValuesFromModel(referenceModel);
        }
        
        // Changes to observables *must* be explicit, which is correct
        private bool SetupValuesFromModel(PlayerModel playerModel)
        {
            Health.Value = playerModel.Health;
            Name.Value = playerModel.Name;
            Guard.Value = playerModel.Guard;
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
        private void GuardAlert(bool guarded)
        {
            Debug.Log("Guard Changed: " + guarded);
        }
        
        private PlayerModel WriteToModel()
        {
            referenceModel.Health = Health;
            referenceModel.Name = Name;
            referenceModel.Guard = Guard;
            return referenceModel;
        }

        public override void Dispose()
        {
            referenceModel = null;
            Health.PropertyChanged -= HealthAlert;
            Name.PropertyChanged -= NameAlert;
            Guard.PropertyChanged -= GuardAlert;
            Health = null;
            Name = null;
            Guard = null;
        }
    }

}