using MetaInfo;
using System.Collections.Generic;
using UnityEngine;

namespace BoardStuff
{
    public class CharacterManager : MonoBehaviour
    {
        class CharacterInfo
        {
            public int id;

            public bool isBottom;

            public Transform charPrefab;

            public List<Transform> digits;

            public CharacterInfo(int id, bool isBottom, Transform charPrefab, List<Transform> digits)
            {
                this.id = id;
                this.isBottom = isBottom;
                this.charPrefab = charPrefab;
                this.digits = digits;
            }
        }

        // Character game objects
        public Transform characterRed;

        public Transform characterBlue;

        public Transform characterGreen;

        // Digits game objects
        public Transform[] powerDigits;

        // Character game objects (holded in structure)
        private Dictionary<StuffClass, Transform> charGameObjects;

        // Characters on cells
        private Dictionary<int, List<CharacterInfo>> characters;

        // Inforamtion about characters' cells (charId -> cellId)
        private Dictionary<int, int> charsCells;

        void Start()
        {
            // Forming the character game objects structure
            charGameObjects = new Dictionary<StuffClass, Transform>();

            charGameObjects.Add(StuffClass.IASA, characterRed);
            charGameObjects.Add(StuffClass.FICT, characterBlue);
            charGameObjects.Add(StuffClass.FPM, characterGreen);

            characters = new Dictionary<int, List<CharacterInfo>>();

            charsCells = new Dictionary<int, int>();
        }

        private CharacterInfo FindCharacter(int characterId)
        {
            // Finding the character
            CharacterInfo character = null;

            foreach (CharacterInfo charInfo in characters[charsCells[characterId]])
            {
                if (charInfo.id == characterId)
                {
                    character = charInfo;
                    break;
                }
            }

            return character;
        }

        // TODO: Add more than 2 characters on cell support
        public void SpawnCharacter(int cellId, int characterId, StuffClass stuffClass,
            int power, bool toBottom)
        {
            Vector3 charCoords = BoardCoordinates.GetCharacterCoords(cellId, toBottom);

            Transform charPrefab = Instantiate(
                charGameObjects[stuffClass], charCoords, Quaternion.identity);

            // Getting the digits
            List<Transform> charDigits = new List<Transform>();

            int digitsQuan = (int)Mathf.Log10(power) + 1;
            int currPower = power;

            foreach (Vector3 coords in BoardCoordinates.GetCharDigitsCoords(charCoords, digitsQuan))
            {
                charDigits.Add(Instantiate(
                    powerDigits[currPower % 10], coords, Quaternion.identity));

                currPower /= 10;
            }

            CharacterInfo charInfo = new CharacterInfo(characterId, toBottom, charPrefab, charDigits);

            // Adding character to the board info
            if (!characters.ContainsKey(cellId))
            {
                characters.Add(cellId, new List<CharacterInfo>());
            }

            characters[cellId].Add(charInfo);

            charsCells.Add(characterId, cellId);
        }

        // TODO: Add more than 2 characters on cell support
        public void RemoveCharacter(int characterId)
        {
            CharacterInfo character = FindCharacter(characterId);

            // Destroying the character
            foreach (Transform trans in character.digits)
            {
                Destroy(trans);
            }
            Destroy(character.charPrefab);

            // Removing character from structure
            characters[charsCells[characterId]].Remove(character);
        }

        public void ChangeCharacterPower(int characterId, int newPower)
        {
            CharacterInfo character = FindCharacter(characterId);
            
            // Destroying old digits
            foreach (Transform trans in character.digits)
            {
                Destroy(trans);
            }

            character.digits.Clear();

            // Drawing new digits
            int digitsQuan = (int)Mathf.Log10(newPower) + 1;
            int currPower = newPower;

            foreach (Vector3 coords in BoardCoordinates.GetCharDigitsCoords(
                character.charPrefab.position, digitsQuan))
            {
                character.digits.Add(Instantiate(
                    powerDigits[currPower % 10], coords, Quaternion.identity));

                currPower /= 10;
            }
        }
    }
}