using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG 
{
    public class Affector
    {
        public bool IsEnabled { get; private set; } = false;
        protected virtual void Enable() { IsEnabled = true; }
        protected virtual void Disable() { IsEnabled = false; }
    }

}
