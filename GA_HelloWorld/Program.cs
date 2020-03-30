using System;
using System.Collections.Generic;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Extensions.Mathematic;

namespace GA_HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Problem: Simple addition
            // 1 + 1 = 2
            // 2 + 2 = 4
            // 3 + 2 = 5
            var myInputs = new List<FunctionBuilderInput>
            {
                new FunctionBuilderInput(new List<double> {1, 1}, 2),
                new FunctionBuilderInput(new List<double> {2, 2}, 4),
                new FunctionBuilderInput(new List<double> {3, 2}, 5)
            };
            var myFitness = new FunctionBuilderFitness(myInputs.ToArray());
            var myChromosome = new FunctionBuilderChromosome(myFitness.AvailableOperations, 5);
            var selection = new EliteSelection(); 
            var crossover = new ThreeParentCrossover(); 
            var mutation = new UniformMutation(true);
            var fitness = myFitness; 
            var chromosome = myChromosome;
            var population = new Population(100, 200, chromosome)
            {
                GenerationStrategy = new PerformanceGenerationStrategy()
            };
            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new FitnessThresholdTermination(0)
            };
            ga.GenerationRan += delegate
            {
                Console.Clear();
                var bestChromosome = ga.Population.BestChromosome;
                Console.WriteLine("Generations: {0}", ga.Population.GenerationsNumber);
                Console.WriteLine("Fitness: {0,10}", bestChromosome.Fitness);
                Console.WriteLine("Time: {0}", ga.TimeEvolving);
                Console.WriteLine("Speed (gen/sec): {0:0.0000}", ga.Population.GenerationsNumber / ga.TimeEvolving.TotalSeconds);
                var best = bestChromosome as FunctionBuilderChromosome;
                Console.WriteLine("Function: {0}", best.BuildFunction());
            };
            ga.Start();
            Console.WriteLine("Evolved.");
            Console.ReadKey();
        }
    }
}
