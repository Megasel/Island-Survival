using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerSystem : GameSystem
{
    [SerializeField] float curSpeed;
    bool touched;
    float normalSpeed;
    float gravity = 9.8f;
    bool canMove=true;
    public bool CanMove=>canMove;

    public override void OnInit()
    {
        game.Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponent>();
        game.Char = game.Player.GetComponent<CharacterController>();
        game.Anim = game.Player.GetComponentInChildren<Animator>();
        game.Player.UpdateVisual();
        game.Player.gameObject.transform.position = config.IslandSpawnPositions[player.IslandLevel];
        player.Speed = config.playerMoveSpeed;
        normalSpeed = player.Speed;
        curSpeed = normalSpeed;
        Signals.Get<SpeedBoostSignal>().AddListener(BoostSpeed);
        Signals.Get<DisableSpeedBoostSignal>().AddListener(DisableSpeedBoost);
        Signals.Get<PlayerCanMoveSignal>().AddListener(AllowMovement);
    }

    private void DisableSpeedBoost()
    {
        curSpeed = normalSpeed;
    }

    private void BoostSpeed()
    {
        curSpeed=normalSpeed*1.3f;//+30%
    }

    private void AllowMovement(bool isCanMove)
    {
        canMove = isCanMove;
    }

    public override void OnUpdate()
    {
        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
                touched = true;

            if (Input.GetMouseButtonUp(0))
            {
                touched = false;
                game.Anim.SetBool("Run", false);
            }

            if (touched)
            {
                var motion = new Vector3(game.Joystick.x, -gravity, game.Joystick.z);
                game.Char.Move(motion * curSpeed * Time.deltaTime);
                if (game.Joystick.magnitude > 0)
                {
                    game.Player.transform.forward = game.Joystick.normalized;
                    game.Anim.SetBool("Run", true);
                    Bootstrap.Instance.GameData.Anim.SetBool("Pick0", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Axe0", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Spear0", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Pick1", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Axe1", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Spear1", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Pick2", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Axe2", false);
                    Bootstrap.Instance.GameData.Anim.SetBool("Spear2", false);

                    game.Anim.transform.localPosition = -Vector3.up;
                    game.Anim.transform.localEulerAngles = Vector3.zero;
                }
                else game.Anim.SetBool("Run", false);
            }
        }
    }
}
