using MetaInfo;

namespace GameStuff
{
    public abstract class Card
    {
        private int id;

        private int price;

        private CardType cardType;

        private StuffClass stuffClass;

        private string text;

        private bool isChoosing;

        protected Card(int id, int price, CardType cardType,
            StuffClass stuffClass, bool isChoosing, string text)
        {
            this.id = id;
            this.price = price;
            this.cardType = cardType;
            this.stuffClass = stuffClass;
            this.text = text;
            this.isChoosing = isChoosing;
        }

        public int GetId()
        {
            return id;
        }

        public int GetPrice()
        {
            return price;
        }

        public CardType GetCardType()
        {
            return cardType;
        }

        public StuffClass GetStuffClass()
        {
            return stuffClass;
        }

        public string GetText()
        {
            return text;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public abstract void Choose(Chooser chooser);

        public abstract void Act(Battle battle, MatchController controller);
    }
}
