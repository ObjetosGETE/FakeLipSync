using UnityEngine;
using UnityEngine.UI;

namespace SharkJets
{
    public class DemoScript : MonoBehaviour
    {
        //get a reference to the VisSoundSync component
        [SerializeField] private VisSoundSync soundSync;
        
        private bool isOn;
        Color lerpedColor = Color.blue;

        void Update()
        {
            if (isOn)
            {
                lerpedColor = Color.Lerp(Color.blue, Color.black, Mathf.PingPong(Time.time, 1));
                GetComponent<Image>().color = lerpedColor;
                
            }
        }
        
        void Start()
        {
            //subscribe to event notifications when a line is shown
            soundSync.OnNextLineEvent += DoSomething;
        }

        private void DoSomething(int lineNumber, string lineText)
        {
            if (lineNumber == 15) isOn = true;
            if (lineNumber == 17) isOn = false;
        }
    }
    
}