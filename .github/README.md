# Ecosystem Simulation
This project aims to simulate an ecosystem with different animal and plant species that evolve through natural selection. <br>
The simulation takes place on a procedurally generated island that I made thanks to Sebastian Lague's [procedural Landmass generation](https://github.com/SebLague/Procedural-Landmass-Generation).

The user can see what is going on by moving freely around the island, focusing on a specific animal to see its stats and what it is doing, or see the statistics of how the population and its genetics are evolving.

## Hot it works

Currently, there are only two animal species on the island: foxes and rabbits. <br>
In order to survive, the rabbits need to eat grass while the foxes will have to eat rabbits. They will also reproduce creating offspring with randomly mutated genes. If these genes are better for survival, the animal will have a higher chance to survive and reproduce, transmitting its genetics to the next generation. If the genes are bad for survival, the animal might die before being able to reproduce and its genetics will not be inherited by the next generation. This process is known as natural selection.

### Genetics
There are multiple genes that can mutate, including speed, vision range, the growth duration for offspring, the gestation duration for females or the urge to reproduce.

The way animals evolve depends on many different factors, including the random mutation of genes, the amount of animals in each species, and the variables that can be set before the simulation starts.

### Variables
An example of these variables is how much speed affects the hunger of the animal. If rabbits could have a greater speed without gaining too much hunger, they might evolve to be faster so they can avoid being eaten by foxes.
