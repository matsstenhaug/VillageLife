using UnityEngine;
using System.Collections;

public class GeneticAlgorithm : MonoBehaviour
{
    public static int CHROMOSOME_SIZE = 3; // Amount of stats to be mutated
    public static int POPULATION_SIZE = 40; // Amount of generated genes
    public static int EVALUTION_TRIALS = 10; // number of simulated iterations for each genome
    public static int GENERATION_SIZE = 5; // initial generation size. The new genes that will be made are based on these

    ArrayList mPopulation; // collection of all genes
    VillagePeopleSimulator currVPS; // Current simulation 

    public GeneticAlgorithm(int size, VillagePeopleSimulator vps)
    {
        currVPS = vps;

        // initialize the arraylist and each gene's initial weights HERE
        mPopulation = new ArrayList();
        for (int i = 0; i < size; i++)
        {
            Gene entry = new Gene();
            entry.randomizeChromosome();
            mPopulation.Add(entry);
        }
    }

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
        for (int i = 0; i < mPopulation.Count; i++) {
            /*
                Here we go through all the genes, simulate their existance, and get the fitness based on damage and kills
            */
            ((Gene)mPopulation[i]).mFitness = runExperimentDisease(EVALUTION_TRIALS, ((Gene)mPopulation[i]).mChromosome);
        }
    }

    public float getFitnessDisease(VillagePeopleSimulator vls)
    {
        /*
            Calculates fitness for a simulated state
            returns the fitness as float
        */
        float killValue = 300;
        float damageValue = 1;
        float fitness = (vls.damageDealt * damageValue) + (vls.kills * killValue);
        return fitness;
    }

    public float runExperimentDisease(int TRIALS, float[] chromosome) {
        /*
            The main experimental simulation method.
            Goes through all simulated steps, and checks fitness afterwards.
        */
        Disease d = new Disease(chromosome[0], chromosome[1], 0, chromosome[2], null);
        VillagePeopleSimulator vls = currVPS.Clone();
        vls.AddDisease(d);
        vls.isSimulation = true;
        while (vls.getEntities().Count > 0 && vls.getIterations() <= TRIALS) {
            vls.SimulateNextStep();
        }

        #region Debug Prints 
        //USED FOR DEBUGGING PURPOSES
        /*
        int infected = 0;
        Debug.Log("Dmg = " + vls.damageDealt);
        Debug.Log("Current Chromosomes: [" + chromosome[0] + ", " + chromosome[1] + ", " + chromosome[2] + "]");
        foreach(Entity e in vls.getEntities())
            if(e.infections.Count > 0) { infected++; }
        Debug.Log("Population size: " + vls.getEntities().Count);
        Debug.Log("People Infected by Disease: " + infected);
        Debug.Log("Disease Count: " + vls.getDiseases().Count);
        */
        #endregion
        float fitness = getFitnessDisease(vls);
        Destroy(vls);
        return fitness;
    }

    public void produceNextGeneration()
    {
        /*
            Uses Roulette Selection to create offsprings of existing genes.
            The next generation are added to the mPopulation arraylist
        */
        mPopulation.Sort((IComparer)new GeneComparator());

        #region Roulette Selection
        while(mPopulation.Count < POPULATION_SIZE) {
            Gene[] pars = new Gene[2];

            for(int a = 0; a < 2; a++) {
                pars[a] = rouletteSelection();
            }
            if (pars[0] != null && pars[1] != null) {
                Gene[] offspr = pars[0].reproduce(pars[1]);
                for (int j = 0; j < offspr.Length; j++)
                {
                    mPopulation.Add(offspr[j]);
                }
            }
        }
        #endregion
    }

    public Gene rouletteSelection()
    {
        /*
            Gets random gene from the population
            and which is to be paired with another selected gene.
        */
        float totalScore = 0;
        float runningScore = 0;
        foreach(Gene ge in mPopulation) 
        {
            totalScore += ge.getFitness();
        }
        float rnd = (float)Random.Range(0, 1) * totalScore;
        foreach(Gene ge in mPopulation)
        {
            if( rnd >= runningScore && rnd <= runningScore + ge.getFitness())
            {
                return ge;
            }
            runningScore += ge.getFitness();
        }

        return null;
    }

    public int size() { return mPopulation.Count; } // returns the size of mPopulation

    public Gene getGene(int index) { return (Gene)mPopulation[index]; } // returns the gene at a given index

    public Gene StartAlgorithm() // return the best fit
    {
        // Initializing the population. A population contains few AI's
        // The small size is based on the few variables included and
        // the time it takes to run the GA
        
        int generationCount = 0;
        Gene bestGene = null;
        ArrayList best = new ArrayList();
        // The algorithm runs until the population has undergone at least several mutations
        while (generationCount < GENERATION_SIZE)
        {
            // --- evaluate current generation:
            evaluateGeneration();
            // --- print results here:
            // we choose to print the average fitness,
            // as well as the maximum and minimum fitness
            // as part of our progress monitoring
            float maxFitness = float.NegativeInfinity;//Float.NEGATIVE_INFINITY;
            string bestIndividual = "";
            string worstIndividual = "";
            for (int i = 0; i < size(); i++)
            {
                float currFitness = getGene(i).getFitness();
                
             
                if (currFitness > maxFitness)
                {
                    maxFitness = currFitness;
                    bestIndividual = getGene(i).getPhenotype();
                    best.Add(getGene(i));
                }
            }
            // produce next generation:
            produceNextGeneration();
            generationCount++;
        }
        foreach(Gene g in best)
        {
            if(bestGene == null || g.mFitness > bestGene.mFitness)
            {
                bestGene = g;
            }
        }
        Debug.Log(bestGene.getPhenotype()+", Fitness: " + bestGene.mFitness+".");
        return bestGene;
    }
}

