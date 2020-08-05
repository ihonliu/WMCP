# WMCP
WMCP(Windows Media Control Panel) is a panel used for control window media in tray area.

## Description
The goal of WMCP is to implement a [media-plasmoid](https://www.kde.org/announcements/plasma-5.13/media-plasmoid.png)-like panel on Windows 10.

## How to use
1. Download 
1. Compile 
1. Run 
1. Check your tray area

## ToDo
+ Add change current session function
+ Time line adjustment to change media progress
+ Album cover presentation
+ Read system color setting and Change color by dark mode
+ Popup animation
+ Launch optimization
+ Memory optimization
+ Tray icon presentation optimization

## Limitations
+ Most application does not provide time line properties, so the progress bar won't work in most situations
+ Global current session cannot be changed by API provided by Microsoft right now (May there be some APIs I can invoke)

## Dependencies

+ FluentWPF
+ WPF-NotifyIcon
+ InputSimulatorCOre
+ Microsoft.WindowsSDK.Contracts

 All packages can be found on Nuget.org

## Thanks

This project is made possible by 
+ [FluentWPF](https://github.com/sourcechord/FluentWPF)
+ [Hardcoddet.NotifyIcon.Wpf.NetCore](https://github.com/hardcodet/wpf-notifyicon)
+ [Input Simulator Core](https://github.com/cwevers/InputSimulatorCore)


## License 
[GPL License](LICENSE)
