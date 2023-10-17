using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<T,U> :Dictionary<T,U>, ISerializationCallbackReceiver
{
    [SerializeField] 
    List<T> _keys = new List<T>();
    [SerializeField] 
    List<U> _values = new List<U>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach(KeyValuePair<T,U> pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value); 
        }
    }
    public void OnAfterDeserialize() 
    { 
        this.Clear(); 

        if (_keys.Count != _values.Count) 
            throw new System.Exception(string.Format("there are 0 keys and 1 values after deserialization. Make sure that both key and value types are serializable.")); 
        for (int i = 0; i < _keys.Count; i++) 
            this.Add(_keys[i], _values[i]); 
    }

}
