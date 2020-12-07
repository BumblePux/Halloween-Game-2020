using UnityEngine;

namespace BumblePux.Halloween2020
{
    public class Weapon : MonoBehaviour
    {
        [Header("Settings")]
        public int Damage = 1;

        [Header("Audio")]
        public AudioClip SwingClip;

        private new AudioManager audio;
        private Camera cam;
        private Animator anim;

        private int animAttackHash = Animator.StringToHash("Attack");

        //--------------------------------------------------------------------------------
        private void Start()
        {
            audio = AudioManager.Instance;
            cam = Camera.main;
            anim = GetComponent<Animator>();
        }

        //--------------------------------------------------
        private void Update()
        {
            AimAtMouse();

            if (Input.GetMouseButtonDown(0))
            {
                SwingShovel();
            }
        }

        //--------------------------------------------------
        private void AimAtMouse()
        {
            Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            transform.rotation = rotation;
        }

        //--------------------------------------------------
        private void SwingShovel()
        {
            anim.SetTrigger(animAttackHash);
            audio.PlaySfx(SwingClip);
        }

        //--------------------------------------------------
        private void OnTriggerEnter2D(Collider2D collision)
        {            
            EnemyController enemy = collision.GetComponentInParent<EnemyController>();
            if (enemy)
            {
                enemy.TakeDamage(Damage);
            }
        }
    }
}