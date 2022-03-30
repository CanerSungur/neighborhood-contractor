using UnityEngine;

// manage a single building
public class Building : MonoBehaviour
{
    [Header("-- REFERENCES --")]
    private Build _build;
    private TextHandler _textHandler;

    #region Properties

    public TextHandler TextHandler => _textHandler;

    #endregion

    private void Init() 
    {
        TryGetComponent(out _build);
        _build.Init(this);
        TryGetComponent(out _textHandler);

    }

    private void Awake()
    {
        Init();
    }
}
