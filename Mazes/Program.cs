
using MazeGenerator;
using Mazes;
using MazeSolver;

var mazeWidth = 30;
var mazeHeight = 30;
var videoWidth = 600;
var videoHeight = 600;

var generationAlgorithm = new AldousBroder(new MazeLibrary.Random(), mazeWidth, mazeHeight);
var solverAlgorithmFactory = new WallFollowerFactory();

var gen = new GenerateAndSolveMaze(generationAlgorithm, solverAlgorithmFactory);

gen.Go(videoWidth, videoHeight);