
using MazeGenerator;
using Mazes;
using MazeSolver;

var mazeWidth = 10;
var mazeHeight = 10;
var videoWidth = 500;
var videoHeight = 500;

var generationAlgorithm = new AldousBroder(mazeWidth, mazeHeight);
var solverAlgorithm = new WallFollower();

var gen = new GenerateAndSolveMaze(generationAlgorithm, solverAlgorithm);

gen.Go(videoWidth, videoHeight);