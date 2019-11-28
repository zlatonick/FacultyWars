using BoardStuff;
using GameEngine;
using GameStuff;
using MetaInfo;
using Preparing;
using System.Collections.Generic;
using UnityEngine;

namespace Match
{
    public class GameController : MonoBehaviour
    {
        public CardsManager cardsManager;

        public CheckManager checkManager;

        public BoardStuffManager boardStuffManager;

        private Board board;

        private MatchController matchController;

        private Player player;

        private Player opponent;

        private PlayerInfo playerInfo;

        private Engine engine;

        void Start()
        {
            // DEBUG. Change to a real StuffPack
            StuffPack.stuffClass = StuffClass.FICT;            
            StuffPack.cards = new List<Card>();
            StuffPack.checks = new List<Check>();

            // DEBUG
            CheckFactory checkFactory = new CheckFactoryImpl();

            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 2));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 2));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 2));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 0));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffClass.FICT, 0));

            board = new BoardController(boardStuffManager, 4);

            checkManager.SetCellInThePlacePredicate(board.GetCellByCoords);

            // Setting up the checks
            Dictionary<int, Check> checkLevels = new Dictionary<int, Check>();

            foreach (Check check in StuffPack.checks)
            {
                if (!checkLevels.ContainsKey(check.GetLevel()))
                {
                    checkLevels.Add(check.GetLevel(), check);
                }
            }

            // Setting up the player
            player = new Player(0, StuffPack.stuffClass);
            playerInfo = new PlayerController(cardsManager, checkManager, checkLevels);

            foreach (Card card in StuffPack.cards)
            {
                playerInfo.AddCardToHand(card);
            }
            foreach (Check check in StuffPack.checks)
            {
                playerInfo.AddCheckToHand(check);
            }

            // Setting up the opponent (AI)
            engine = EngineCreator.CreateEngine();
            opponent = new Player(1, engine.GetStuffClass());

            // Setting up the match controller
            matchController = new MatchControllerImpl(board,
                player, playerInfo, opponent, /*engine.GetPlayerInfo()*/ null);

            playerInfo.SetCheckPlacedAction(CheckPlaced);

            // Starting the game
            if (matchController.GetCurrMovingPlayer() == player)
            {
                StartPlayerTurn();
            }
            else
            {
                StartOpponentTurn();
            }
        }

        private void StartPlayerTurn()
        {
            // TODO: Make "Your turn" animation

            // Allow user to play cards and characters
            playerInfo.SetActionsPermission(true);
            playerInfo.SetAllowedCardTypes(matchController.GetAllowedCardTypes());
            playerInfo.SetAllowedCharacters(matchController.AreCharactersAllowed());
        }

        private void StartOpponentTurn()
        {
            if (matchController.IsBattleNow())
            {
                Card card = engine.MakeBattleMove();
                matchController.PlayCard(card);
            }
            else
            {
                // TODO: Add non-battle card moves
                PlayerMove move = engine.MakeMove();
                matchController.PlaceCheck(move.check, move.cell);
            }

            if (matchController.GetCurrMovingPlayer() == opponent)
            {
                StartOpponentTurn();
            }
            else
            {
                StartPlayerTurn();
            }
        }

        private void UpdatePlayerTurn()
        {
            if (matchController.GetCurrMovingPlayer() == player)
            {
                playerInfo.SetAllowedCardTypes(matchController.GetAllowedCardTypes());
                playerInfo.SetAllowedCharacters(matchController.AreCharactersAllowed());
            }
            else
            {
                playerInfo.SetActionsPermission(false);
            }
        }

        public void CardPlayed(Card card)
        {
            matchController.PlayCard(card);

            UpdatePlayerTurn();
        }

        public void CheckPlaced(Check check, Cell cell)
        {
            matchController.PlaceCheck(check, cell);

            UpdatePlayerTurn();
        }

        public void FinishTurn()
        {
            StartOpponentTurn();
        }
    }
}