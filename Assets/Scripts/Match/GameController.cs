using BoardStuff;
using GameEngine;
using GameStuff;
using MetaInfo;
using Preparing;
using UnityEngine;

namespace Match
{
    public class GameController : MonoBehaviour
    {
        public Board board;

        public CardsManager cardsManager;

        private MatchController matchController;

        private Player player;

        private Player opponent;

        private PlayerInfo playerInfo;

        private Engine engine;

        void Start()
        {
            // Setting up the player
            player = new Player(0, StuffPack.stuffClass);
            playerInfo = new PlayerController(cardsManager);

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
                player, playerInfo, opponent, engine.GetPlayerInfo());

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