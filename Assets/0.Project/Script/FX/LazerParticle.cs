using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Battle
{
    public class LazerParticle : Hovl_Laser2
    {
        [Header("Position")]
        public Vector3 _StartPos;
        public Vector3 _EndPos;


        void Start()
        {
            laserPS = GetComponent<ParticleSystem>();
            laserMat = GetComponent<ParticleSystemRenderer>().material;
            Flash = FlashEffect.GetComponentsInChildren<ParticleSystem>();
            Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
            laserMat.SetFloat("_Scale", laserScale);

            transform.position = _StartPos;
        }

        void Update()
        {
            if (laserPS != null && UpdateSaver == false)
            {
                laserMat.SetVector("_StartPoint", _StartPos);

                var distance = Vector3.Distance(_EndPos, _StartPos);
                particleCount = Mathf.RoundToInt(distance / (2 * laserScale));
                if (particleCount < distance / (2 * laserScale))
                {
                    particleCount += 1;
                }
                particlesPositions = new Vector3[particleCount];
                AddParticles();

                laserMat.SetFloat("_Distance", distance);
                laserMat.SetVector("_EndPoint", _EndPos);

                transform.LookAt(_EndPos);

                if (Hit != null)
                {
                    HitEffect.transform.position = _EndPos;
                    HitEffect.transform.LookAt(_EndPos);
                    foreach (var AllHits in Hit)
                    {
                        if (!AllHits.isPlaying) AllHits.Play();
                    }
                    foreach (var AllFlashes in Flash)
                    {
                        if (!AllFlashes.isPlaying) AllFlashes.Play();
                    }
                }
            }

            if (startDissovle)
            {
                dissovleTimer += Time.deltaTime;
                laserMat.SetFloat("_Dissolve", dissovleTimer * 5);
            }
        }
    }
}