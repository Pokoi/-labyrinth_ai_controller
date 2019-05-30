using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Locomotion))]
    public class CharacterBehaviour: MonoBehaviour
    {
        
        protected Locomotion LocomotionController;
        protected AbstractPathMind PathController;
        public BoardManager BoardManager { get; set; }
        protected CellInfo currentTarget;
        protected Planner PlanController;

        void Awake()
        {

            PathController = GetComponentInChildren<AbstractPathMind>();
            PathController.SetCharacter(this);
            LocomotionController = GetComponent<Locomotion>();
            LocomotionController.SetCharacter(this);

            PlanController = GetComponent<Planner>();
            PlanController.SetCharacter(this);           

        }

        void Update()
        {
            if (BoardManager == null) return;
            if (this.currentTarget == null)
            {
                if (PlanController.currentPlan.Count != 0) {
                    PlanController.currentPlan.RemoveAt(0);}

                CellInfo nextAction = PlanController.GetNextAction();
                this.SetCurrentTarget(nextAction);
            }
            if (LocomotionController.MoveNeed)
            {
                var boardClone = (BoardInfo)BoardManager.boardInfo.Clone();
                LocomotionController.SetNewDirection(PathController.GetNextMove(boardClone,LocomotionController.CurrentEndPosition(),new [] {this.currentTarget}));
                if(this.currentTarget == LocomotionController.CurrentEndPosition())
                {                    
                    SetCurrentTarget(null);
                    if (PlanController.currentPlan.Count != 0) { foreach (var property in PlanController.currentPlan[0].GetAddedProperties()) PlanController.AddProperty(property); }
                }
            }
        }

       

        public void SetCurrentTarget(CellInfo newTargetCell)
        {
            this.currentTarget = newTargetCell;
            LocomotionController.MoveNeed = true;
            PathController.current_phase = AbstractPathMind.Phases.Searching;
        }
    }
}

