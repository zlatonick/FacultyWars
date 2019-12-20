using System;
using GameStuff;
using MetaInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Preparing
{
    public class StartMatchEvent : MonoBehaviour
    {
        public PickedShopScrollList scrollList;

        public void LoadSceneMode()
        {
            var checkFactory = new CheckFactoryImpl();
            var cardFactory = new CardFactoryImpl();

            foreach (var pickedShopItem in scrollList.itemList)
            {
                if (pickedShopItem.itemType.Equals("Card"))
                {
                    var card = cardFactory.GetCard(StuffPack.stuffClass, pickedShopItem.id);
                    StuffPack.cards.Add(card);
                }
                else if (pickedShopItem.itemType.Equals("Check"))
                {
                    var check = checkFactory.GetCheck(StuffPack.stuffClass, pickedShopItem.level);
                    StuffPack.checks.Add(check);
                }
            }

            Debug.Log("Loading Scene: Match");
            SceneManager.LoadScene("Match");
        }

        public void LoadMainScreen()
        {
            Debug.Log("Loading Scene: MainMenu");
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}