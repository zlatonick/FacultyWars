using BoardStuff;
using GameEngine;
using GameStuff;
using MetaInfo;
using Preparing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Match
{
    public class GameController : MonoBehaviour
    {
        public CardsManager cardsManager;

        public CheckManager checkManager;

        public CardsDemonstrator cardsDemonstrator;

        public BoardStuffManager boardStuffManager;

        public ChooserImpl chooser;

        public GameObject yourTurnText;

        public GameObject gameOverPanel;
        public Text gameOverWinnerText;

        private Board board;

        private MatchController matchController;

        private Player player;

        private Player opponent;

        private PlayerController playerInfo;

        private Engine engine;

        private bool matchIsGoing;

        void Start()
        {
            // DEBUG. Change to a real StuffPack
            StuffPack.stuffClass = StuffClass.FICT;            
            StuffPack.cards = new List<Card>();
            StuffPack.checks = new List<Check>();

            // DEBUG
            CardFactory cardFactory = new CardFactoryImpl();

            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 7));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 11));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 12));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 13));

            // DEBUG
            CheckFactory checkFactory = new CheckFactoryImpl();

            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 0));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 0));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 2));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 2));

            board = new BoardController(boardStuffManager, 4,
                chooser.CellClicked, chooser.CharacterClicked);

            // Setting up the checks
            Dictionary<int, Check> checkLevels = CheckLevels.GetCheckLevels(StuffPack.stuffClass);

            // Setting up the player
            player = new Player(0, StuffPack.stuffClass);
            playerInfo = new PlayerController(StuffPack.stuffClass,
                cardsManager, checkManager, checkLevels);

            foreach (Card card in StuffPack.cards)
            {
                playerInfo.AddCardToHand(card);
            }
            foreach (Check check in StuffPack.checks)
            {
                playerInfo.AddCheckToHand(check);
            }

            // Setting up the opponent (AI)
            engine = EngineCreator.CreateEngine();      // IASA by default
            opponent = new Player(1, engine.GetStuffClass());

            // Setting up the match controller
            matchController = new MatchControllerImpl(board, chooser, cardsDemonstrator,
                player, playerInfo, opponent, engine.GetPlayerInfo(), GameOver);

            engine.SetMatchController(matchController);

            checkManager.SetCellInThePlacePredicate(board.GetCellByCoords);
            checkManager.SetCanPlaceCheckThere(matchController.CanPlaceCheckThere);
            checkManager.SetPlacableCellsFunctions(matchController.GetAllPlacableCells,
                board.HighlightCells, board.UnhighlightCells);

            playerInfo.SetCheckPlacedAction(CheckPlaced);
            playerInfo.SetCheckClickedAction(chooser.CheckClicked);
            playerInfo.SetCardPlayedAction(CardPlayed);
            playerInfo.SetCanPlayCardPredicate(matchController.GetAllowedCardTypes);

            // Starting the game
            matchIsGoing = true;

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
            Debug.Log("Player's turn");

            // "Your turn" animation
            yourTurnText.SetActive(true);
            Animator animator = yourTurnText.GetComponent<Animator>();
            animator.SetInteger("YourTurnParam", 2);
            animator.SetInteger("YourTurnParam", 1);
            StartCoroutine(HideYourTurnText());
        }

        private IEnumerator HideYourTurnText()
        {
            yield return new WaitForSeconds(3);
            yourTurnText.SetActive(false);

            // Allow user to play cards and characters
            playerInfo.SetActionsPermission(true);
            playerInfo.SetAllowedCharacters(matchController.AreCharactersAllowed());
        }

        private IEnumerator StartPlayerTurnAfterFewSeconds(float secQuan)
        {
            yield return new WaitForSeconds(secQuan);
            StartPlayerTurn();
        }

        private void StartOpponentTurn()
        {
            Debug.Log("Computer's turn");

            int computersTurn = 0;

            if (matchController.IsBattleNow())
            {
                Card card = engine.MakeBattleMove();

                if (card != null)
                {
                    Debug.Log("Computer played a card");
                    matchController.PlayCard(card);
                    computersTurn = 1;
                }
                else
                {
                    Debug.Log("Computer skipped his turn");
                }
            }
            else
            {
                // TODO: Add non-battle card moves
                PlayerMove move = engine.MakeMove();

                if (move != null)
                {
                    Debug.Log("Computer placed a check");
                    matchController.PlaceCheck(move.check, move.cell);
                    computersTurn = 2;
                }
                else
                {
                    Debug.Log("Computer skipped his turn");
                }
            }
            matchController.FinishMove();

            if (!matchIsGoing) return;

            if (matchController.GetCurrMovingPlayer() == opponent)
            {
                StartOpponentTurn();
            }
            else
            {
                if (computersTurn == 1)
                    StartCoroutine(StartPlayerTurnAfterFewSeconds(2.5f));
                else if (computersTurn == 2)
                    StartCoroutine(StartPlayerTurnAfterFewSeconds(1));
                else
                    StartPlayerTurn();
            }
        }

        private void UpdatePlayerTurn()
        {
            playerInfo.SetActionsPermission(false);
        }

        public void CardPlayed(Card card)
        {
            Debug.Log("Player has played a card");

            matchController.PlayCard(card);

            if (card.GetCardType() != CardType.NO_BATTLE)
            {
                playerInfo.SetActionsPermission(false);
            }
        }

        public void CheckPlaced(Check check, Cell cell)
        {
            Debug.Log("Player has placed a check");

            matchController.PlaceCheck(check, cell);

            if (matchController.IsBattleNow())
            {
                playerInfo.SetActionsPermission(false);
            }
            else
            {
                playerInfo.SetAllowedCharacters(false);
            }
        }

        public void FinishTurn()
        {
            matchController.FinishMove();

            if (matchIsGoing)
            {
                if (matchController.GetCurrMovingPlayer() == player)
                    StartPlayerTurn();
                else
                    StartOpponentTurn();
            }
        }

        public void GameOver(Player winner)
        {
            matchIsGoing = false;
            playerInfo.SetActionsPermission(false);

            gameOverPanel.SetActive(true);

            if (winner == player)
            {
                Debug.Log("Game over. Player wins");
                gameOverWinnerText.text = "Победитель - игрок";
            }
            else
            {
                Debug.Log("Game over. Computer wins");
                gameOverWinnerText.text = "Победитель - компьютер";
            }
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}