using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructorController : MonoBehaviour
{

    [System.Serializable]
    public class ConstructorView
    {
        public List<GameObject> editingWordLettersGO = new List<GameObject>();
        public GameObject prefabLetterConstruct;
        public Transform constructorTrans;
        public Transform startPointConstructor;
        public Transform endPointConstructor;
        public float minDistanceBetweenObjects = 0.5f;

        public GameObject OnDragDetectedPanel;

        public void UpdateEditingWord(string _editingWord, GameManager _gameManager)
        {
            minDistanceBetweenObjects = 0.5f;

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
                tmp.GetComponent<LetterConstructor>().viewLetter.SetLetter(letter.ToString());

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
                editingWordLettersGO[i].transform.position = new Vector3(position.x, position.y, -0.2f);
            }
        }
        public void SetActivePanel(bool _desicion) 
        {
            OnDragDetectedPanel.SetActive(_desicion);
        }
    }
    public ConstructorView constructorView;

    public GameObject dragLetter; 


    // Start is called before the first frame update
    private void OnDisable()
    {
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragLetter") //Comprovar que es una letra en el futuro
        {
            constructorView.SetActivePanel(true);

            GameObject closest = FindClosestObject(collision.gameObject.transform.position, constructorView.editingWordLettersGO);
            if (closest != null)
            {
                foreach (GameObject letterConstrGO in constructorView.editingWordLettersGO)
                {
                    if (letterConstrGO == closest)
                    {
                        closest.GetComponent<LetterConstructor>().viewLetter.SetPositionMark(true);

                        collision.gameObject.GetComponent<LetterConstructor>().AddLetterAvaiable(true, closest.GetComponent<LetterConstructor>().index);
                    }
                    else
                    {
                        letterConstrGO.GetComponent<LetterConstructor>().viewLetter.SetPositionMark(false);
                    }
                }
            }
            else
            {
                collision.gameObject.GetComponent<LetterConstructor>().AddLetterAvaiable(true, 0);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragLetter") //Comprovar que es una letra en el futuro
        {
            constructorView.SetActivePanel(false);
            collision.gameObject.GetComponent<LetterConstructor>().AddLetterAvaiable(false);

            foreach (GameObject letterConstrGO in constructorView.editingWordLettersGO)
            {
                letterConstrGO.GetComponent<LetterConstructor>().viewLetter.SetPositionMark(false);        
            }
        }
    }

    private GameObject FindClosestObject(Vector3 position, List<GameObject> gameObjects)
    {
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;

        // Recorre todos los objetos para encontrar el más cercano.
        foreach (GameObject obj in gameObjects)
        {
            Vector3 directionToTarget = obj.transform.position - position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = obj;
            }
        }

        return closest;
    }
}
