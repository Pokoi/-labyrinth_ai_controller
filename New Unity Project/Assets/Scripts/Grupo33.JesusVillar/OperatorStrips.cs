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

        private List<PropertyStrips> PC;
        private List<PropertyStrips> A;
        private List<PropertyStrips> E;

        public OperatorStrips(List<PropertyStrips> pc, CellInfo target, string resultTag)
        {
            this._cellInfo = target;
            this.PC = pc;
            this.A = new List<PropertyStrips>{new PropertyStrips(resultTag)};

        }

        public CellInfo GetCellInfo()
        {
            return null;
        }
    }
}
