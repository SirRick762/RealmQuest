using UnityEngine;

namespace Plataformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(JumpHash, crossFadeDuration);

        }
        public override void FixedUpdate()
        {
            //chamar a logica de salto do player
            player.HandleJump();
            player.HandleMovement();

        }
    }

}
