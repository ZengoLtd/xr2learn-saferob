Assets.zip downloadable from: https://drive.zengo.eu/s/EAQJD8rHkLAtJTE


- extract assets.zip into the project
- open the project and press ignore for the safemode
- Import VR Interaction Framework.unitypackage
- Move BNG Framework folder into VR Core
- Install https://assetstore.unity.com/packages/tools/network/pun-2-free-119922 asset
- Install Final IK.unitypackage
- Import PUN integration (BNG FrameWork/Integrations/PUN)
- Import FinalIK integration (BNG FrameWork/Integrations/FinalIK)
- Install https://assetstore.unity.com/packages/tools/animation/leantween-3595
- Install https://assetstore.unity.com/packages/tools/utilities/auto-asmdef-156502
- Install https://assetstore.unity.com/packages/tools/utilities/b-zier-path-creator-136082
- Reopen Project
- Delete any AutoASMDEF folder
- Alt+Q open Auto-ASMDEF plugin and update assembly definitions
- Under advanced settings remove playmode
- Start process
- Under Plugins/RootMotion creat a new AsseblyDefinition and call it RootMotion
- Add the newly created RootMotion assembly to Auto-asmdef-SecondPass
- Import TMP
- Open Persistent Scene
- Add Persistent Scene to Build Scenes
- Add SceneSelector Scene to Build Scenes
- Add Scenario 1 to Build Scenes
- Add Scenario 2 to Build Scenes
- If you want to use networking configure photon
