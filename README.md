# AceJump For Visual Studio

Allows for quick movement around the editor screen.  Inspired by [AceJump for Webstorm]((http://plugins.jetbrains.com/plugin/?idea&pluginId=7086)) and Vim plugin AceJump. 

Download it at https://marketplace.visualstudio.com/items?itemName=jsturtevant.AceJump

## Use
1. Press "Ctrl + Alt + ;" to display a key selector.  
2. Press any letter to highlight occurrences in the text editor.   
3. Press highlighted character and your cursor jumps to that spot!

![demo of acejump for visual studio](https://github.com/jsturtevant/ace-jump/blob/master/ace-jump-demo.gif)

To change keybinding: 

1. Open the Keybord settings at ```Tools->Options->Environment->Keyboard```.  
2. Type ```acejump``` in the ```show commands containing``` dialog box.  
3. Enter new keyboard shortcut and hit assign.
4. Hit OK

![edit keyboard binding](https://github.com/jsturtevant/ace-jump/blob/master/vs-edit-keyboard-bindings.png)

## Contributors
Thanks to everyone who has contibuted to the project:

- [mgutekunst](https://github.com/mgutekunst)
- [aerworker](https://github.com/aerworker)
- awesome unknown person when first released 
- [m0d7](https://github.com/m0d7)
- [jpcrs](https://github.com/jpcrs)

## Build yourself

1. [Install VS 2017 Extensibility Workload] (https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk)
2. Build Project using VS Build
3. Run Unit Tests

Manually test in Experimental version:

1. Right click on Project file and select properties:
2. Select Debug Tab
3. Select ```Start External Program``` and enter path to ```devenv.exe```.  Usually something like: ```C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe```
4. Under ```Start Options->Command Line Arguements``` enter ```/rootSuffix Exp```
5. Start Debugging and Experimental version of VS will startup and you can set breakpoints in the code.
