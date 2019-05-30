using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{

    public class Planner : MonoBehaviour
    {
        private CharacterBehaviour _character;

        private List<OperatorStrips> currentPlan;
        private List<OperatorStrips> all_operators;
        
        // Start is called before the first frame update
        void Start()
        {
            currentPlan = new List<OperatorStrips>();
            all_operators = new List<OperatorStrips>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SetCharacter(CharacterBehaviour _character)
        {
            this._character = _character;
            
        }

        public CellInfo GetNextAction()
        {

            if(all_operators.Count == 0)
            {
                var l = this._character.BoardManager.boardInfo.ItemsOnBoard;
                foreach (PlaceableItem p in l)
                {
                    var lPC = new List<PropertyStrips>();
                    foreach( var pc in p.Preconditions) { lPC.Add(new PropertyStrips(pc.Tag));}
                    var target = this._character.BoardManager.boardInfo.CellWithItem(p.Tag);
                    all_operators.Add(new OperatorStrips(lPC, target, p.Tag));
                }
            }

            if (currentPlan.Count == 0)
            {
                //Aquí va strips planner
            }           
            
            var nextOperation = currentPlan[0];
            currentPlan.RemoveAt(0);
            return nextOperation.GetCellInfo();
            
        }
    }
}
