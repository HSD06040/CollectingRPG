using System;

[Serializable]
public class Property<T>
{
    private T _value;

    public T Value
    {
        get => _value;
        set 
        {
            _value = value; 

            _onChange?.Invoke(_value); 
        }
    }

    private event Action<T> _onChange;

    public void AddEvent(Action<T> action)
    {
        _onChange += action;
    }

    public void RemoveEvent(Action<T> action)
    {
        _onChange -= action;
    }
}
