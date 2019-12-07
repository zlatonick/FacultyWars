using MetaInfo;

namespace GameStuff
{
    public class CardFict12 : Card
    {
        Check chosenCheck;

        public CardFict12()
            : base(12, 50, CardType.SILVER, StuffClass.FICT, true,
                  "Вы добавляете в текущий бой еще одного персонажа из руки")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            controller.PlaceCheck(chosenCheck, battle.GetCell());
        }

        public override void Choose(Chooser chooser)
        {
            chosenCheck = chooser.ChooseCheck("Выберите персонажа из руки");
        }
    }
}