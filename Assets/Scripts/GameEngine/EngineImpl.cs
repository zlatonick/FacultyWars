using BoardStuff;
using GameStuff;
using MetaInfo;
using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class EngineImpl : Engine
    {
        private StuffClass stuffClass;

        private MatchController controller;

        private CardFactory cardFactory;

        private CheckFactory checkFactory;

        private EnginePlayerController playerInfo;

        private Random random;

        private List<int> availableCells;

        public EngineImpl(StuffClass stuffClass, Dictionary<int, Check> checkLevels)
        {
            this.stuffClass = stuffClass;
            playerInfo = new EnginePlayerController(stuffClass, checkLevels);

            // Determined start pack
            cardFactory = new CardFactoryImpl();
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 0));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 0));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 1));

            checkFactory = new CheckFactoryImpl();
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 0));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));

            // Some settings
            random = new Random();
            availableCells = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
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

            int cellId = availableCells[random.Next(availableCells.Count)];
            availableCells.Remove(cellId);

            Cell cell = controller.GetCellById(cellId);

            return new PlayerMove(check, cell);
        }
    }
}