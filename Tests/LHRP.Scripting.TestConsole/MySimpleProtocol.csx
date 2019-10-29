
Protocol.SetRuntimeEngine(SimulationEngine);

Protocol.AddLabware("costar_96", 1, 1);
Protocol.AddLabware("costar_96", 2, 2);
Protocol.AddTips("ntr_300", 3, 300);

Protocol.PickUpTips(300, "11");
Protocol.Aspirate(1, "A1,A2", "11", 50.0, "water");
Protocol.Dispense(2, "C1,C5", "11", 50.0, "water");
Protocol.DropTips(false);

Protocol.Run();