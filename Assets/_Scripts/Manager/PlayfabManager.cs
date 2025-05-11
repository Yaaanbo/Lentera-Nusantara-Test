using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using Clicker.ScriptableObjects;

namespace Clicker.Manager
{
    public class PlayfabManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Login();
        }

        #region Login Handler
        private void Login()
        {
            var request = new LoginWithCustomIDRequest
            { 
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLogInSuccess, OnLogInFailed);
        }

        private void OnLogInSuccess(LoginResult result)
        {
            Debug.Log("Logged in");
        }

        private void OnLogInFailed(PlayFabError error)
        {
            Debug.Log("Error Loggin In : " + error);
        }
        #endregion

        #region Player Data Hander
        private void SavePlayerData()
        {

        }
        #endregion
    }

    public class PlayerData
    {
        //Click count & multipliers
        public float clickCount;
        public float multiplier;
        
        //Items
        public List<ItemSO> activeItems = new List<ItemSO>();

        //Quests
        public List<QuestSO> activeQuests = new List<QuestSO>();

        //Click count & multiplier constructor
        public PlayerData(float clickCount, float multiplier)
        {
            this.clickCount = clickCount;
            this.multiplier = multiplier;
        }

        //Active items constructor
        public PlayerData(List<ItemSO> activeItems)
        {
            this.activeItems = activeItems;
        }

        //Active quests constructor
        public PlayerData(List<QuestSO> activeQuests)
        {
            this.activeQuests = activeQuests;
        }
    }
}
