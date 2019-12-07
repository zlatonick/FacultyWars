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
                    break;
                
                case 1: 
                    StuffPack.stuffClass = StuffClass.FICT;
                    break;
                
                case 2:
                    StuffPack.stuffClass = StuffClass.FPM;
                    break;
            }
            
            Debug.Log("Loading Scene: Preparing");
            SceneManager.LoadScene("Preparing");
        }
    }
}