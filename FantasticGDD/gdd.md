</br>
<p align="center">
<b><span style="font-size:3em;">Game Design Document</span>
<p align="center">
<b><span style="font-size:1.5em;">Fantastic Beasts and Where to Find Them</span>
<p align="center">
<p align="center"></br>
<b><span style="font-size:2em;">Desenvolvimento de Jogos Digitais - I</span></p>
</br>

Luiz Santos - 21901441</p>
Pedro Marques - 21900253</p>
Nelson Milheiro - 21904365</p>

# Version Control

>GDD Version: 1.1

- v1.0 - 27/03/2020
  - Initial GDD
- v1.1 - 27/05/2020
  - Added new images; changed mechanics

# Introduction

This Game Design Document is a guiding material for the development team and a
compilation of all the important aspects of the game.

- [Version Control](#version-control)
- [Introduction](#introduction)
- [Specifications](#specifications)
  - [Movie](#movie)
  - [Synopsis](#synopsis)
  - [Theme](#theme)
  - [Idea](#idea)
    - [Story](#story)
    - [Game Concept](#game-concept)
  - [Setting](#setting)
  - [Characters](#characters)
    - [Player](#player)
    - [Enemies](#enemies)
  - [Gameloops](#gameloops)
    - [Levels Gameloop](#levels-gameloop)
    - [General Gameloop (core)](#general-gameloop-core)
  - [Gameplay](#gameplay)
    - [Control](#control)
    - [Actions](#actions)
    - [Indicators](#indicators)
    - [Scale/Tiles](#scaletiles)
    - [Camera Trap](#camera-trap)
- [Target Audience](#target-audience)

# Specifications

## Movie

Fantastic Beasts and Where to Find Them

## Synopsis

“In mid-20s New York, Newt Scamander, the British young activist wizard, arrives
in town, holding a mysterious leather suitcase which shelters a wide array of
diverse and magical creatures that exist among us. (...) Newt's precious
suitcase will be lost--and to make matters worse--several creatures will manage
to escape.”, Nick Riganas - IMDB

## Theme

Fantastic Beasts has two main themes and a less explored third one:

- The protection of the helpless:
  
  - Illustrated by New's efforts to keep the Beasts and the character Creedence
  away from those who look at them as threats and are willing to even hurt
  them, if necessary, to get rid of that "threat";
  - Once those subjects have zero chances of protecting themselves against a
  whole wizarding community, Newt is the one who tries to protect them;
  
- The education of the whole wizarding community about them:
  - There's not a lot of people in the wizarding community who truly understand
  what the Beasts are. Mostly, they are seen as dangerous things and a possible
  cause to the wizarding world exposure;
  - Newt has devoted his career to the study, categorization and sharing of
  reliable information about this creatures;
- Fantastic Beasts traffic:
  - Not really explored on the movie, even though, there is a reference to
  Beasts traffic, when Newt talks about Frank, a Thunderbird that was made
  captive in Egypt;
  - The game will explore a little bit more of this grey area of the wizard
  world.

## Idea

### Story

By the end of the movie, using the assistance of Frank, the Thunderbird, Newt
convinces the president of MACUSA (Magical Congress of the United States of
America) that magical creatures aren't all about danger. From that moment on,
the american wizarding community starts to, gradually, change their ideas and
concepts about magic creatures, starting by some changes in the laws regarding
them.

This new concepts of protection of the beasts caught the wizarding underworld's
attention and some creatures, with unique features, started being victims of
imprisonment and traffic. MACUSA, however, was still so focused on avoiding
the exposition of the wizarding world, that they didn't give much attention
about what was happening in the underworld.

And with all that happening, Newt Scamander happens to come back to America to
deliver a copy of his book, in person, to Tina Goldstein, as he told her when
they last seen each other.

Newt, of course, notices some weird things happening and finds out about the
whole beat traffic situation and starts a new adventure focused on dismantling
criminal scheme.

### Game Concept

Considering the whole story, the game will be based on solving puzzles,
overcoming obstacles and fighting different kinds of enemies, such as goblins,
giants, trolls, and ogres.

All of this has, as main goal, saving the creatures that have been captured.
Throughout the gameplay, the player will have to use Newt's swooping evil to get
to new positions and find collectibles, that will reveal some information
about the beasts and the universe around them.

## Setting

The game story starts in New York, in the mid 20's so, some of the scenarios
will be inspired on NY visual.
Even though, as the game progresses, the player will get in touch with
different scenarios, such as, forests, lakes, etc.

![nelson_mb](cenarios.png)

## Characters

### Player

>Newton Artemis Fido “Newt” Scamander

Famous magizoologist, who developed his interest in magical creatures at an
early age, encouraged by his mother.

Given his passion for magical creatures, Newt devoted his time and career to
gathering as much information as he could about these beings and educating his
fellow wizards about them.
Newt is known for being an eccentric and awkward man, who feels way more
comfortable around animals then people.
When it comes to following the system's rules, Newt is not one of its great
followers. As long as no one gets hurt, he will do whatever it takes to assure
that his kind and, genuinely, good ideals are achieved.

### Enemies

>The wizarding underworld

It's a community that operates in the shadows, mostly, engaged in criminal
activities.

In the game, this community is composed by sapient magical creatures, like
Giants, Globins, Trolls and Ogres.

## Gameloops

### Levels Gameloop

Collectables -> Obstacles -> Enemies -> Saving Creature

### General Gameloop (core)

Level beginning -> Level Progression -> Next Level

## Gameplay

### Control

Keyboard.

### Actions

- Running;
- Jumping;
- Attacks:
  - Melee attack - fast, low damage, NO mana cost;
  - Magic spell - fast, medium damage, low mana cost;
- Defense:
  - Shield - high mana cost;
- Actions with beasts:
  - Using the Swooping Evil as a rope to pass through some obstacles:
![swooping evil rope](swoopingRope.png)
  
  - Using the Swooping Evil as a platform to pass through some obstacles:
![swooping evil platform](swoopingPlatform.png)

### Indicators

In this topic will be grouped some of the important interface features for the
game.

- Health Bar and mana Bar
  - Indicator meant to be at the top left side of  the screen.
  The mana bar will have a wand silhouette and the health bar will be attached at
  the top of it.

### Scale/Tiles

Character scale:
![player size](playerSize.png)

### Camera Trap

Considering that the player will have to do plenty of jumping, we use a camera
trap to avoid too much camera movement, while keeping a fluid movement.
![camera trap](cameraTrap.png)

# Target Audience

This game is mainly focused on people (especially males) who had their first
contact with the Harry Potter’s universe when they were about 12-15 years old.
That’s because they would be 30+ years old now, and that audience is the most
likely to be able to afford a game and be willing to buy it.

Even though 30+ years old males are the main audience, the game will still have
attractive features to younger people and the female audience, as we can see at
the table below:

| **Features**      | **Audience genre & age**  |
|-------------------|---------------------------|
|Action             | Male (15+)                |
|Fast-paced         | Male (15+)                |
|Strategy           |Female (15+), Male (30+)   |
|Multiple solutions |Female (15+)               |
|Pixel Art          |General (30+)              |
|Visual Effects     |General                    |
|“Kawaii” aspect    |Female (15+)               |
