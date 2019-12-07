using BoardStuff;
using MetaInfo;
using System.Collections.Generic;

namespace GameStuff
{
    public class CardFict6 : Card
    {
        public CardFict6()
            : base(6, 40, CardType.NO_BATTLE, StuffClass.FICT, false,
                  "Вы получаете нейтральную карту «+10 к силе» в руку за каждого вашего персонажа на поле")
        { }

        public override void Act(Battle battle, MatchController controller)
        {
            List<Character> characters = controller.GetAllCharacters();
            characters.RemoveAll(character => character.GetPlayer() != battle.GetPlayer());

            PlayerInfo playerInfo = controller.GetPlayerInfo(battle.GetPlayer());

            for (int i = 0; i < characters.Count; i++)
            {
                playerInfo.AddCardToHand(new CardAll0(GetStuffClass()));
            }
        }

        public override void Choose(Chooser chooser) { }
    }
}