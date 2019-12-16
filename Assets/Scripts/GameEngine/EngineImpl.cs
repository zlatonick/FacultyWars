using BoardStuff;
using GameStuff;
using MetaInfo;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class EngineImpl : Engine
    {
        private StuffClass stuffClass;

        private MatchController controller;

        private CardFactory cardFactory;

        private CheckFactory checkFactory;

        private EnginePlayerController playerInfo;

        private System.Random random;

        public EngineImpl(StuffClass stuffClass, Dictionary<int, Check> checkLevels)
        {
            this.stuffClass = stuffClass;
            playerInfo = new EnginePlayerController(stuffClass, checkLevels);

            // Determined start pack
            cardFactory = new CardFactoryImpl();

            checkFactory = new CheckFactoryImpl();
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));

            // Some settings
            random = new System.Random();
        }

        public void SetMatchController(MatchController controller)
        {
            this.controller = controller;
        }

        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }

        public StuffClass GetStuffClass()
        {
            return stuffClass;
        }

        public Card MakeBattleMove()
        {
            CardType maxCardType = controller.GetAllowedCardTypes();

            List<Card> cards = new List<Card>(playerInfo.GetCardsInHand());
            cards.RemoveAll(card => (int)card.GetCardType() < (int)maxCardType);

            if (cards.Count == 0) return null;

            Card cardPlay = cards[random.Next(cards.Count)];

            playerInfo.RemoveCardFromHand(cardPlay);
            return cardPlay;
        }

        public PlayerMove MakeMove()
        {
            // Choosing the cell
            List<Cell> availableCells = controller.GetAllPlacableCells();
            Cell cell = availableCells[random.Next(availableCells.Count)];

            // Choosing the check
            Dictionary<int, int> checks = playerInfo.GetChecksInHand();
            Check check = null;
            
            foreach (var pr in checks)
            {
                if (pr.Value > 0)
                {
                    check = checkFactory.GetCheck(stuffClass, pr.Key);
                    playerInfo.RemoveCheckFromHand(pr.Key);
                    break;
                }
            }

            if (check == null) return null;

            return new PlayerMove(check, cell);
        }
    }
}