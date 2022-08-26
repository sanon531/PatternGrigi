using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PG.Data;
using PG.Battle;
using PG;

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
public class ActionDataDic : SerializableDictionary<EnemyActionID, EnemyActionData> { }
[Serializable]
public class ArtifactIDShowCaseDic : SerializableDictionary<ArtifactID, ArtifactShowCase> 
{


}
[Serializable]
public class ArtifactIDArtifactDic: SerializableDictionary<ArtifactID, Artifact> { }



[Serializable]
public class ArtifactIDDataEntityDic : SerializableDictionary<CharacterID, DataEntity>, ISerializationCallbackReceiver { }



[Serializable]
public class MyClass
{
    public int i;
    public string str;
    public int i2;
    public string str2;
    public int i22;
    public string str22;

}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}