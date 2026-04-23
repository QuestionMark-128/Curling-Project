# Curling Project

Created by Aiden Le and Calvin Richards

### Notes
Create a copy of the curling stones and brooms that are more blue, to reflect team ownership

File structure:

CurlingStone.cs (contains input manager):
Takes in mouse data for Magnitude, Vector, and Curl - After throw is released, this data is maintained for continuing movement. Additionally, this object owns the main camera, which will assist in ease of play as the stone moves down the sheet
This might require some scripting - after a stone is done being thrown, the camera needs to migrate to the next stone
Data: 
{
variables:
  Magnitude,
  Direction,
  Curl,
  Team (boolean or something, just to determine for scoring)

methods:
  SetIceFriction()
  OnMove()
  
children:
  colliders,
  camera,
  ring and arrow for aiming process
}

CurlingBroom.cs (contains input manager):
Once a stone is thrown, the broom will follow the player's mouse position. When Left Mouse is pressed and held, the 
broom will become "active." When active the broom will activate a collider along the bottom rectangle, and it will 
utilize its "brushing" function. Brushing will alter the friction value of all stones on the field, meaning it will need to talk through the input manager. 

CurlingSheet.cs (the only actual method is the score calculator):
Mostly just a collider and some regions for deciding the score at the end of each round.

GameManager.cs
Will be responsible for handling the interaction between the broom and the curling stones
Data:
{
variables:
  Player 1 and Player 2
    (each has a score etc)
  
}
