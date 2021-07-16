# LHRP: Liquid Handling Robotic Platform

## Current Features
1. Devices
	* Basic simulated pipettor with tip pickup, drop, aspirate and dispense
	* Basic pipette and liquid tracking

1. Instruments
	* Base Instrument and coordinate system management
	* Basic Deck design, with positions
	* Liquid management
	* Tip management
	
1. Labware Definition
	* Tip racks, Plates, and general Liquid Containers
	* Definition for labware, with basic shapes and simple volume calculations
	
1. Liquid Definition
	* Basic liquid definitions (name/id)
	* Basic liquid mixing logic
	
1. Protocols
	* Protocol, step and command level programming
	* Steps: One-to-One transfer, Liquid transfer, Custom step
	* Commands: Aspirate, Liquid Aspirate, Dispense, Tip Pickup and Drop
	* Transfer pattern optimization based on pipetting device
	
1. Runtime
	* Runtime engine and command queue interfaces
	* Compilation / verification logic
	* Basic error handling logic and interfaces
	* Resource usage calculation (tips, liquids, etc...) for each command/step/protocol
	* Basic simulation and state snapshot

## Work in Progress

* Testing! Testing! Testing!
* Device interfaces implementations
* Expand and enhance error handling
* Add more commands and step types
* Prototyping scripting for creating custom steps or protocols

## Future Features

* Labware library
* IO operations: Ability to serialize/deserialize protocols, labware, instruments, etc
* Handle labware stacking (tip racks and plates)
* Labware transport support
* Serial pipetting step types
* Support other device types, washers, readers, incubators, etc...
* Multi-threaded or parallelizing of commands
* More...

