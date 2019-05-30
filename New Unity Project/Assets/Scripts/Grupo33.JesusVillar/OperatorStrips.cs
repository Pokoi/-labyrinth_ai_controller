using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    public class OperatorStrips
    {

        private CellInfo _cellInfo;

        private List<PropertyStrips> preconditions;
        private List<PropertyStrips> added_properties;
        private List<PropertyStrips> eliminated_properties;

        public OperatorStrips(List<PropertyStrips> pc, CellInfo target, string resultTag)
        {
            this._cellInfo = target;
            this.preconditions = pc;
            this.added_properties = new List<PropertyStrips>{new PropertyStrips(resultTag)};
        }

        public CellInfo GetCellInfo() { return _cellInfo; }

        public List<PropertyStrips> GetAddedProperties(){ return added_properties; }
        public List<PropertyStrips> GetPreconditions() { return preconditions; }
    }
}
