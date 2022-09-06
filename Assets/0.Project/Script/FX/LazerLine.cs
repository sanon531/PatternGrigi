using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Battle 
{
    public class LazerLine : Hovl_Laser
    {
        [Header("Position")]
        public Vector3 _StartPos;
        public Vector3 _EndPos;


        void Start()
        {
            Laser = GetComponent<LineRenderer>();
            Effects = GetComponentsInChildren<ParticleSystem>();
            Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();

            transform.position = _StartPos;
        }

        void Update()
        {
            Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
            Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));

            if (Laser != null && UpdateSaver == false)
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
