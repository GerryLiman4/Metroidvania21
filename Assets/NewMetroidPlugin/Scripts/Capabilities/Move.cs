using J98214;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J98214
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
        [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 100f;
        [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 100f;
        [SerializeField] private TrailRenderer tr;

        private Controller _controller;
        private Vector2 _direction, _desiredVelocity, _velocity;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;

        private float _maxSpeedChange, _acceleration;
        private bool _onGround;

        private bool canDash = true;
        private bool isDashing;
         private float dashingPower = 35f;
         private float dashingTime = 0.5f;
         private float dashingCooldown = 0.5f;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            _direction.x = _controller.input.RetrieveMoveInput(this.gameObject);
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _collisionDataRetriever.Friction, 0f);
            float moveSpeed = Mathf.Abs(_desiredVelocity.x);
            _controller.animator.SetFloat(AnimationVariableId.MoveSpeed.ToString(), moveSpeed);
            Flip();
            if (isDashing)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

        private void Flip()
        {
            if (_direction.x == 0) return;
            Vector3 originalScale = _controller.transform.localScale;

            if (_direction.x > 0)
            {
                originalScale.x = Mathf.Abs(originalScale.x);
            }
            else if (_direction.x < 0)
            {
                originalScale.x = Mathf.Abs(originalScale.x) * -1f;
            }

            _controller.transform.localScale = originalScale;
        }

        private void FixedUpdate()
        {
            _onGround = _collisionDataRetriever.OnGround;
            _velocity = _body.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _body.velocity = _velocity;

            _controller.animator.SetBool(AnimationVariableId.Fall.ToString(), _body.velocity.y > 0 ? false : true);
            _controller.animator.SetBool(AnimationVariableId.Grounded.ToString(), _onGround);
            if (_body.velocity.y <= 0) _controller.animator.SetBool(AnimationVariableId.Jump.ToString(), false);
            if (isDashing)
            {
                return;
            }
        }

        public Vector2 GetMoveDirection()
        {
            return _direction;
        }

        private IEnumerator Dash()
        {
            canDash = false;
            isDashing = true;
            float originalGravity = _body.gravityScale;
            _body.gravityScale = 0f;
            _body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            _body.gravityScale = originalGravity;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }
    }
}
