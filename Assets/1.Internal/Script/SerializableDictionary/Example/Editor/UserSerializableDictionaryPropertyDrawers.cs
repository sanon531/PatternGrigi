using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
[CustomPropertyDrawer(typeof(StringAudioDictionary))]
[CustomPropertyDrawer(typeof(ObstacleIDObjectDic))]
[CustomPropertyDrawer(typeof(EnemyActionDataDic))]
[CustomPropertyDrawer(typeof(QuaternionMyClassDictionary))]
[CustomPropertyDrawer(typeof(ArtifactIDArtifactDic))]
[CustomPropertyDrawer(typeof(ArtifactIDArtifactDataDic))]
[CustomPropertyDrawer(typeof(ArtifactIDListDic))]
[CustomPropertyDrawer(typeof(ArtifactIDVecotrInt2Dic))]
[CustomPropertyDrawer(typeof(CharactorIDDataEntityDic))]
[CustomPropertyDrawer(typeof(MobIDObjectDic))]
[CustomPropertyDrawer(typeof(MobActionDataDic))]
[CustomPropertyDrawer(typeof(ArtifactStringaDic))]
[CustomPropertyDrawer(typeof(TimeWaveDic))]
[CustomPropertyDrawer(typeof(StartConditionCampaignDic))]
[CustomPropertyDrawer(typeof(MobIDSpawnDataDic))]
[CustomPropertyDrawer(typeof(LaserIDObjectDic))]
[CustomPropertyDrawer(typeof(ProjectileIDObjectDic))]
[CustomPropertyDrawer(typeof(ProjectileIDObjectPoolDic))]
[CustomPropertyDrawer(typeof(ProjectileIDFloatDic))]
[CustomPropertyDrawer(typeof(ProjectileIDintDic))]
[CustomPropertyDrawer(typeof(ProjectileIDDataDic))]

public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
