This repository contains the tools and patches used to mod Magicka.

## Contributing

**THIS GUIDE DETAILS THE STEPS THAT WILL BE REQUIRED TO CONTRIBUTE. THE TOOLKIT IS NOT YET COMPLETE.**

To contribute to the modloader, follow these steps:
- Fork this repository and clone it onto your computer into the Magicka installation folder;
- Open `Tools.sln` under the Tools directory and run/build the `ModdingToolkit` project;
- Run said Toolkit and answer `y` to the first question (`Are you a modder ?`);
- Enter the number corresponding to the `Setup Environment` command and wait for it to finish, answering any question asked during the process;
- Assuming the setup process completed succesfully, an explorer window should open with a specific folder opened. This is the solution you want to edit when adding features to the ModLoader;
- **Make sure to always test your changes;**
- To push your changes to your repository, open the Modding Toolkit as a modder and run the `Create Patches` command. This will generate the list of changes in the `Patches` folder, which you can then push;
- **Make sure to run `Apply Patches` whenever updating from remote;**

All these steps are required since we cannot legally upload the modified executable or source code.
The installer will always setup the environment in the same folder as Magicka is. **Do not move it from that folder.** This will be changed in the future.