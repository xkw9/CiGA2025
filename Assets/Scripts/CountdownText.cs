using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class CountdownText : MonoBehaviour
    {

        TextMeshProUGUI tmp;

        [SerializeField]
        MovingObject obj;

        private void Start()
        {

            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            transform.position = obj.transform.position; 
        }

        public void SetText(string txt)
        {
            tmp.text = txt;
        }

        public void Show()
        {
            transform.position = obj.transform.position;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public static CountdownText makeText(MovingObject obj)
        {
            var textPrefab = Resources.Load(Config.COUNTDOWN_TEXT_PREFAB);

            var uiRoot = GameObject.Find("Canvas");
            var textObj = Instantiate(textPrefab, uiRoot.transform).GetComponent<CountdownText>();
            textObj.obj = obj;

            return textObj;

        }
    }
}
