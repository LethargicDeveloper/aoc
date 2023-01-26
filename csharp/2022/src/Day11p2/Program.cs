#if DEBUG
new PuzzleSolver().Solve().Log();
#else
BenchmarkDotNet.Running.BenchmarkRunner.Run<PuzzleSolver>();
#endif