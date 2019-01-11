using UnityEngine;

namespace UnityExtensions.Editor
{
    /// <summary>
    /// 对象池使用示例
    /// </summary>
    public class AutoGun : StateMachineComponent<State>
    {
        [SerializeField]
        Transform _muzzle;

        [SerializeField]
        Renderer _muzzleRenderer;

        [SerializeField]
        [Min(10)]
        float _bulletSpeed = 100;

        [SerializeField]
        [Min(0.5f)]
        float _bulletLifeTime = 4;

        [SerializeField]
        [Min(0)]
        float _shootInterval = 0.1f;

        [SerializeField]
        [Min(1)]
        int _maxContinuouslyShootCount = 5;

        [SerializeField]
        [Min(0.2f)]
        float _coolingDuration = 0.5f;


        State _shootingState;
        State _coolingState;

        MaterialPropertyBlock _muzzleProperty;


        void Awake()
        {
            InitShootingState();
            InitCoolingState();

            _muzzleProperty = new MaterialPropertyBlock();

            SetHot(0);
            currentState = _shootingState;
        }


        void FixedUpdate()
        {
            OnUpdate(Time.deltaTime);
        }


        void SetHot(float hot)
        {
            _muzzleProperty.SetColor(ShaderIDs.color, Color.Lerp(Color.white, Color.red, hot));
            _muzzleRenderer.SetPropertyBlock(_muzzleProperty);
        }


        void InitShootingState()
        {
            _shootingState = new State();

            float shootWaitTime = 0f;
            int continuouslyShootCount = 0;
            
            _shootingState.onUpdate += deltaTime =>
            {
                shootWaitTime -= deltaTime;
                if (shootWaitTime <= 0)
                {
                    Shoot();

                    if (continuouslyShootCount == _maxContinuouslyShootCount)
                    {
                        shootWaitTime = 0;
                        continuouslyShootCount = 0;
                        currentState = _coolingState;
                    }
                    else shootWaitTime += _shootInterval;
                }
            };

            void Shoot()
            {
                var bullet = GameObjectPool.lastAwaked.Spawn("Bullet");
                GameObjectPool.lastAwaked.Despawn(bullet, _bulletLifeTime);

                bullet.transform.position = _muzzle.position;
                bullet.transform.rotation = _muzzle.rotation;

                bullet.GetComponent<Rigidbody>().velocity = _muzzle.forward * _bulletSpeed;

                continuouslyShootCount++;
                SetHot((float)continuouslyShootCount / _maxContinuouslyShootCount);
            }
        }


        void InitCoolingState()
        {
            _coolingState = new State();

            _coolingState.onUpdate += deltaTime =>
            {
                float hot = Mathf.Clamp01(1f - currentStateTime / _coolingDuration);
                SetHot(hot);
                if (hot == 0f) currentState = _shootingState;
            };
        }
    }
}