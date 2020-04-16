# WS2812b-DotNET-Controller
This is an application for controlling WS2812B Led Strips. Project uses Arduino as a hardware controller and I2C AT24C128 EEPROM Memory for holding up to 10 led Patterns. Each Pattern can drive up to 150 leds. 
## Getting Started
### Prerequisites
Things that are required to use this project:
- Visual Studio with .NET Framework
- Arduino IDE
- Arduino Board
- WS2812b Leds or Strip
- AT24C128 EEPROM Memory
### Wiring
EEPROM Memory is connected to I2C bus by SDA and SCL lines (Pin 2 and 3 on Arduino Pro Micro board).
By default optional status diode is connected to pin 4 and LED Strip is connected to pin 5.
### Connecting to Arduino
After starting the App you should see 5 settings that are required to set before connecting.
- **Serial**: This should be detected by application. 
- **Baud Rate**: Default is 9600
- **Parity**: Should be set to **None**
- **Stop Bits**: Should be set to **One**
- **Handshake**: Should be set to **None**
### Status Diode
When connected or when Arduino is rebooted status diode should blink twice with blue color. To check if everything is OK you can send this command via Serial:
```
CONN
```
After sending this command status diode should blink twice with green color.

While uploading new pattern to Arduino status diode should blinking with red color and when all data is successfully received by Arduino status diode should blink twice with green color.
