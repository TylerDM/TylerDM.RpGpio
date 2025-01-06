# Introduction
RpGpio provides a more opinionated wrapper over the GPIO APIs present in System.Device.Gpio which are purpose built for use with modern (40 pin) Raspberry Pi hardware.  

Features:
- Prevents invalid states such as opening a pin in read mode, then writing to it.
- Provide DI support out of the box.
- Add abstraction classes that allow rapid use of common hardware.
- Abstract away if you're using Input, InputPullUp, or InputPullDown.

# Devices
Wrapper classes are provided for common hardware and provide common featuring like debouncing and noise filtering.  Check the Devices folder to see all currently available device wrappers.

Implemented as of time of writing:
- Buzzers
- LEDs
- Pulsers
- Relays
- Parallax 4x4 Keypad
- Buttons