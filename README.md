

# Pong 2D (Unity)

Juego de Pong para 2 jugadores locales hecho en Unity (2D, físicas con `Rigidbody2D`). Las paletas se mueven en toda su mitad de la cancha, la partida se juega al mejor de 5 y hay un límite de tiempo para convertir cada gol.

> **Jugalo en Itch.io:**https://zdra.itch.io/plong

## Características

- **2 jugadores locales**, cada uno con su paleta.
- **Movimiento en 2 ejes con físicas**: las paletas se mueven con `AddForce` en vertical y horizontal, limitadas a su mitad de la cancha (desde el arco hasta el medio).
- **Partida al mejor de 5**: gana quien llegue primero a 3 puntos (configurable).
- **Límite de tiempo por gol**: si pasan 20 segundos (configurable) sin gol, se le hace un gol al jugador que tiene la pelota de su lado.
- **Colores dinámicos de las paletas**:
  - Al golpear la pelota, la paleta cambia a un color aleatorio.
  - Al tocar un límite de la pantalla (pared superior/inferior o el borde de su arco), la paleta se pone negra mientras dure el contacto.
- **Pelota con rebote físico real**: reflexión correcta contra paredes usando la velocidad previa al step de física.
- **Rampa de velocidad**: la pelota acelera en cada rebote (pared o paleta), hasta un máximo configurable.
- **Sistema anti-atasco**: si la pelota queda con velocidad casi nula, se relanza sola en la última dirección válida.
- **Sistema de goles y puntaje**: detección por triggers en los costados, UI de puntaje y timer, y respawn de la pelota con delay.
- **Pantalla de fin de partida** con el ganador, y opciones para reiniciar o volver al menú.
- **Menú principal, pausa, créditos y panel de configuración** (velocidad y altura de las paletas).

## Controles

| Jugador | Arriba | Abajo | Izquierda | Derecha |
|---|---|---|---|---|
| Izquierdo | `W` | `S` | `A` | `D` |
| Derecho | `↑` | `↓` | `←` | `→` |

Las teclas son configurables desde el Inspector de cada paleta (`PaddleMovement`).

## Reglas

1. Cada jugador defiende el arco de su lado. Si la pelota entra a tu arco, el rival suma un punto.
2. Cada gol tiene un límite de tiempo (por defecto 20 s). Si se agota, el punto es para el jugador que **no** tiene la pelota de su lado.
3. Gana quien llegue primero a la cantidad de puntos configurada (por defecto 3, es decir, mejor de 5).
4. Después de cada gol, la pelota se relanza hacia quien recibió el gol tras una breve pausa, con su velocidad inicial.

## Scriptable Objects

Los valores de inicialización viven en assets que se crean desde `Create > Pong` y se asignan en el Inspector:

| Asset | Contenido |
|---|---|
| `GameSettings` | Puntos para ganar (`PointsToWin`), tiempo para convertir un gol (`TimeToScore`) y delay de respawn (`RespawnDelay`). |
| `BallSettings` | Velocidad inicial, aumento por golpe, velocidad máxima, ángulo de rebote y componente vertical mínima. |
| `PaddleSettings` | Fuerza de movimiento, velocidad máxima y color de límite (negro por defecto). |

## Estructura de scripts

| Script | Responsabilidad |
|---|---|
| `PaddleMovement.cs` | Movimiento de la paleta con `AddForce` en X e Y, límites de la cancha y detección de contacto con los límites de pantalla. |
| `PaddleAppearance.cs` | Altura (escala en Y) y color de la paleta: color base, color aleatorio y color de límite (negro). |
| `SettingsPanel.cs` | Panel de configuración: sliders de velocidad, altura y color para ambos jugadores. |
| `BallMovement.cs` | Movimiento, rebote y rampa de velocidad de la pelota. Cambia el color de la paleta al golpearla. |
| `GoalTrigger.cs` | Detecta cuándo la pelota entra a un arco (trigger) y avisa al `MatchManager`. |
| `MatchManager.cs` | Puntaje, timer de gol, condición de victoria, fin de partida y relanzamiento de la pelota. |
| `GameSettings.cs`, `BallSettings.cs`, `PaddleSettings.cs` | `ScriptableObject`s con los valores de configuración. |
| `MainMenuManager.cs`, `PauseMenuManager.cs` | Menú principal y menú de pausa. |

## Configuración en el editor

### Pelota
- `Rigidbody2D`: Gravity Scale = 0, Collision Detection = Continuous.
- `CircleCollider2D` sin `Is Trigger`.
- Tag: `Ball`.
- Componente `BallMovement` con el asset `BallSettings` asignado.

### Paredes (arriba/abajo)
- `Collider2D` sólido (sin `Is Trigger`).
- Tag: `Wall`.

### Paletas
- `Rigidbody2D` (el script fuerza Gravity Scale = 0 y solo congela la rotación) y `Collider2D` sólido.
- Tag: `Paddle`.
- `PaddleMovement`: asset `PaddleSettings`, teclas de movimiento, `Court Side` (Left/Right) y límites `Min X` / `Max X` (del arco al medio) y `Min Y` / `Max Y`.
- `PaddleAppearance`: asset `PaddleSettings`.

### Arcos (costados de la cancha)
- `Collider2D` con `Is Trigger` tildado.
- Componente `GoalTrigger`, con `Side` (Left/Right) según corresponda y referencia al `MatchManager`.

### MatchManager
- Referencias: `Settings` (`GameSettings`), `Ball`, `Left Score Text` / `Right Score Text` / `Timer Text` (TMP_Text).
- Fin de partida: `End Match Panel`, `Winner Text` y `Main Menu Scene Name`.
- Los botones del panel de fin de partida se conectan a `RestartMatch()` y `GoToMainMenu()`.

### SettingsPanel
- Referencias de `Player1`/`Player2`: `Movement`, `Appearance`, sliders de velocidad y altura.
- `Color Slider` único, conectado a `OnColorSliderChanged`. Aplica un color base a ambas paletas (que luego cambia durante la partida).

## Notas técnicas

- El rebote contra pared usa `velocityBeforeStep` (la velocidad capturada al inicio del `FixedUpdate`, antes de que la física del step la modifique) en vez de `rb.linearVelocity` leída dentro de `OnCollisionEnter2D`, que ya viene alterada por la resolución interna de colisión de Unity.
- La velocidad de la pelota se resetea a su valor inicial en cada relanzamiento tras un gol (no es acumulativa entre puntos).
- El contacto de la paleta con las paredes se detecta con `OnCollisionStay2D` (bandera por paso de física), y el contacto con los bordes de su zona por posición; ambos ponen la paleta en negro y al separarse recupera su color.
- El timer de gol se reinicia en cada punto y se pausa durante el respawn y al terminar la partida.
- Los `ScriptableObject` no se modifican en runtime: los sliders del panel de configuración actúan sobre copias en memoria.
