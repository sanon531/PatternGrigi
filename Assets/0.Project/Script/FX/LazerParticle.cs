using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class LazerParticle : Hovl_Laser2 , Data.ILazerOnoff
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

        bool _active = true;
        public void SetActiveLazer(bool var) 
        {
            _active = var;
        }
        public void SetLazerEachPos(Vector3 _start, Vector3 _end)
        {
            _StartPos = _start;
            _EndPos = _end;
        }


        void Update()
        {
            if (_active && laserPS != null && UpdateSaver == false)
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