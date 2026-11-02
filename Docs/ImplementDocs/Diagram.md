# 구현 다이어그램
> 구현마다 최신화하여 갱신한다. (최종 갱신: 2026-11-02, `261102FeatEnemy`)

```mermaid
flowchart TD
    subgraph InputGroup["입력"]
        Input["PlayerControls<br/>(InputActions)<br/>───<br/>Move / TouchPress / TouchPosition"]
        Joystick["FloatingJoystick<br/>───<br/>OnTouchStarted();<br/>OnTouchCanceled();<br/>Direction"]
    end

    subgraph PlayerGroup["플레이어"]
        Movement["PlayerMovement<br/>───<br/>Update();<br/>FixedUpdate();<br/>MoveDirection / IsMoving"]
        Body["Rigidbody<br/>───<br/>linearVelocity"]
        Animation["PlayerAnimation<br/>───<br/>Update();<br/>Animator.SetBool(IsMoving);<br/>SpriteRenderer.flipX"]
        Camera["CameraFollow<br/>───<br/>LateUpdate();"]
        Weapon["PlayerWeapon<br/>───<br/>Update();<br/>GetAimDirection();<br/>AimAt();<br/>AlignMuzzleToSprite();<br/>MuzzlePoint"]
        WeaponGraphic["WeaponGraphic<br/>───<br/>SpriteRenderer<br/>localScale.y 반전"]
    end

    subgraph CombatGroup["전투"]
        AutoAttack["PlayerAutoAttack<br/>───<br/>Update();<br/>FindNearestEnemy();<br/>Fire();<br/>CurrentTarget"]
        Bullet["ObjectPool / Bullet<br/>───<br/>Launch();<br/>FixedUpdate();<br/>OnTriggerEnter();<br/>ReleaseToPool();"]
    end

    subgraph EnemyGroup["적"]
        EnemyHealth["EnemyHealth<br/>───<br/>TakeDamage();<br/>Die();<br/>OnHealthChanged"]
        EnemyHealthBar["EnemyHealthBar<br/>───<br/>UpdateGauge();<br/>Image.fillAmount"]
    end

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
