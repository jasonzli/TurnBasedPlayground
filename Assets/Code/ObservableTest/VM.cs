using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Code.ObservableTest2
{
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

    public interface ISimpleNotifyPropertyChanged
    {
        public Action PropertyChanged { get; set; }
    }
    
    public abstract class ViewModelSimpleBase : MonoBehaviour, ISimpleNotifyPropertyChanged, IDisposable
    {
        public Action PropertyChanged { get; set; }

        protected virtual void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke();
        }

        protected bool SetField<T>(ref T field, T value)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged();
            return true;
        }

        public void Dispose()
        {
            
        }
    }

    public abstract class ObservableBase<T>
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public Action PropertyChanged;

        protected virtual void NotifyPropertyChanged<T>(T value)
        {
            PropertyChanged?.Invoke();
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged<T>(value);
            return true;
        }
    }

    public class ObservableVal<T> : ObservableBase<T>
    {
        
        private T _value;
        public T Value
        {
            get => _value;
            private set => SetField(ref _value, value);
        }
        public static implicit operator ObservableVal<T>(int i) => new ObservableVal<T> {Value = (T) (object) i};
        public static implicit operator ObservableVal<T>(bool i) => new ObservableVal<T> {Value = (T) (object) i};
        public static implicit operator ObservableVal<T>(float i) => new ObservableVal<T> {Value = (T) (object) i};
        public static implicit operator ObservableVal<T>(string i) => new ObservableVal<T> {Value = (T) (object) i};
        public static implicit operator ObservableVal<T>(char i) => new ObservableVal<T> {Value = (T) (object) i};
        
        public static implicit operator int(ObservableVal<T> i) => i.Value as int? ?? 0;
        public static implicit operator bool(ObservableVal<T> i) => i.Value as bool? ?? false;
        public static implicit operator float(ObservableVal<T> i) => i.Value as float? ?? 0f;
        public static implicit operator string(ObservableVal<T> i) => i.Value.ToString();
        public static implicit operator char(ObservableVal<T> i) => i.Value as char? ?? ' ';
    }
    
    
    public class PlayerModel
    {
        private ObservableVal<int> _health;
        public ObservableVal<int>  Health
        {
            get => _health.Value;
            set => _health = value;
        }
        
        private ObservableVal<string> _name;
        public ObservableVal<string>  Name
        {
            get => _name.Value;
            set => _name = value;
        }
    }
    
    /*
     * What's better?
     *
     * But even if I did this, I'd still have to get a means of propagating that information *forward* to the view model
     * public class PlayerModel{
     *  public int Health {get;set;}
     *  public string Name {get;set;}
     * }
     */
    public class VM : ViewModelSimpleBase
    {
        private PlayerModel player;
        
        private int _health;
        public int Health
        {
            get => _health;
            set => SetField(ref _health, value);
        }
    
        [ContextMenu("Initialize")]
        public void Intialize()
        {
            player = new PlayerModel();
            player.Health = 100;
            Debug.Log("Health: " + player.Health);
            player.Name = "John";
            Debug.Log("Name: " + player.Name);
            player.Health.PropertyChanged += HealthAlert;
            player.Health= 50;
            
            player.Name.PropertyChanged += NameAlert;
            player.Name = "Bob";

            this.PropertyChanged += OtherAlert;
            
            Health = 10;

            player.Health = 20;

            Health = 10;

            Health = 12; //Note that data persists between runs
        }

        public void OtherAlert()
        {
            Debug.Log($"Other health changed {Health + player.Health}");
        }
        
        

        public void HealthAlert()
        {
            Debug.Log("Health Changed: " + player.Health.Value);
        }
        public void NameAlert()
        {
            Debug.Log("Name Changed");
        }
    }

}