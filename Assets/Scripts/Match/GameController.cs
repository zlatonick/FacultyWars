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

        public Text opponentPanelChecks;
        public Text opponentPanelCards;

        public GameObject yourTurnText;

        public GameObject gameOverPanel;
        public Text gameOverWinnerText;

        public GameObject pauseMenu;

        private Board board;

        private MatchController matchController;

        private Player player;

        private Player opponent;

        private PlayerController playerInfo;

        private PlayerInfo opponentInfo;

        private Engine engine;

        private bool matchIsGoing;

        private bool isFinishMoveClickable;

        void Start()
        {
            // DEBUG. Change to a real StuffPack
            /*StuffPack.stuffClass = StuffClass.FICT;            
            StuffPack.cards = new List<Card>();
            StuffPack.checks = new List<Check>();

            // DEBUG
            CardFactory cardFactory = new CardFactoryImpl();

            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 0));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 1));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 2));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 3));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 4));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 5));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 6));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 7));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 8));
            StuffPack.cards.Add(cardFactory.GetCard(StuffPack.stuffClass, 9));

            // DEBUG
            CheckFactory checkFactory = new CheckFactoryImpl();

            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 0));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 0));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 1));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 2));
            StuffPack.checks.Add(checkFactory.GetCheck(StuffPack.stuffClass, 2));*/

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
            opponentInfo = engine.GetPlayerInfo();
            UpdateOpponentPanel();

            // Setting up the match controller
            matchController = new MatchControllerImpl(board, chooser, cardsDemonstrator,
                player, playerInfo, opponent, opponentInfo, GameOver);

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
            isFinishMoveClickable = false;

            if (matchController.GetCurrMovingPlayer() == player)
            {
                StartPlayerTurn();
            }
            else
            {
                StartOpponentTurn();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown("escape"))
            {
                pauseMenu.SetActive(true);
            }
        }

        private void UpdateOpponentPanel()
        {
            opponentPanelChecks.text = "x" + opponentInfo.GetChecksCount();
            opponentPanelCards.text = "x" + opponentInfo.GetCardsInHand().Count;
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
            yield return new WaitForSeconds(2);
            yourTurnText.SetActive(false);

            // Allow user to play cards and characters
            playerInfo.SetActionsPermission(true);
            playerInfo.SetAllowedCharacters(matchController.AreCharactersAllowed());
            isFinishMoveClickable = true;

            // Highlight available cards in hand
            playerInfo.UnhighlightCards();
            playerInfo.HighlightPlayableCards(matchController.GetAllowedCardTypes());
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
            UpdateOpponentPanel();

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

            UpdateOpponentPanel();
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

        public void CardPlayed(Card card)
        {
            Debug.Log("Player has played a card");

            matchController.PlayCard(card);

            playerInfo.UnhighlightCards();
            playerInfo.SetActionsPermission(false);

            isFinishMoveClickable = false;
            StartCoroutine(UnlockFinishTurnAfterFewSeconds(2.5f));
        }

        private IEnumerator UnlockFinishTurnAfterFewSeconds(float secQuan)
        {
            yield return new WaitForSeconds(secQuan);
            isFinishMoveClickable = true;
        }

        public void CheckPlaced(Check check, Cell cell)
        {
            Debug.Log("Player has placed a check");

            matchController.PlaceCheck(check, cell);

            playerInfo.UnhighlightCards();

            if (matchController.IsBattleNow())
            {
                playerInfo.SetActionsPermission(false);
            }
            else
            {
                playerInfo.SetAllowedCharacters(false);

                // Highlight available cards in hand
                playerInfo.HighlightPlayableCards(matchController.GetAllowedCardTypes());
            }
        }

        public void FinishTurn()
        {
            if (isFinishMoveClickable)
            {
                isFinishMoveClickable = false;

                matchController.FinishMove();
                playerInfo.UnhighlightCards();

                UpdateOpponentPanel();

                if (matchIsGoing)
                {
                    if (matchController.GetCurrMovingPlayer() == player)
                        StartPlayerTurn();
                    else
                        StartOpponentTurn();
                }
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
            StuffPack.cards.Clear();
            StuffPack.checks.Clear();
            SceneManager.LoadScene("MainMenu");
        }

        public void ClosePauseMenu()
        {
            pauseMenu.SetActive(false);
        }
    }
}