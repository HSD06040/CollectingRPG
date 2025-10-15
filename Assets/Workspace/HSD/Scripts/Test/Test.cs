using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Test<TKey, TValue> : Dictionary<TKey, TValue>
{
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public override void OnDeserialization(object sender)
    {
        base.OnDeserialization(sender);
    }
}
