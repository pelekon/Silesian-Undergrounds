require('pixi');
require('p2');
require('phaser');

import { Sprite } from 'phaser';
document.body.style.margin = '0px'

class Game {
  private game: Phaser.Game
  private box: Sprite;
  private pickUps: Array<Sprite> = []
  private character: Sprite;
  private BOX_SIZE = 20
  private SQUARE_SIZE = 40
  private SPEED = 4
  private counter = 0;

  private colisionInfo = document.createElement('h1')
  constructor(){
    this.game = new Phaser.Game(window.innerWidth, window.innerHeight, Phaser.CANVAS, 'game', { preload: this.preload, create: this.create, update: this.update, render: this.render });
    this.colisionInfo.innerHTML = `${this.counter}`
    this.colisionInfo.style.position = 'absolute'
    this.colisionInfo.style.color = 'black'
    this.colisionInfo.style.paddingLeft = '20px'
    document.body.append(this.colisionInfo)
  }

  preload = () => {
    this.game.load.image('square', require('../assets/sprites/character.png'));
    this.game.load.image('circle', require('../assets/sprites/circle.png'));
    this.game.load.image('coal', require('../assets/sprites/coal.png'));
  }

  createPickUps = () => {
    this.pickUps = []
    for(let i = 0; i < 10; i ++){
      const pickUp = this.game.add.sprite(Math.floor(Math.random() * window.innerWidth) + 1,Math.floor(Math.random() * window.innerHeight) + 1, 'coal');
      pickUp.width = this.BOX_SIZE
      pickUp.height = this.BOX_SIZE
      pickUp.name = 'coal';
      this.pickUps.push(pickUp)
    }
  }

  checkCollisions = () => {
    this.pickUps.forEach((pickUp) => {
      if(!pickUp.visible) return;
      if(this.isColison(pickUp,this.character)) {
        this.colisionInfo.innerHTML = `${++this.counter}`
        if(this.counter % 10 === 0){
          this.createPickUps()
        }
        pickUp.destroy()
      } 
    })
  }

  create = () => {
    this.game.physics.startSystem(Phaser.Physics.ARCADE);

    this.game.stage.backgroundColor = '#fff';
    // this.box = this.game.add.sprite((innerWidth / 2) - this.BOX_SIZE / 2 - 100, (innerHeight / 2) -this.BOX_SIZE / 2, 'coal');
    // this.box.width = this.BOX_SIZE
    // this.box.height = this.BOX_SIZE
    // this.box.name = 'coal';

    this.createPickUps()

    this.character = this.game.add.sprite(0, 0, 'square', 2);
    this.character.width = this.SQUARE_SIZE
    this.character.height = this.SQUARE_SIZE
    this.character.name = 'square';
  }
  update = () => {
    if (this.game.input.keyboard.isDown(Phaser.Keyboard.LEFT) || this.game.input.keyboard.isDown(Phaser.Keyboard.A))
    {
        this.character.x -= this.SPEED;
    }
    else if (this.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT) || this.game.input.keyboard.isDown(Phaser.Keyboard.D))
    {
        this.character.x += this.SPEED;
    }
  
    if (this.game.input.keyboard.isDown(Phaser.Keyboard.UP) || this.game.input.keyboard.isDown(Phaser.Keyboard.W))
    {
        this.character.y -= this.SPEED;
    }
    else if (this.game.input.keyboard.isDown(Phaser.Keyboard.DOWN) || this.game.input.keyboard.isDown(Phaser.Keyboard.S))
    {
        this.character.y += this.SPEED;
    }
    this.checkCollisions()
      // if(this.isColison(this.box,this.character)) {
      //   this.colisionInfo.innerText = "COLLISION"
      // } else {
      //   this.colisionInfo.innerText = "NO COLLISION"
      // }
  } 
  render = () => {
    // this.game.debug.bodyInfo(this.box, 16, 24);
  }

  isColison = (sprite1: Sprite, sprite2: Sprite) => {
    const { x: sprite1X, y: sprite1Y, height: sprite1Height, width: sprite1Width  } = sprite1
    const { x: sprite2X, y: sprite2Y, height: sprite2Height, width: sprite2Width } = sprite2
    return (sprite2X + sprite2Width > sprite1X && sprite1X + sprite1Width > sprite2X) &&
    (sprite2Y + sprite2Height > sprite1Y && sprite1Y + sprite1Height > sprite2Y)
  }
}
new Game()