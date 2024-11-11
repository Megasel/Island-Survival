using Kuhpik;
using UnityEngine;

public class PointerSystem : GameSystemWithScreen<PointerScreen>
{
    [SerializeField] private Transform[] camps;
    [SerializeField] private float distanceToDisablePointer = 10;

    private Transform playerTransform;
    public PointerScreen Screen => screen;
    public Transform[] Camps=> camps;
    public override void OnInit()
    {
        screen.Open();
        if (camps[player.IslandLevel]!=null)
        {
            screen.SetCamp(camps[player.IslandLevel]);
        }
        playerTransform = game.Player.transform;
    }
    private void FixedUpdate()
    {
        //if (!player.TutorialDone) return;
        if (camps[player.IslandLevel]!=null)
        {
            var targetPos = camps[player.IslandLevel].position;
            targetPos.y = playerTransform.position.y;
            if ((playerTransform.position - targetPos).sqrMagnitude <
                distanceToDisablePointer * distanceToDisablePointer)
            {
                screen.HidePointer();
            }
            else
            {
                screen.ShowPointer();
            }
        }
        else
        {
            screen.HidePointer();
        }
    }
}
