# Overview
The HoloLens Sync plugin for Grasshopper consists of 6 components.

![Components](./Images/Components.png)

In the "GH->Unity" category, it can send the mesh model and the 6-DoF pose of QR Code from Rhino to Unity.

![Send Mesh](./Images/Send_Mesh_to_Unity.png)![Send QRCode](./Images/Send_QRCode.png)

In the "Unity->GH" category, it can get the 6-DoF pose of HoloLens, eye gaze, and hand joints from Unity.

![Get HL](./Images/Display_HoloLens_in_Rhino.png)
![Get EyeGaze](./Images/Display_GazeDir_in_Rhino.png)
![Get HandJoints](./Images/Display_HandJoints_in_Rhino.png)
![Get QR Code](./Images/Get_QRCode.png)

# Features

1. Display the HoloLens, EyeGaze, and HandJoints.
![Display Human Factors](./Images/Result.png)
2. Display mesh models in the HoloLens view.
![Display Model](./Images/Result2.png)

# Sample

Note: The sample GH files `HL-Unity-GH(with QR Code).gh` and `HL-Unity-GH(withouot QR Code).gh` should be used together with the Unity project in `Unity_Project_RIU+MR` folder. Ensure that the following modules are added to the project: `MRTK3`, `OpenXR`, `Microsoft.MixedReality.QR`, and `Microsoft.VCRTForwarders`.