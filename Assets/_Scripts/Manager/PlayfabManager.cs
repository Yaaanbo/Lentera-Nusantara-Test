using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using Clicker.ScriptableObjects;
using MyBox;

namespace Clicker.Manager
{
    public class PlayfabManager : Utilities.Singleton<PlayfabManager>
    {
        private const string PLAYER_DATA_KEY = "Player data";

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
            GetPlayerData();
            Debug.Log("Logged in");
        }

        private void OnLogInFailed(PlayFabError error)
        {
            Debug.Log("Error Loggin In : " + error);
        }
        #endregion

        #region Saving Handler
        [ButtonMethod]
        public void SavePlayerData()
        {
            //Set constructor for click count and multiplier
            PlayerData playerData = new PlayerData
            (
                GameManager.Instance.ClickCount,
                GameManager.Instance.ClickMultiplier,
                ItemEffectManager.Instance.ActiveItemList,
                QuestManager.Instance.ActiveQuestsList
            );

            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {PLAYER_DATA_KEY, JsonUtility.ToJson(playerData)}
                }
            };
            PlayFabClientAPI.UpdateUserData(request, OnDataSaved, OnDataFailedToSave);
        }

        private void OnDataSaved(UpdateUserDataResult result)
        {
            Debug.Log("Data Saved!");
        }

        private void OnDataFailedToSave(PlayFabError error)
        {
            Debug.Log("Data Failed To Save : " + error);
        }
        #endregion

        #region Loading Handler
        
        [ButtonMethod]
        private void GetPlayerData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnDataFailedToRecieved);
        }

        private void OnDataRecieved(GetUserDataResult result)
        {
            if(result != null)
            {
                PlayerData deserializedJSON = JsonUtility.FromJson<PlayerData>(result.Data[PLAYER_DATA_KEY].Value);

                //Load click count & multiplier
                GameManager.Instance.SetClickCount(deserializedJSON.clickCount);
                GameManager.Instance.SetClickMultiplier(deserializedJSON.multiplier);
                Debug.Log("Saved Click Count : " + deserializedJSON.clickCount);

                //Load active items
                ItemEffectManager.Instance.ActiveItemList.Clear();
                foreach (var item in deserializedJSON.activeItems)
                {
                    ItemEffectManager.Instance.AddItemEffect(item, false);
                }

                //Load active quests
                QuestManager.Instance.ActiveQuestsList.Clear();
                foreach (var quest in deserializedJSON.activeQuests)
                {
                    QuestManager.Instance.ActiveQuestsList.Add(quest);
                    quest.isActive = true;
                    quest.questGoal.currentAmount = deserializedJSON.currentAmount - 1;
                    quest.questGoal.AddCurrentAmount(() => { });
                }
            }
            
        }

        private void OnDataFailedToRecieved(PlayFabError error)
        {
            Debug.Log("Failed to recieve player data : " + error);
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
        public float currentAmount;

        public PlayerData(float clickCount, float multiplier, List<ItemSO> activeItems, List<QuestSO> activeQuests)
        {
            this.clickCount = clickCount;
            this.multiplier = multiplier;

            this.activeItems = activeItems;

            this.activeQuests = activeQuests;
        }
    }
}
