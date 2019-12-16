using MetaInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Preparing
{
    public class ChooseFacultyEvent : MonoBehaviour
    {
        public void LoadScene(int stuffClassId)
        {
            switch (stuffClassId)
            {
                case 0:
                    StuffPack.stuffClass = StuffClass.IASA;
                    PreparingData.cardCount = 10;
                    break;
                
                case 1: 
                    StuffPack.stuffClass = StuffClass.FICT;
                    PreparingData.cardCount = 17;
                    break;
                
                case 2:
                    StuffPack.stuffClass = StuffClass.FPM;
                    PreparingData.cardCount = 19;
                    break;
            }
            
            Debug.Log("Loading Scene: Preparing");
            SceneManager.LoadScene("Preparing");
        }
    }
}