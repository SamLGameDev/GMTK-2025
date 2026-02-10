using UnityEngine;

public interface ISelectable
{
    public void OnDrop();

    public void OnInvalidLastPos();

    public bool CanMoveInsideWalls();

    public Color GetInvalidColor();
}
