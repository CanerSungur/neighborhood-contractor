using UnityEngine;

[RequireComponent(typeof(Neighbor))]
public class NeighborComplain : MonoBehaviour
{
    private Neighbor _neighbor;

    [SerializeField] private GameObject[] dialogs;
    private Animator _dialogAnimator;

    public void Init(Neighbor neighbor)
    {
        _neighbor = neighbor;

        for (int i = 0; i < dialogs.Length; i++)
            dialogs[i].SetActive(false);

        _neighbor.OnStartComplaining += ActivateRandomDialog;
        NeighborhoodEvents.OnBuildingRepaired += CloseDialog;
    }

    private void OnDisable()
    {
        _neighbor.OnStartComplaining -= ActivateRandomDialog;
        NeighborhoodEvents.OnBuildingRepaired -= CloseDialog;
    }

    private void ActivateRandomDialog()
    {
        int random = Random.Range(0, dialogs.Length);
        for (int i = 0; i < dialogs.Length; i++)
        {
            if (i == random)
            {
                dialogs[i].SetActive(true);
                _dialogAnimator = dialogs[i].transform.parent.parent.GetComponent<Animator>();
                _dialogAnimator.SetBool("Close", false);
            }
                
            else
                dialogs[i].SetActive(false);
        }
    }

    private void CloseDialog(Building building)
    {
        if (building == _neighbor.RelatedBuilding)
            _dialogAnimator.SetBool("Close", true);
    }
}
