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
public class ArtifactIDShowCaseDic : SerializableDictionary<ArtifactID, ArtifactShowCase> {}
[Serializable]
public class ArtifactIDArtifactDic: SerializableDictionary<ArtifactID, Artifact> 
{
    public override void CopyFrom(IDictionary<ArtifactID, Artifact> dict) 
    {
        this.Clear();
        foreach (var kvp in dict)
        {
            this[kvp.Key] = Artifact.Create(kvp.Key);
        }
    }

[Serializable]
public class MobIDObjectDic : SerializableDictionary<MobID, GameObject> { }
}

[Serializable]
public class CharactorIDDataEntityDic : SerializableDictionary<CharacterID, DataEntity>
{
    public void CopyFrom(CharactorIDDataEntityDic dict)
    {

        this.Clear();
        foreach (var kvp in dict)
        {
            Add(kvp.Key, new DataEntity(kvp.Value));
        }
    }

}



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