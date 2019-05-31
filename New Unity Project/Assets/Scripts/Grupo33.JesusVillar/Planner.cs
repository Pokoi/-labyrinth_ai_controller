using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{

    public class Planner : MonoBehaviour
    {
        private CharacterBehaviour _character;

        public List<OperatorStrips> currentPlan;
        private List<OperatorStrips> all_operators;
        private List<PropertyStrips> own_properties;

        private List<string> desired_tags;
        
        // Start is called before the first frame update
        void Start()
        {
            currentPlan    = new List<OperatorStrips>();
            all_operators  = new List<OperatorStrips>();
            own_properties = new List<PropertyStrips>();
            desired_tags = new List<string>();
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddProperty(PropertyStrips _p)
        {
            if (!own_properties.Contains(_p))
                own_properties.Add(_p);
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

            desired_tags.Add("Goal");

            Analize();                    
            
            var nextOperation = currentPlan[0];            
            return nextOperation.GetCellInfo();            
        }

        private void Analize()
        {
            currentPlan.Clear();            

            while (desired_tags.Count != 0)
            {
                foreach (var _operator in all_operators)
                {
                    var owned_tags = new List<string>();
                    foreach (var property in own_properties) owned_tags.Add(property.GetTag());

                    var iterator = 0;
                    var added_conditions = _operator.GetAddedProperties();  
                    
                    for (iterator = 0; iterator < _operator.GetAddedProperties().Count; iterator++)
                    {
                        if (desired_tags.Contains(added_conditions[iterator].GetTag()) && !currentPlan.Contains(_operator))
                        {
                            currentPlan.Add(_operator);
                            foreach (var condition in _operator.GetPreconditions())
                                if (!owned_tags.Contains(condition.GetTag()))
                                    desired_tags.Add(condition.GetTag());                            
                            foreach (var added in _operator.GetAddedProperties()) if (desired_tags.Contains(added.GetTag())) desired_tags.Remove(added.GetTag());
                        }
                    }            
                }
              
            }
            currentPlan.Reverse();
        }

        public void QuitGame()
        {
            // save any game data here
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
    }
}
