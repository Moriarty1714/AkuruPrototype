using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstructorController : MonoBehaviour
{
    [System.Serializable]
    public class ConstructorView
    {
        private List<GameObject> editingWordLettersGO = new List<GameObject>();
        public GameObject prefabLetterConstruct;
        public Transform constructorTrans;
        public Transform startPointConstructor;
        public Transform endPointConstructor;
        public float minDistanceBetweenObjects = 0.5f;

        public void UpdateEditingWord(string _editingWord, GameManager _gameManager)
        {
            foreach (GameObject letterGO in editingWordLettersGO)
            {
                Destroy(letterGO);
            }
            editingWordLettersGO.Clear();

            int i = 0;
            foreach (char letter in _editingWord)
            {
                GameObject tmp = Instantiate(prefabLetterConstruct, constructorTrans);
                tmp.name = i.ToString() + letter.ToString() + "-" + prefabLetterConstruct.name;
                tmp.GetComponent<LetterConstructor>().index = i;
                tmp.GetComponent<LetterConstructor>().SetLetter(letter.ToString());

                editingWordLettersGO.Add(tmp);
                i++;
            }
            PlaceObjectsInLine(startPointConstructor.position, endPointConstructor.position);
        }
        public void PlaceObjectsInLine(Vector2 pointA, Vector2 pointB)
        {
            Vector2 direction = (pointB - pointA).normalized;
            float totalLength = Vector2.Distance(pointA, pointB);
            float totalObjectLength = minDistanceBetweenObjects * (editingWordLettersGO.Count - 1);

            if (totalObjectLength > totalLength)
            {
                float scaleFactor = totalLength / totalObjectLength;
                foreach (GameObject go in editingWordLettersGO)
                {
                    go.transform.localScale *= scaleFactor;
                }
                totalObjectLength = totalLength;
                minDistanceBetweenObjects = totalLength / (editingWordLettersGO.Count - 1);
            }

            float padding = (totalLength - totalObjectLength) / 2;
            Vector2 startPoint = pointA + direction * padding;

            for (int i = 0; i < editingWordLettersGO.Count; i++)
            {
                Vector2 position = startPoint + direction * minDistanceBetweenObjects * i;
                editingWordLettersGO[i].transform.position = position;
            }
        }
    }
    public ConstructorView constructorView;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
