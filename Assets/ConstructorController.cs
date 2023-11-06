using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructorController : MonoBehaviour
{
    public GameObject letterDragging = null;

    [System.Serializable]
    public class ConstructorView
    {
        public List<GameObject> editingWordLettersGO = new List<GameObject>();
        public GameObject prefabLetterConstruct;
        public Transform constructorTrans;
        public Transform startPointConstructor;
        public Transform endPointConstructor;
        public float minDistanceBetweenObjects = 0.5f;

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
    private void OnDisable()
    {
        LetterController.OnLetterDragGetReference -= GetReference;
    }
    void Start()
    {
        LetterController.OnLetterDragGetReference += GetReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetReference(GameObject _letterReference)
    {
        letterDragging = _letterReference;
        Debug.Log(_letterReference.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter");
        if (true) //Comprovar que es una letra en el futuro
        {
            GameObject closest = FindClosestObject(letterDragging.transform.position, constructorView.editingWordLettersGO);
            
            // Activa todos los objetos y desactiva el más cercano.
            foreach (GameObject obj in constructorView.editingWordLettersGO)
            {
                if (obj != closest)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    private GameObject FindClosestObject(Vector2 position, List<GameObject> gameObjects)
    {
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = new Vector3(position.x, position.y, 0);

        // Recorre todos los objetos para encontrar el más cercano.
        foreach (GameObject obj in gameObjects)
        {
            Vector3 directionToTarget = obj.transform.position - currentPosition;
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
