using System;
using System.Collections.Generic;
using GameStuff;
using MetaInfo;

public class PlayerController : PlayerInfo
{
    StuffClass stuffClass;

    List<Card> cards;

    List<Check> checks;

    List<Card> cardsPlayed;

    List<Check> checksDead;

    List<int> battlesHistory;

    public PlayerController(StuffClass stuffClass, List<Card> cards, List<Check> checks)
    {
        this.stuffClass = stuffClass;
        this.checks = checks;
        this.cards = cards;
        cardsPlayed = new List<Card>();
        checksDead = new List<Check>();
        battlesHistory = new List<int>();
    }

    public void AddCardToHand(Card card)
    {
        cards.Add(card);
    }

    public void AddCheckToHand(Check check)
    {
        checks.Add(check);
    }

    public List<int> GetBattlesHistory()
    {
        return battlesHistory;
    }

    public List<Card> GetCardsInHand()
    {
        return cards;
    }

    public List<Card> GetCardsPlayed()
    {
        return cardsPlayed;
    }

    public List<Check> GetChecksDead()
    {
        return checksDead;
    }

    public List<Check> GetChecksInHand()
    {
        return checks;
    }

    public void RemoveCardFromHand(Card card)
    {
        cards.Remove(card);
    }

    public void RemoveCheckFromHand(Check check)
    {
        checks.Remove(check);
    }

    public void SetPlayingCardAction(Action<Card> action)
    {
        throw new NotImplementedException();
    }
}