using UnityEngine;
using System.Collections;

public class GeneticAlgorithm
{
    public static int CHROMOSOME_SIZE = 3;
    public static int POPULATION_SIZE = 40;
    public static int EVALUTION_TRIALS = 10;
    public static int GENERATION_SIZE = 100;

    ArrayList mPopulation;

    public GeneticAlgorithm(int size)
    {
        // initialize the arraylist and each gene's initial weights HERE
        mPopulation = new ArrayList();
        for (int i = 0; i < size; i++)
        {
            Gene entry = new Gene();
            entry.randomizeChromosome();
            mPopulation.Add(entry);
        }
    }

    public void evaluateGeneration()
    {
        for (int i = 0; i < mPopulation.Count; i++)
        {

            //// NEEDS TO BE CHANGED TO OUR SIMULATOR , NOT PACMAN ///
           // Executor exe = new Executor();
            // calculates the fitness passed from a number of trials
           // mPopulation.get(i).mFitness = exe.runExperiment(new PacmanStateMachine(mPopulation.get(i).mChromosome),
                 //   new StarterGhosts(), EVALUTION_TRIALS);
        }
    }

    public void produceNextGeneration()
    {
        // use one of the offspring techniques suggested in class (also applying any mutations) HERE
        mPopulation.Sort();
        //Collections.sort(mPopulation, Collections.reverseOrder(new GeneComparator()));
        while (mPopulation.Count > POPULATION_SIZE / 2)
        {
            mPopulation.RemoveAt(mPopulation.Count - 1);
        }
        int i = 0;
        while (mPopulation.Count < POPULATION_SIZE)
        {
            Gene[] offspring = ((Gene)mPopulation[i]).reproduce((Gene)mPopulation[i + 1]);
            for (int j = 0; j < offspring.Length; j++)
            {
                mPopulation.Add(offspring[j]);
            }
            i++;
        }
        for (int k = POPULATION_SIZE / 2; k < POPULATION_SIZE; k++)
        {
            ((Gene)mPopulation[k]).mutate();
        }
    }

    public int size() { return mPopulation.Count; }

    public Gene getGene(int index) { return (Gene)mPopulation[index]; }

    public void main(string[] args)
    {
        // Initializing the population. A population contains few AI's
        // The small size is based on the few variables included and
        // the time it takes to run the GA
        GeneticAlgorithm population = new GeneticAlgorithm(POPULATION_SIZE);
        int generationCount = 0;
        // The algorithm runs until the population has undergone at least several mutations
        while (generationCount < GENERATION_SIZE)
        {
            // --- evaluate current generation:
            population.evaluateGeneration();
            // --- print results here:
            // we choose to print the average fitness,
            // as well as the maximum and minimum fitness
            // as part of our progress monitoring
            float avgFitness = 0.0f;
            float minFitness = float.PositiveInfinity;//Float.POSITIVE_INFINITY;
            float maxFitness = float.NegativeInfinity;//Float.NEGATIVE_INFINITY;
            string bestIndividual = "";
            string worstIndividual = "";
            for (int i = 0; i < population.size(); i++)
            {
                float currFitness = population.getGene(i).getFitness();
                avgFitness += currFitness;
                if (currFitness < minFitness)
                {
                    minFitness = currFitness;
                    worstIndividual = population.getGene(i).getPhenotype();
                }
                if (currFitness > maxFitness)
                {
                    maxFitness = currFitness;
                    bestIndividual = population.getGene(i).getPhenotype();
                }
            }
            if (population.size() > 0) { avgFitness = avgFitness / population.size(); }
            string output = "Generation: " + generationCount;
            output += "\t AvgFitness: " + avgFitness;
            output += "\t MinFitness: " + minFitness + " (" + worstIndividual + ")";
            output += "\t MaxFitness: " + maxFitness + " (" + bestIndividual + ")";
            //System.out.println(output);
            // produce next generation:
            population.produceNextGeneration();
            generationCount++;
        }
    }
}

