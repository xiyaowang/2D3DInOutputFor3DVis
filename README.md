# 2D3DInOutputFor3DVis
This repository contains program code used in the study of the paper "" submitted to Elsevier's International Journal of Human Computer Studies.
This project was coded with Unity and includes four parts: input with a normal PC, input with the Google Tango Tablet, output with a normal PC, and output with the Microsoft HoloLens version 1. 

For each program, sost relevant files are under the Assets folder:
- Materials: textures for object rendering (such as the flow data simulation).
- Model: teapot model.
- Scene: Unity scene.
- Scripts: all scripts.
   -dd
- Shaders: shaders to render the scene, including slice-based volume rendering for the flow data visualization and simple mesh renderer for the teapot.

## S23_Input_PC 
This folder contains the Unity project running on a normal PC. This program captures and process the input from mouse/keyboard and space mouse. 

This project was coded under Unity 2018.4.3.f1 with C Sharp. For capturng and processing the mouse and keyboard input, no additional resources was required. For the space mouse input, it is required to have the SpaceNavigator Driver [(https://assetstore.unity.com/packages/tools/spacenavigator-driver-9774)](https://assetstore.unity.com/packages/tools/spacenavigator-driver-9774). Currently this driver is included in the project.

This program also logs users' manipulation. If this program is running within Unity, logs will be saved under the related subfolder of AppData of Unity. An example location is C:\Users\USER_NAME\AppData\...

## S23_Input_Tablet_Unity5
This folder contains the Unity project running on a Google Tango Tablet. This program captures the process the input from the tangible tablet.

This project was coded under Unity 5.6.7f1 (due to some package-related reasons, it is necessary to use an old version of Unity) with C Sharp. The required Google Tango SDK [(https://github.com/googlearchive/tango-examples-unity)]
(https://github.com/googlearchive/tango-examples-unity) is already included in the folder. It is sufficient to directly compile and run it on the tablet.

This program also logs users' manipulation. The files are stocked inside the tablet's storage folder related to this app.

## S23_Output_Screen
This folder contains the Unity project running on a normal PC. This program visualizes the study task scenes and reflects on the users' interaction.

This project was coded under Unity 2018.4.3.f1 with C Sharp.

## S23_Output_HoloLens
This folder contains the Unity project running on a normal PC. This program visualizes the study task scenes and reflects on the users' interaction.

This project was coded under Unity 2018.4.3.f1 with C Sharp.

## Network Communications
For the input programs (both for the PC and the tablet), it is required to fill in the local IP address of the output devices at the start page.

## How to Use
For all programs, it is sufficient to open them with the correct unity version. For the programs running on the PC, it is both ok to run them within the unity editor or first compile them and run seperately.
For the programs running on the tablet or on the HoloLens, it is needed to load them into the devices with Unity Editor.
