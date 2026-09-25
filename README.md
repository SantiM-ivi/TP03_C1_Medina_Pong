# Pong 2D (Unity)

Juego de Pong para 2 jugadores hecho en Unity (2D, físicas con Rigidbody2D), con paleta personalizable, sistema de puntaje y reinicio automático de la pelota.

## Características

- **2 jugadores locales**, cada uno con su paleta.
- **Altura de paleta ajustable** por slider (`PaddleAppearance`), escala solo en Y.
- **Color de paleta** elegible con un único slider que recorre una paleta de colores y lo aplica a los dos jugadores a la vez.
- **Velocidad de movimiento** de cada paleta configurable por slider.
- **Pelota con rebote físico real**: reflexión correcta contra paredes usando la velocidad previa al step de física (evita el bug de Unity donde `OnCollisionEnter2D` ya reporta la velocidad post-colisión).
- **Rampa de velocidad**: la pelota acelera en cada rebote (pared o paleta), hasta un máximo configurable.
- **Sistema anti-atasco**: si la pelota queda con velocidad casi nula (rebote de esquina, por ejemplo), se relanza sola en la última dirección válida.
- **Sistema de goles y puntaje**: detección por triggers en los costados de la cancha, actualización de UI y respawn de la pelota con delay.

## Estructura de scripts

| Script | Responsabilidad |
|---|---|
| `PaddleAppearance.cs` | Altura (escala en Y) y color de una paleta. |
| `SettingsPanel.cs` | Panel de configuración: sliders de velocidad, altura y color para ambos jugadores. |
| `BallMovement.cs` | Movimiento, rebote y rampa de velocidad de la pelota. |
| `GoalTrigger.cs` | Detecta cuándo la pelota entra a un arco (trigger) y avisa al `MatchManager`. |
| `MatchManager.cs` | Lleva el puntaje, actualiza la UI y relanza la pelota tras cada gol. |

## Configuración en el editor

### Pelota
- `Rigidbody2D`: Gravity Scale = 0 (se fuerza por código), Collision Detection = Continuous.
- `CircleCollider2D` sin `Is Trigger`.
- Tag: `Ball`.

### Paredes (arriba/abajo)
- `Collider2D` sólido (sin `Is Trigger`).
- Tag: `Wall`.

### Paletas
- `Collider2D` sólido.
- Tag: `Paddle`.

### Arcos (costados de la cancha)
- `Collider2D` con `Is Trigger` tildado.
- Componente `GoalTrigger`, con `Side` (Left/Right) según corresponda y referencia al `MatchManager`.

### MatchManager
- Referencias: `Ball` (BallMovement), `Left Score Text` / `Right Score Text` (TMP_Text), `Respawn Delay`.

### SettingsPanel
- Referencias de `Player1`/`Player2`: `Movement`, `Appearance`, sliders de velocidad y altura.
- `Color Slider` único, conectado al método `OnColorSliderChanged` en el evento `On Value Changed` del slider.

## Cómo jugar

Cada jugador mueve su paleta para devolver la pelota. Si la pelota pasa el arco propio, el otro jugador suma un punto y la pelota se relanza hacia el que recibió el gol tras una breve pausa.

## Notas técnicas

- El rebote contra pared usa `velocityBeforeStep` (la velocidad capturada al inicio del `FixedUpdate`, antes de que la física del step la modifique) en vez de `rb.linearVelocity` leída dentro de `OnCollisionEnter2D`, que ya viene alterada por la resolución interna de colisión de Unity.
- La velocidad de la pelota se resetea a su valor inicial en cada relanzamiento tras un gol (no es acumulativa entre puntos).
