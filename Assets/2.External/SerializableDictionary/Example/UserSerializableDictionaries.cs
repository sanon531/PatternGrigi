using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PG.Data;
[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class StringAudioDictionary : SerializableDictionary<string, AudioClip> { }
[Serializable]
public class ObstacleIDObjectDic : SerializableDictionary<ObstacleID, GameObject> { }
[Serializable]
public class ActionDataDic : SerializableDictionary<EnemyAction, ActionData> { }




[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}