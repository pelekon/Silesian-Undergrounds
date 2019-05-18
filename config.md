# Here is all needed information to create config.json file
`config.json` file should be placed in the `Data` directory, but it is not required, when `config.json` file does not exist default values are used. There is also no need of declaring whole config.json file with all fields, you can change as many as you want also only one field, for example:
```json
{
  "player": {
    "max-hunger": 200
  }
}
```
be aware that when your JSON file will contain errors which will make your file unable to be parsed, default values will be used (if debug, an error will be print to the console).

# Config information

## `player` - keeps all required player config
  - `hunger-decrese-interval-in-seconds` int, default: `10`
  - `hunger-decrease-value` int, default: `5`
  - `live-decrease-value-when-hunger-is-zero` int, default: `20`
  - `player-collider-box-width` int, default: `60`
  - `player-collider-box-height` int, default: `60`
  - `health` int, default: `100`
  - `max-health` int, default: `150`
  - `hunger` int, default: `100`
  - `max-hunger` int, default: `150`
  - `attack-speed` float, default: `1.0f`
  - `movement-speed` float, default: `2.0f`
  - `damage` int, default: `10`
  - `key-amount` int, default: `0`
  - `money-amount` int, default: `0`
  
## `terrain` - keeps terrain config
  - `percentage-of-textures-with-things` int, default: `25`
  
## `chest` - keeps chests config
  - `number-of-chest-texture` int, default: `4`
  - `number-of-possible-spawned-item` int, default: `6`
  - `minimum-number-of-spawned-item` int, default: `1`
  - `range-of-spawn` int, default: `1`
  
## `pickable` - keeps config of all pickables
- ### `heart`
  - `heart-increase-value` int, default: `10`
  - `live-regeneration-value` int, default: `25`
- ### `attack-booster`
  - `player-attack-increase-by` float, default: `1.0f`
- ### `hunger-booster`
  - `player-max-hunger-value-increase-by` int, default: `100`
- ### `live-booster`
  - `player-max-live-value-increase-by` int, default: `100`
- ### `movement-booster`
  - `player-movement-increase-by` int, default: `1.0f`


### Here is a full `config.json` file with all fields set to default values: 

```json
{
  "player": {
    "hunger-decrese-interval-in-seconds": 10,
    "hunger-decrease-value": 5,
    "live-decrease-value-when-hunger-is-zero": 20,
    "player-collider-box-width": 60,
    "player-collider-box-height": 60,
    "health": 100,
    "max-health": 150,
    "hunger": 100,
    "max-hunger": 150,
    "attack-speed": 1.0,
    "movement-speed": 2.0,
    "damage": 10,
    "key-amount": 0,
    "money-amount": 0
  },
  "terrain": {
    "percentage-of-textures-with-things": 25
  },
  "chest": {
    "number-of-chest-texture": 4,
    "number-of-possible-spawned-item": 6,
    "minimum-number-of-spawned-item": 1,
    "range-of-spawn": 1
  },
  "pickable": {
    "heart": {
      "heart-increase-value": 10,
      "live-regeneration-value": 25
    },
    "attack-booster": {
      "player-attack-increase-by": 1.0
    },
    "hunger-booster": {
      "player-max-hunger-value-increase-by": 100
    },
    "live-booster": {
      "player-max-live-value-increase-by": 100
    },
    "movement-booster": {
      "player-movement-increase-by": 1.0
    }
  }
}
```