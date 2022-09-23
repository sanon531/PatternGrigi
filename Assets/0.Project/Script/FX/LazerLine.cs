using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle 
{

    public class LazerLine : Hovl_Laser, Data.ILazerOnoff
    {
        [Header("Position")]
        public Vector3 _StartPos;
        public Vector3 _EndPos;

        [SerializeField]
        LightningBoltScript _electric;

        void Start()
        {
            Laser = GetComponent<LineRenderer>();
            _electric = GetComponent<LightningBoltScript>();
            Effects = GetComponentsInChildren<ParticleSystem>();
            Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
            transform.position = _StartPos;

        }
        bool _active = true;
        public void SetActiveLazer(bool var)
        {
            _active = var;
            if(Laser ==null)
                Laser = GetComponent<LineRenderer>();
            Laser.enabled = var;
        }

        public void SetLazerEachPos(Vector3 _start, Vector3 _end)
        {
            _StartPos = _start;
            _EndPos = _end;
            _electric.StartPosition = _StartPos;
            _electric.EndPosition = _EndPos;
        }

        void Update()
        {
            Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
            Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));

            if (_active && Laser != null && UpdateSaver == false)
            {
                Laser.SetPosition(0, _StartPos);
                Laser.SetPosition(1, _EndPos);
                HitEffect.transform.position = _EndPos;
                HitEffect.transform.LookAt(_EndPos);

                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }

                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, _EndPos));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, _EndPos));

                if (Laser.enabled == false && LaserSaver == false)
                {
                    LaserSaver = true;
                    Laser.enabled = true;
                }
            }
        }

    }

}
