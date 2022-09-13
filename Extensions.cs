using System;
using Robot.Common;
using System.Linq;
using System.Collections.Generic;

namespace Leshnevskyi.Nazarii.RobotChallenge {
	public static class Extensions {
		public static int GetDistanceTo(this Position thisRosition, Position anotherPosition) {
			return (int)Math.Sqrt(
				Math.Pow(thisRosition.X - anotherPosition.X, 2) + Math.Pow(thisRosition.Y - anotherPosition.Y, 2)
			);
		}

		public static EnergyStation GetNearestStation(this Map map, Robot.Common.Robot robot) {
			return map.Stations
				.OrderBy(station => station.Position.GetDistanceTo(robot.Position))
				.FirstOrDefault();
		}

		public static int GetEnergyLossTo(this Robot.Common.Robot robot, Position toPosition) {
			return (int)Math.Pow(robot.Position.GetDistanceTo(toPosition), 2);
    }

		public static int GetAttackProfit(this Robot.Common.Robot attacker, Robot.Common.Robot enemy) {
			return (int)(enemy.Energy * Constants.EnemyEnergyProfitCoefficient) 
				- attacker.GetEnergyLossTo(enemy.Position)
				- Constants.AttackingRobotEnergyLoss;
		}

		public static Robot.Common.Robot GetMostProfitableEnemy(
			this Robot.Common.Robot attacker, IList<Robot.Common.Robot> robots
		) {
			return robots
				.Where(robot => robot.OwnerName != attacker.OwnerName)
				.OrderBy(enemy => attacker.GetAttackProfit(enemy))
				.FirstOrDefault();
		}

		public static List<Robot.Common.Robot> GetNearestEnemies(
			this Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, int count
		) {
			if (count <= 0) return new List<Robot.Common.Robot>();

			Dictionary<Robot.Common.Robot, int> robotDistance = new Dictionary<Robot.Common.Robot, int>();

			foreach (var currentRobot in robots) {
				if (currentRobot.OwnerName != robot.OwnerName) { 
					robotDistance.Add(currentRobot, robot.Position.GetDistanceTo(currentRobot.Position));
				}
			}

			robotDistance = robotDistance
					.Where(r => r.Key.Energy >= Constants.MinEnemyEnergy)
					.OrderBy(r => r.Value)
					.ToDictionary(r => r.Key, r => r.Value);

			return robotDistance.Keys.Take(count).ToList();
		}
	}
}
