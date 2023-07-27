using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Code.ScriptableObjects;

namespace Code.BattleSystem
{     
    //This is a model
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
        private IBattleActor EvaluateWinner()
        {
            if (PlayerOne.CurrentHP == 0)
            {
                BattleOver?.Invoke(PlayerTwo);
                return PlayerTwo;
            }

            if (PlayerTwo.CurrentHP == 0)
            {
                BattleOver?.Invoke(PlayerOne);
                return PlayerOne;
            }

            return null;
        }
        
        public void PerformAction(IBattleAction action)
        {
            action.Execute();
            ActionsTaken.Add(action);
            Winner = EvaluateWinner();
        }

        
    }
}