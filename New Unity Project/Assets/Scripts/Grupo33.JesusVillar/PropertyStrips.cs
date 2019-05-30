using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PropertyStrips
    {
        string _tag;

        public PropertyStrips (string tag)
        {
            _tag = tag;
        }
        public string GetTag() { return _tag; }
    }
}
