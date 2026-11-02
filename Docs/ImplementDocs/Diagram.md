# 구현 다이어그램
> 구현마다 최신화하여 갱신한다. (최종 갱신: 2026-11-02, `261102FeatEnemy`)

```mermaid
flowchart LR
    Input["PlayerControls<br/>(InputActions)<br/>───<br/>Move / TouchPress / TouchPosition"]
    Joystick["FloatingJoystick<br/>───<br/>OnTouchStarted();<br/>OnTouchCanceled();<br/>Direction"]
    Movement["PlayerMovement<br/>───<br/>Update();<br/>FixedUpdate();<br/>MoveDirection / IsMoving"]
    Animation["PlayerAnimation<br/>───<br/>Update();<br/>Animator.SetBool(IsMoving);<br/>SpriteRenderer.flipX"]
    Body["Rigidbody<br/>───<br/>linearVelocity"]
    Camera["CameraFollow<br/>───<br/>LateUpdate();"]
    Weapon["PlayerWeapon<br/>───<br/>Update();<br/>GetAimDirection();<br/>AimAt();<br/>AlignMuzzleToSprite();<br/>MuzzlePoint"]
    WeaponGraphic["WeaponGraphic<br/>───<br/>SpriteRenderer<br/>localScale.y 반전"]
    AutoAttack["PlayerAutoAttack<br/>───<br/>Update();<br/>FindNearestEnemy();<br/>Fire();<br/>CurrentTarget"]
    Bullet["ObjectPool / Bullet<br/>───<br/>Launch();<br/>FixedUpdate();<br/>OnTriggerEnter();<br/>ReleaseToPool();"]
    EnemyHealth["EnemyHealth<br/>───<br/>TakeDamage();<br/>Die();<br/>OnHealthChanged"]
    EnemyHealthBar["EnemyHealthBar<br/>───<br/>UpdateGauge();<br/>Image.fillAmount"]

    Input -->|TouchPress, TouchPosition| Joystick
    Input -->|Move| Movement
    Joystick -->|Direction| Movement
    Movement -->|속도 설정| Body
    Movement -->|MoveDirection, IsMoving| Animation
    Body -.->|Player Transform| Camera
    Movement -->|MoveDirection| Weapon
    Weapon -->|회전, 반전, 총구 정렬| WeaponGraphic
    AutoAttack -->|CurrentTarget| Weapon
    Weapon -->|MuzzlePoint| AutoAttack
    AutoAttack -->|Get / Launch| Bullet
    Bullet -->|TakeDamage| EnemyHealth
    EnemyHealth -->|OnHealthChanged| EnemyHealthBar
```
