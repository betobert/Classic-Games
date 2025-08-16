using TicTacToe.Core;
using TicTacToe.Cli;

var bot = new HeuristicBot();
var gameLoop = new GameLoop(bot);
gameLoop.Run();
