using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BumblePux.Halloween2020
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float Speed = 5f;

        [Header("Health Settings")]
        public int MaxHealth = 5;

        [Header("Audio")]
        public AudioClip HitClip;

        private Rigidbody2D rb2d;
        private Animator anim;
        private new AudioManager audio;

        private Slider healthBar;
        private SpriteRenderer[] sprites;

        private Vector2 moveInput;
        private int animIsMovingHash = Animator.StringToHash("IsMoving");
        
        private int currentHealth;
        private bool isInvincible;

        //--------------------------------------------------------------------------------
        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            audio = AudioManager.Instance;

            healthBar = GetComponentInChildren<Slider>();
            sprites = GetComponentsInChildren<SpriteRenderer>();

            currentHealth = MaxHealth;

            healthBar.maxValue = MaxHealth;
            healthBar.wholeNumbers = true;
            UpdateHealthBar();
        }

        //--------------------------------------------------
        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(horizontal, vertical);
            moveInput = moveInput.normalized * Speed;

            anim.SetBool(animIsMovingHash, moveInput != Vector2.zero);
        }

        //--------------------------------------------------
        private void FixedUpdate()
        {
            //rb2d.MovePosition(rb2d.position + moveInput);
            rb2d.velocity = moveInput;
        }

        //--------------------------------------------------
        public void TakeDamage()
        {
            if (isInvincible) return;

            if (HitClip)
                audio.PlaySfx(HitClip);

            StartCoroutine(Invincible());
            StartCoroutine(ChangeColor());
            currentHealth--;
            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        //--------------------------------------------------
        private void UpdateHealthBar()
        {
            healthBar.value = currentHealth;
        }

        //--------------------------------------------------
        private IEnumerator Invincible()
        {
            isInvincible = true;
            yield return new WaitForSeconds(1f);
            isInvincible = false;
        }

        //--------------------------------------------------
        private IEnumerator ChangeColor()
        {
            foreach (var sprite in sprites)
            {
                sprite.color = Color.black;
            }

            yield return new WaitForSeconds(0.2f);

            foreach (var sprite in sprites)
            {
                sprite.color = Color.white;
            }
        }

        //--------------------------------------------------
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}