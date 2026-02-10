using System;
using UnityEngine;
using TMPro;

public class PowerupSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject PowerupPrefab;
    [SerializeField]
    GameObjectStore SelectedObject;

    [SerializeField]
    SelectedObjectMover Mover;

    [SerializeField] private GameObjectSet PowerupPool;

    [SerializeField] private UInt16 NumPowerUps;

    [SerializeField] private InputController IC;

    [SerializeField]
    private BoolStore CanUseAbilities;

    [SerializeField] private AudioSource squeakySelection;

    [SerializeField] private TextMeshProUGUI remainingToysDisplay;

    private void Start()
    {
        for (UInt16 i = 0 ; i < NumPowerUps; i++)
        {
            GameObject powerup = Instantiate(PowerupPrefab);
            PowerupPool.Add(powerup);
            powerup.SetActive(false);
        }

        remainingToysDisplay.text = PowerupPool.GetListSize().ToString();
    }

    public void Spawn()
    {
        if (!CanUseAbilities.GetValue()) return;

        GameObject powerup = PowerupPool.GetItemAtIndex(0);
        PowerupPool.Remove(powerup);
        powerup.SetActive(true);
        powerup.transform.position = IC.TouchWorldPos;
        SelectedObject.SetObjects(powerup);
           
        powerup.layer = 2;
            
        Mover.StartMovingObject();

        squeakySelection.Play();
        remainingToysDisplay.text = PowerupPool.GetListSize().ToString();
    }
}
