using System;
using System.Collections.Generic;
using System.Text;

namespace Leshnevskyi.Nazarii.RobotChallenge {
  class Constants {
    public const string AuthorName = "Nazarii Leshnevskyi";

    public const int MaxDistanceToEnergyStation = 2;
    public const int AttackingRobotEnergyLoss = 30;
    public const double EnemyEnergyProfitCoefficient = 0.1;

    public const int EnergyToFight = 20;
    public const int MinEnergyToFight = 5;
    public const int EnergyNewRobot = 100;
    public const int EnergyNewRobotCreate = 400;
    public const int TakeNearestRobot = 10;
    public const int MaxNearestRobots = 2;
    public const int MinEnemyEnergy = 600;
  }
}
