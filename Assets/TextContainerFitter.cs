using TMPro;
using UnityEngine;

public class TextContainerFitter : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI m_TextMeshPro;

    private RectTransform m_RectTransform;
    private RectTransform m_TMPRectTransform;
    private float m_PreferredWidth;

    private void OnEnable()
    {
        SetWidth();
    }
        
    // Start is called before the first frame update
    void Start()
    {
        SetWidth();
    }
    // Update is called once per frame
    void Update()
    {
        if (PerreferedWidth != TextMeshPro.preferredWidth)
        {
            SetWidth();
        }

    }
    public TextMeshProUGUI TextMeshPro
    {
        get
        {
            if (m_TextMeshPro == null && transform.GetComponentInChildren<TextMeshProUGUI>())
            {
                m_TextMeshPro = transform.GetComponent<TextMeshProUGUI>();
                m_RectTransform = m_TextMeshPro.rectTransform;
            }
            return m_TextMeshPro;
        }
    }
    public RectTransform rectTransform
    {
        get
        {
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
            return m_RectTransform;
        }
    }
    public RectTransform TMPRectTransform
    {
        get
        {
            return m_TMPRectTransform;
        }
    }
      
    private float PerreferedWidth
    {
        get
        {
            return m_PreferredWidth;
        }
    }

    private void SetWidth() 
    {
        if (TextMeshPro == null)
            return;
        m_PreferredWidth = TextMeshPro.preferredWidth;

        rectTransform.sizeDelta = new Vector2(m_PreferredWidth, rectTransform.sizeDelta.y);
    }

}