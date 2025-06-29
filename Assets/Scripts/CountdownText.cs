using Assets.Scripts.Utils;
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

        Vector2 offset = Vector2.zero;

        [SerializeField]
        GameObject obj;

        private void Awake()
        {

            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            transform.position = obj.transform.position + offset.toVec3(); 
        }

        public void SetText(string txt)
        {
            tmp.text = txt;
        }

        public void SetColor(Color color)
        {
            tmp.color = color;
        }

        public void SetFontSize(float size)
        {
            tmp.fontSize = size;
        }

        public void Show()
        {
            transform.position = obj.transform.position + offset.toVec3();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetOffset(Vector2 offset)
        {
            this.offset = offset;
        }

        public static CountdownText makeText(GameObject obj)
        {
            var textPrefab = Resources.Load(Config.COUNTDOWN_TEXT_PREFAB);

            var uiRoot = GameObject.Find("Canvas");
            var textObj = Instantiate(textPrefab, uiRoot.transform).GetComponent<CountdownText>();
            textObj.obj = obj;

            return textObj;

        }
    }
}
