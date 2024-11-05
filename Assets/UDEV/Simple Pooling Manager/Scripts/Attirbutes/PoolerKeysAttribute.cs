using UnityEngine;
using System;

namespace UDEV.SPM
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true)]
    public class PoolerKeysAttribute : PropertyAttribute
    {

        public PoolerTarget target;

        public PoolerKeysAttribute(PoolerTarget _target)
        {
            target = _target;
        }

        public PoolerKeysAttribute()
        {
        }
    }
}
