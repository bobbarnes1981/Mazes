
using MazeGenerator;
using Mazes;
using MazeSolver;

var mazeWidth = 30;
var mazeHeight = 30;
var videoWidth = 600;
var videoHeight = 600;

var generationAlgorithm = new AldousBroder(mazeWidth, mazeHeight);
var solverAlgorithm = new WallFollower();

var gen = new GenerateAndSolveMaze(generationAlgorithm, solverAlgorithm);

gen.Go(videoWidth, videoHeight);