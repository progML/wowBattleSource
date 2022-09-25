using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Presenter.Misc;
using ServerRest;
using ServerRest.ServerStatusInteractor;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ServerGameStateProvider : MonoBehaviour
    {

        //есть/нет пробития
        public bool TryAttackLine(int lineNum, Action<ErrorType> onError,Action<bool> callback)
        {
            ServerInteractor.Instance.PostFight(lineNum, onError, callback);
            return true;
        }

        public int GetMaxAttackCount()
        {
            return ServerGameStatus.Instance.staticGameParams.maxTurns;
        }

        public int GetRemainingAttackCount()
        {
            return ServerGameStatus.Instance.playerData.attackCount;
        }

        public int GetMaxHealth()
        {
            return ServerGameStatus.Instance.staticGameParams.maxHp;
        }

        public int GetCurrentHealth()
        {
            return ServerGameStatus.Instance.enemyData.currentHealth;
        }

        public int GetCurrentGameLineCount()
        {
            return ServerGameStatus.Instance.staticGameParams.baseLen;
        }

        public ServeGameState GetServerGameState()
        {
            return ServerGameStatus.Instance.serveGameState;
        } 

        public UserUIData GetAttacker()
        {
            return new UserUIData(ServerGameStatus.Instance.playerData.icon, ServerGameStatus.Instance.playerData.login);
        }

        public UserUIData GetDefender()
        {
            return new UserUIData(ServerGameStatus.Instance.enemyData.icon, ServerGameStatus.Instance.enemyData.login);
        }
    }

    public enum ServeGameState
    {
        
        Playing, //Еще нужно атаковать
        Victory,
        Defeat
    }
}