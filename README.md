# TurnBasedPlayground

## Description

This project explores the use of a work-in-progress MVVM-like framework for creating a unity project. I have written a stackoverflow inspired version for purely observable values. Here this framework relies on a primary Battle Conductor which coordinates and sets up all the simulation, viewmodels, and initializes the views. This is not a fully traditional implementation of the MVVM, just one approach since I haven't had an opportunity to fully try and build one up before. A lot of this is mine but the ideas come from a lot of different stack overflow threads and these talks https://www.youtube.com/watch?v=dgaCPnWadEo and this one https://medium.com/etermax-technology/embracing-changes-with-mvvm-14fcf6d35468

## How to extend

The way this framework works is the following:

Battle Constructor controls models and general game logic, it passes action commands to the Battle System in order to continue battle. It houses the updates to the viewmodels.
Viewmodels are all setup to receive context from the various contexts and game going-ons from the battle constructor. Once set up, only a few with live data (HP related) need to be manually called to update (this way the Battle System is never aware of any viewmodels it is being watched by). 

Viewmodels then house all the various commands that need to be applied back onto the Conductor and also send notifications to their views via Observables. These Observables are one approach to the NotifyProperty changed pattern common in other MVVMs. I didn't like sending the whole object through and wanted to try this for the project. 

Views are all the Monobehaviors that handle UI GameObjects and Scene GameObjects. They are intended to be as dead-simple as possible often with direct references to all their relevant children game objects. They do the minimal of setting up intialized values from viewmodels and subscribing to events on the viewmodel (except for one case where it is a "sub"-view, the Battle Action Row View). As a general rule, if it is **visible** on screen, then it is a View and has a corresponding viewmodel class. The benefit to this is a reactive UI system that responds to things happening in the game layer (triggering self-damage animations is one such example of this).

THe workflow that ended up being comfortable for me was the following:
1. Make the relevant prefab view
2. Setup the view class with the intended subscriptions and behaviors
3. Create viewmodels that feed those
4. Manage the viewmodels in the Conductor
5. Resolve holes through testing
6. Upgrade View as needed with animations

It's not the fastest but it is pretty robust against random errors and can be done reliably. The general guideline is this: if your view needs to know something in your model, you've done something wrong. If you model needs to know something in the view, something is wrong. 

## Next Steps

Number one on the To Do list is a way service the need for creating Prefabs without needing existing instances of them. A view creation service + the means to create viewmodels that bind tho them in one shot is needed desperately in this. It's cumbersome to create Unity prefabs and then also sideload their viewmodels. Right now, having all the game objects initalized works because the scene is set, however, if we needed to do anything like ... load an adventure scene, this problem would need to be addressed.

Number Two is the issue with how animations were implemented. In a better world, ActionData would hold a reference to whatever controller override it needed and then the characters can perform overriden animations. It's clumsy for now.

Another thing I noticed is that because I relied on the Observable property over the INotifyPropertyChanged interface, I couldn't make an IViewModel that would be useful for enforcing *contracts* between Views and Viewmodels. For my own purposes, this is an ok thing, but I can see this being a difficult to maintain point in the future for onboarding.


# Resource credits

Ground texture is *Grass 12.png* from https://opengameart.org/content/blended-textures-of-dirt-and-grass
Skybox exture is *PurpleBlueSky.png* from https://crystallotus.itch.io/skybox-textures
Assets generated from a certain Figma