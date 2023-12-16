# Ecosystem
The game can be downloaded on [my itch.io page](https://kheeto.itch.io/). It simulates an ecosystem with genetics, evolution through natural selection and procedural world generation.

## Implementation
You can implement my code into your project as you wish, read the guide below.

For the ecosystem itself, you only need the scripts in the Ecosystem folder, but you can download the other ones too, those include Procedural World Generation, UI and other game scripts.

### Prefabs
For each animal species you need to create two prefabs both with a NavMeshAgent, add the AnimalMale script to one of them, and AnimalFemale to the other one.
<br>

Remember that all prefabs must be stored in a folder named "Resources".

### Layers
My code makes use of layers to detect animals and food, you need to create a layer for each species (both plants and animals). For example I use three layers: Grass, Rabbit, Fox.

Then, make sure every prefab contains a collider with the proper layer assigned.

Now to the animal prefabs and configure the following layers in the animal script:
- whatIsFood: The layer of food that this animal will eat
- whatIsAnimal: The layer of this animal, must be the same for males and females
- whatIsPredator: The layer of the predator this animal will escape from

### Genes
Some of the settings you will read below are classified as genes, each gene contains five floats:
- value: The current value of this gene
- mutationAmount: The maximum amount this gene will mutate, becoming higher or lower
- mutationChance: The chance this gene will mutate during reproduction, between 0 and 1
- minAmount: The minimum value this gene can possibly have
- maxAmount: The maximum value this gene can possibly have

### Other settings
These are all the settings you can configure in the animal prefabs:
- speed: A gene that determines how fast the animal goes at the cost of increased hunger
- hunger: The current hunger between 0 and 10, no need to change this
- minimumHunger: The animal will not eat food below this hunger level, instead it will explore new places
- maxFleeHunger: Above this hunger level, the animal will try to eat food even if there is a predator
- hungerIncrease: How much this animal's hunger increases every second
- speedHungerIncrease: An extra increase in hunger that will be multiplied with the animal's speed
- spotRange: A gene determining how far the animal can spot food, females and predators
- eatRange: A fixed value determining how far the animal can eat food
- growthTime: A gene determining how fast the offspring grows
- growthRequirement: The sum of growthTime + gestationDuration is needed for a proper growth, animals below this have a chance of dying
- agingTime: The animal will always die after this amount of time

Male-only settings:
- reproductionUrge: A gene determining how much reproduction is valued compared to hunger. The animal will only focus on food when the hunger is greater than this value.

Female-only settings:
- gestationTime: A gene determining how much time the pregnancy lasts
- minBirthAmount: A fixed integer for the minimum amount of offspring
- maxBirthAmount: A fixed integer for the maximum amount of offspring
- femalePrefab: The name of the female prefab in the Resources folder
- malePrefab: The name of the male prefab in the Resources folder
