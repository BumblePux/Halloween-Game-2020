using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BumblePux.Halloween2020
{
    public class EnemyController : MonoBehaviour
    {
        [Header("References")]
        public Slider HealthBar;

        [Header("Audio")]
        public AudioClip HitClip;

        [Header("Movement Settings")]
        public float Speed = 20f;
        public float StoppingDistance = 2f;

        [Header("Health Settings")]
        public int MaxHealth = 3;
        public float PushBackAmount = 100f;

        private WaveGameMode gameMode;
        private PlayerController player;
        private new AudioManager audio;

        private Rigidbody2D rb2d;
        private SpriteRenderer[] sprites;

        private int currentHealth;
        private bool canMove;
        private bool takenDamage;

        //--------------------------------------------------------------------------------
        private void Start()
        {
            gameMode = FindObjectOfType<WaveGameMode>();
            player = FindObjectOfType<PlayerController>();
            audio = AudioManager.Instance;

            rb2d = GetComponent<Rigidbody2D>();
            sprites = GetComponentsInChildren<SpriteRenderer>();

            gameMode.RegisterEnemy(this);
            currentHealth = MaxHealth;
            canMove = true;

            HealthBar.maxValue = MaxHealth;
            HealthBar.wholeNumbers = true;
            UpdateHealthBar();

            if (!player)
                Die();
        }

        //--------------------------------------------------
        private void FixedUpdate()
        {
            if (!player)
            {
                rb2d.velocity = Vector2.zero;
                return;
            }

            if (canMove)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized; // * Time.fixedDeltaTime;

                if (Vector2.Distance(player.transform.position, transform.position) <= StoppingDistance)
                {
                    return;
                }

                //rb2d.MovePosition(rb2d.position + direction * Speed);
                rb2d.velocity = direction * Speed;
            }
            else
            {
                rb2d.velocity = Vector2.zero;
            }
        }

        //--------------------------------------------------
        public void TakeDamage(int amount)
        {
            currentHealth--;

            if (HitClip)
                audio.PlaySfx(HitClip);

            UpdateHealthBar();
            StartCoroutine(ChangeColor());
            StartCoroutine(WaitToMove());
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        //--------------------------------------------------
        private void UpdateHealthBar()
        {
            HealthBar.value = currentHealth;
        }

        //--------------------------------------------------
        private IEnumerator ChangeColor()
        {
            foreach (var sprite in sprites)
            {
                sprite.color = Color.black;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var sprite in sprites)
            {
                sprite.color = Color.white;
            }
        }

        //--------------------------------------------------
        private IEnumerator WaitToMove()
        {
            canMove = false;
            yield return new WaitForSeconds(0.2f);
            canMove = true;
        }

        //--------------------------------------------------
        private void OnCollisionEnter2D(Collision2D other)
        {
            PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
            if (player)
            {
                player.TakeDamage();
            }
        }

        //--------------------------------------------------
        private void Die()
        {
            gameMode.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
}