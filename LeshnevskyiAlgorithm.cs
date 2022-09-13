using System.Collections.Generic;
using Robot.Common;
using System.Linq;

namespace Leshnevskyi.Nazarii.RobotChallenge {
	public class LeshnevskyiAlgorithm : IRobotAlgorithm {
		public string Author => Constants.AuthorName;

		public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map) {
			var robot = robots[robotToMoveIndex];
			var privateRobots = robots.Where(currRobot => currRobot.OwnerName == robot.OwnerName).ToList();
			var nearestStation = map.GetNearestStation(robot);
			var distanceToNearestStation = nearestStation.Position.GetDistanceTo(robot.Position);
			var nearestRobots = robot.GetNearestEnemies(robots, Constants.TakeNearestRobot);

			int energyProfitForFight = 0;
			Robot.Common.Robot robotToFight = null;

			if (nearestRobots.Any()) {
				foreach (var currRobot in nearestRobots) {
					int energyProfit = robot.GetAttackProfit(currRobot);

					if (energyProfit < 0) continue;

					if (energyProfit > energyProfitForFight) {
						energyProfitForFight = energyProfit;
						robotToFight = currRobot;
					}
				}
			}

			var stationIsTakenByPrivateRobot = privateRobots.Any(currentRobot => 
				currentRobot.Position.GetDistanceTo(nearestStation.Position) == 0
			);

			if (
				privateRobots.Count == Constants.MaxPrivateRobotCount
				&& robot.Position.GetDistanceTo(nearestStation.Position) != 0
				&& !stationIsTakenByPrivateRobot
			) {
				return new MoveCommand() {NewPosition = nearestStation.Position};
			}

			return robot.Energy > Constants.EnergyNewRobotCreate && privateRobots.Count < Constants.MaxPrivateRobotCount
				? new CreateNewRobotCommand() {
						NewRobotEnergy = Constants.EnergyNewRobot
					}
				: distanceToNearestStation < Constants.MaxDistanceToEnergyStation && nearestStation.Energy > 0
				? (RobotCommand)new CollectEnergyCommand()
				: energyProfitForFight >= Constants.MinEnergyProfitForFighting && robotToFight != null
				? new MoveCommand() {NewPosition = robotToFight.Position}
				: !stationIsTakenByPrivateRobot
				? new MoveCommand() {NewPosition = nearestStation.Position}
				: new MoveCommand() {NewPosition = map.FindFreeCell(robot.Position, robots)};
		}
	}
}
