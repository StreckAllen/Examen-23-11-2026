# Dungeon de demostración

Abre `Assets/Scenes/Dungeon_V1.unity` y pulsa Play en Unity. Después pulsa el botón Play del Canvas.

Controles originales: W/S avanzar y retroceder, A/D girar, clic izquierdo disparar, V cambiar cámara y botón derecho para mirar.

| Zona | Enemigos | Collectibles |
| --- | --- | --- |
| Zone1 | 3 Goblins | 2 GoldCoin y 5 SilverCoin |
| Zone2 | 2 Golems | 2 GoldCoin y 5 SilverCoin |
| Zone3 | 2 Orcs | 2 GoldCoin y 5 SilverCoin |
| Zone4 | 4 Skeletons | 2 GoldCoin y 5 SilverCoin |
| Zone5 | 2 Goblins, 1 Golem, 1 Orc, 2 Skeletons | 4 GoldCoin, 3 SilverCoin y 1 Trophy |

Son 17 puntos de enemigos y 36 collectibles en total. Las cuatro rampas suben 2 metros cada una. No es necesario saltar para cambiar de zona.

## Jerarquía

```
Dungeon_V1
  Zone1
    Geometry
    EnemySpawnPoints
      GoblinSpawnPoint
      GoblinSpawnPoint_02
      GoblinSpawnPoint_03
    CollectiblesSpawnPoints
      GoldCoinSpawnPoint
      SilverCoinSpawnPoint
      ...
    CollectibleSpawnZone
    EnemySpawnZone
    RampToZone2
  Zone2
  Zone3
  Zone4
  Zone5
  Entrance
```

Cada `EnemySpawnZone` contiene los grupos originales del script, con sus puntos asignados, y referencia al `CollectibleSpawnZone` de su propia zona. Al atravesarlo se generan ambos grupos mediante las factories y pools existentes. `spawnOnTrigger` del collectible está desactivado porque el generador de enemigos llama a `Spawn()`.

El volumen de activación está separado de los puntos: el script original destruye su propio objeto al activarse. Por eso los puntos permanecen dentro de `EnemySpawnPoints` y `CollectiblesSpawnPoints`.

Las factories y los pools se conservaron con sus referencias y prefabs originales. No se cambiaron los scripts de movimiento, combate, estados, cámara, UI ni recogida. La eliminación por flecha y por contacto con el jugador conserva el comportamiento original. No se añadieron puertas que exijan matar enemigos para continuar.

`OriginalLayout_Reference` contiene el montaje anterior desactivado, para consulta. `SampleScene.unity` conserva el montaje original. Solo la escena nueva usa esta dungeon.

El NavMesh ya está calculado y guardado en `DungeonDemoNavMesh.asset`. Tras mover suelos o rampas, selecciona `Dungeon_V1` y pulsa Bake en NavMeshSurface.

Verificado en Play: recorrido con W por las cuatro rampas, activación de las cinco zonas, los 17 spawns, recogida de monedas, disparo del arco, sangre y reutilización del enemigo por el pool.

## Contador de collectibles

El Canvas muestra trofeos, oro y plata arriba a la derecha, también después de pulsar Play. `CollectiblesCounter` lee las propiedades de `GameManager`. El saldo respeta la conversión existente: tres monedas de plata generan una de oro.
