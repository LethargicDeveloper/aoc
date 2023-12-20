using AocLib;

/*
 * 
 * Libraries
 *  Benchmark.net - https://benchmarkdotnet.org/
 *  MoreLINQ - https://github.com/morelinq/MoreLINQ
 *  OptimizedPriorityQueue - https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
 *  QuikGraph - https://github.com/KeRNeLith/QuikGraph
 *  
 */

var puzzle = new AdventOfCode._2023.Day13.Part02();
puzzle.Solve("sample").Log();

#if !DEBUG
BenchmarkDotNet.Running.BenchmarkRunner.Run<AdventOfCode._2023.Day11.Part02>();
#endif
