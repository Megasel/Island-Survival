using DG.Tweening;
using Kuhpik;
using Supyrb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSystem : GameSystem
{
    [SerializeField] ParticleSystem boatGoneVFX;
    bool atDestinationPoint;
    public override void OnUpdate()
    {
        if (game.currentBoat.gameObject.transform.position!= game.currentBoat.Destination.position)
        {
            atDestinationPoint = false;
            Transform boatTransform = game.currentBoat.gameObject.transform.parent;
            Vector3 targetDirection = game.currentBoat.Destination.position - boatTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            boatTransform.rotation = Quaternion.RotateTowards(boatTransform.rotation, targetRotation, game.currentBoat.Speed*2 * Time.deltaTime);
            //
            game.currentBoat.gameObject.transform.parent.position=Vector3.MoveTowards(game.currentBoat.gameObject.transform.parent.position, game.currentBoat.Destination.position, game.currentBoat.Speed*Time.deltaTime);
        }
        else if(!atDestinationPoint)
        {
            atDestinationPoint = true;
            var pos = game.currentBoat.gameObject.transform.parent.position;
            Bootstrap.Instance.GameData.Player.transform.DORotate(game.currentBoat.playerOffBoat.eulerAngles, .5f);
            Bootstrap.Instance.GameData.Anim.SetTrigger("Jump");
            Bootstrap.Instance.GameData.Player.transform.DOJump(game.currentBoat.playerOffBoat.position, 1, 1, 1f).OnComplete(() =>
            {
                IslandsManager.Instance.DeactivateIsland(player.IslandLevel);
                player.IslandLevel++;
                FindObjectOfType<PointerSystem>().Screen.SetCamp(FindObjectOfType<PointerSystem>().Camps[player.IslandLevel]);
                game.Player.transform.SetParent(null);
                if (game.currentBoat.IsNewEra)
                {
                    player.VisualLevel++;
                    game.Player.UpdateVisual(true);
                }
                else
                {
                    Signals.Get<PlayerCanMoveSignal>().Dispatch(true);
                }
                Bootstrap.Instance.ChangeGameState(GameStateID.Game);
                Bootstrap.Instance.SaveGame();
                game.currentBoat.gameObject.transform.parent.DORotate(new Vector3(0, 1080, 0), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.InCubic);
                game.currentBoat.gameObject.transform.parent.DOScale(0, 1f).OnComplete(() => { boatGoneVFX.transform.position = game.currentBoat.transform.position; boatGoneVFX.Play(); game.currentBoat.gameObject.SetActive(false); game.currentBoat = null; });
            });
        }
    }
}
