using Autofac;
using MazeGenerator;
using MazeLibrary;
using Mazes;
using MazeSolver;
using Microsoft.Extensions.Configuration;
using Random = MazeLibrary.Random;

var configuration = new MazeConfiguration();
new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .Build().Bind(configuration);

var builder = new ContainerBuilder();
builder.RegisterType<Random>().As<IRandom>();
builder.RegisterInstance(configuration).As<MazeConfiguration>();
builder.RegisterType<RandomisedDepthFirstSearch>().As<IGenerationAlgorithm>();
builder.RegisterType<WallFollowerFactory>().As<ISolverAlgorithmFactory>();
builder.RegisterType<GenerateAndSolveMaze>().As<GenerateAndSolveMaze>();
var container = builder.Build();

var render = container.Resolve<GenerateAndSolveMaze>();
render.Go();