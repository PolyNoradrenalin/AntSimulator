# Ant Colony Engine :

## Components/Actors :

-  ## Ant : 
  
Movement : 360° movement, max speed, unable to jump. Must replicate the movement of a real ant (unable to rotate on a whim, must turn around) 
Unable to pass through walls and food.

Behaviour : Dependant on pheromones, must be able to search for food and bring it back home.

An ant has an inventory slot to store another carriable entity (eg : food)

The ant's strategy for searching and bringing back food could differ depending on variations.

## Search : 

Explore in a semi-random manner (must follow a logical trajectory). Will try to move away from the colony if no food is next to it. 

Leaves behind "home" pheromones.

When it finds food, picks up a food entity, it transitions to "carry home".

## Carry Home : 

Releases food pheromones periodically and uses "home" pheromones to head home. Gives food to colony once it arrives home and transitions back to "search".

## Death and abandonment

Has a lifespan. 

(Could have an energy level that decreases and need to go back to the colony to replenish itself)

- ## Pheromones : Are emitted and read by ants in order to modify their behaviour

Basic entity, without any movement. 
Has a lifespan, and decays until it disappears.

Can be of different types (eg: food, home, ...)

- ## Colony : Spawns ants & manages food

Is unpassable.
Creates initial ants.
Takes food and outputs ants.

- ## Food : Is picked up by ants and brought back to colony

Entity that possesses a capacity (units of nourishment)

The ant's objective is to get food.

- ## Walls : 

Unpassable terrain. Ants are unable to sense pheromones through walls.