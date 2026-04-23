# Curling Project

Created by Aiden Le and Calvin Richards

File structure:
CurlingStone.cs (contains input manager):
Takes in mouse data for Magnitude, Vector, and Curl - After throw is released, this data is maintained for continuing movement.

CurlingBroom.cs (contains input manager):
Once a stone is thrown, the broom will follow the player's mouse position. When Left Mouse is pressed and held, the 
broom will become "active." When active the broom will activate a collider along the bottom rectangle, and it will 
utilize its "brushing" function. Brushing will alter the friction value of all stones on the field, meaning it will need to talk through the input manager. 

CurlingSheet.cs (the only actual method is the score calculator)
Mostly just a collider and some 
