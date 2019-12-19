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

        private int[] cardsQuan;

        public EngineImpl(StuffClass stuffClass, Dictionary<int, Check> checkLevels)
        {
            this.stuffClass = stuffClass;
            playerInfo = new EnginePlayerController(stuffClass, checkLevels);

            // Determined start pack
            cardFactory = new CardFactoryImpl();
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 0));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 0));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 0));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 2));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 3));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 4));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 5));
            playerInfo.AddCardToHand(cardFactory.GetCard(stuffClass, 6));

            cardsQuan = new int[10] { 3, 0, 1, 1, 1, 1, 1, 0, 0, 0 };

            checkFactory = new CheckFactoryImpl();
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 0));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 0));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 1));
            playerInfo.AddCheckToHand(checkFactory.GetCheck(stuffClass, 2));

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
            List<Battle> battles = controller.GetCurrBattles();

            int myTotalPower = battles[0].GetCharacter().GetPower();
            int opponentTotalPower = 0;
            int opponentsQuan = 0;

            foreach (Battle battle in battles)
            {
                opponentTotalPower += battle.GetEnemyCharacter().GetPower();
                opponentsQuan++;
            }

            if (myTotalPower > opponentTotalPower) return null;

            CardType maxCardType = controller.GetAllowedCardTypes();

            List<Card> cards = new List<Card>(playerInfo.GetCardsInHand());

            if (cards.Count == 0) return null;

            int powerDiffer = opponentTotalPower - myTotalPower;

            Card cardPlay = null;

            if (myTotalPower >= 70 && powerDiffer < 30 && cardsQuan[2] > 0 && maxCardType <= CardType.SILVER)
            {
                cardPlay = cards.Find(card => card.GetId() == 2);
                cardsQuan[2]--;
            }
            else if (powerDiffer < 20 && cardsQuan[0] > 0 && maxCardType <= CardType.SILVER)
            {
                cardPlay = cards.Find(card => card.GetId() == 0);
                cardsQuan[0]--;
            }
            else if (opponentTotalPower < 80 && cardsQuan[5] > 0 && maxCardType <= CardType.SILVER)
            {
                cardPlay = cards.Find(card => card.GetId() == 5);
                cardsQuan[5]--;
            }
            else if (opponentsQuan == 1 && cardsQuan[3] > 0 && maxCardType <= CardType.GOLD
                && battles[0].GetEnemyCharacter().GetStartPower() < battles[0].GetCharacter().GetStartPower())
            {
                cardPlay = cards.Find(card => card.GetId() == 3);
                cardsQuan[3]--;
            }
            else if (opponentsQuan == 1 && cardsQuan[4] > 0 && maxCardType <= CardType.GOLD)
            {
                cardPlay = cards.Find(card => card.GetId() == 4);
                cardsQuan[4]--;
            }
            else if (cardsQuan[6] > 0 && maxCardType <= CardType.GOLD)
            {
                cardPlay = cards.Find(card => card.GetId() == 6);
                cardsQuan[6]--;
            }

            if (cardPlay != null) playerInfo.RemoveCardFromHand(cardPlay);

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