using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AbstractPathMind: MonoBehaviour
    {
        protected CharacterBehaviour character;
        public enum Phases { Searching, PathFound, NotPathFound }
        public Phases current_phase = Phases.Searching;
        public abstract void Repath();
        
        public abstract Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals);

        public void SetCharacter(CharacterBehaviour characterBehaviour)
        {
            this.character = characterBehaviour;
        }
    }
}
