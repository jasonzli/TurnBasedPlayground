using System;
using System.Collections.Generic;
using Code.ScriptableObjects;

namespace Code.BattleSystem
{     
    /// <summary>
    /// This battle system tracks the actions of the battle and determines a winner.
    /// How the battle unfolds visually is on the conductor
    ///
    /// As such, this is very straight forward. It has battle actors, and can execute actions on the battle.
    /// </summary>
    public class BattleSystem
    {
        public IBattleActor PlayerOne { get; private set; }
        public IBattleActor PlayerTwo { get; private set; }

        //An action event to report who has won
        public Action<IBattleActor> BattleOver;
        public IBattleActor Winner { get; private set; }
        
        public List<IBattleAction> ActionsTaken { get; set; }

        public BattleSystem(ActorData playerOneData, ActorData playerTwoData)
        {
            //Create BattleActors
            PlayerOne = CreateActor(playerOneData);
            PlayerTwo = CreateActor(playerTwoData);
            
            //Create history of actions taken
            ActionsTaken = new List<IBattleAction>();
        }
        
        private IBattleActor CreateActor(ActorData actorData)
        {
            return new BattleActor(actorData.Name,actorData.Health, false);
        }
        
        /// <summary>
        /// If you have zero health, you lose the battle.
        /// No ties, no draws, no mercy.
        /// </summary>
        /// <returns>Winning BattleActor</returns>
        public IBattleActor EvaluateWinner()
        {
            if (PlayerOne.CurrentHP <= 0)
            {
                BattleOver?.Invoke(PlayerTwo);
                return PlayerTwo;
            }

            if (PlayerTwo.CurrentHP <= 0)
            {
                BattleOver?.Invoke(PlayerOne);
                return PlayerOne;
            }

            return null;
        }
        
        public bool PerformAction(IBattleAction action)
        {
            bool actionSuccessful = action.Execute();
            ActionsTaken.Add(action);
            
            if (!actionSuccessful)
            {
                return false;
            }
            Winner = EvaluateWinner();
            return true;
        }
        
        
        
    }
}