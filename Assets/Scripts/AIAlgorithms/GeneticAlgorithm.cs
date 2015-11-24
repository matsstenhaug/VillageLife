using UnityEngine;
using System.Collections;

public class GeneticAlgorithm
{
    public static int CHROMOSOME_SIZE = 3;
    public static int POPULATION_SIZE = 40;
    public static int EVALUTION_TRIALS = 10;
    public static int GENERATION_SIZE = 5;
    public static int SIMULATION_ITERATIONS = 10;

    ArrayList mPopulation;
    ArrayList currEnts = new ArrayList();

    public GeneticAlgorithm(int size, ArrayList ents)
    {
        this.currEnts = ents;

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
        for (int i = 0; i < mPopulation.Count; i++)
        {
			((Gene)mPopulation[i]).mFitness = runExperimentDisease(EVALUTION_TRIALS, ((Gene)mPopulation[i]).mChromosome, currEnts);
			//Debug.Log("Fitness pop#"+i+": "+((Gene)mPopulation[i]).mFitness);

			/*
				Connections: 
				Leth: Mutate (1-100)
				Spread: 100-Leth
				Droprate: Mutate (leth-100)
			 */


            // the less iterations it takes for the population to die, the better. (for the diseases)
            // the more iterations it - the better (for entities)

            /*
                make copies of the "state" and run simulations and return the iterations at end.

            current population
            current diseases
            (general) populate new simulatioin with the new generation with diseases from existing.
                    spawn x children where x is how many children would have been spawned.

                    When do they die?
                    for the sim: create a population based on how many would have spawned on the previous iteration
            ///////////////////////                 
            Make dummy population of 100(example) ents, based on what the "current" pop would have made
            then test that population of 100 for each of the "evolutions" of diseases.
            //////////////////////
            Force-produce children until you have X-children to work with.
            Take these 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            (Entities)
            STEP 1:
                Force-produce 100 children from the actual population
            STEP 2:
                Play simulation over X iterations, or untill they all die; WHichever comes first
            STEP 3: (Evaluation)
                Check to see how many iterations it took.
                Check all the genes we create, in regards to the simulation
                    - How many died, how many were born, how much damage was dealt?(from the created disease)
            STEP 4:
                Select fittest mutation.
                go to STEP 1 and continue 10 times (Evolutioin_trials)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            (Diseases)
            STEP 1:
                Take current population
                Mutate current disease ( the one you wanna make fit )
            STEP 2:
                Play simulation over X iterations, or untill they all die; WHichever comes first
            STEP 3:
                Check to see how many iterations it took.
                Check all the genes we create, in regards to the simulation
                    - How many died, and how much damage was dealt.
            STEP 4:
                select fittest mutation.
                got to Step 1 and do over until done 10 times (Evolution_trials)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           (disease) Create field that sums up total damage taken from a disease, and evaluate.
           (entities) 
            */
            //Debug.Log("Ents in : " + currEnts.Count);
            


            //// NEEDS TO BE CHANGED TO OUR SIMULATOR , NOT PACMAN ///
            // Executor exe = new Executor();
            // calculates the fitness passed from a number of trials
            // mPopulation.get(i).mFitness = exe.runExperiment(new PacmanStateMachine(mPopulation.get(i).mChromosome),
            //   new StarterGhosts(), EVALUTION_TRIALS);
        }
    }

    public float getFitnessDisease(VillageLifeSimulator vls)
    {
        float killValue = 100;
        float damageValue = 1;
        float fitness = (vls.damageDealt * damageValue) + (vls.kills * killValue);
        return fitness;
    }

    public float runExperimentDisease(int TRIALS, float[] chromosome, ArrayList ents)
    {
        VillageLifeSimulator vls = new VillageLifeSimulator();
        Disease d = new Disease(chromosome[0], chromosome[1], 0, chromosome[2], null);
        vls.Init(ents, d);
        ArrayList upEnt = new ArrayList();
        foreach (Entity e in ents) {
            upEnt.Add(e.Copy());
        }
        //Debug.Log("no Ents: " + ents.Count);
        while (upEnt.Count > 0 && vls.getIterations() <= SIMULATION_ITERATIONS)
        {
            //Debug.Log("Iteration no: " + vls.getIterations());
            upEnt = vls.SimulateUpdate(upEnt);
        }
		//Debug.Log ("LOOP GOINE");
        return getFitnessDisease(vls);
    }

    public void produceNextGeneration()
    {
        // use one of the offspring techniques suggested in class (also applying any mutations) HERE
        GeneComparator mycomp = new GeneComparator();
        mPopulation.Sort((IComparer) mycomp);
        //mPopulation.Sort();
        //mPopulation.Reverse();
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

    public Gene StartAlgorithm() // return the best fit
    {
        // Initializing the population. A population contains few AI's
        // The small size is based on the few variables included and
        // the time it takes to run the GA

        //GeneticAlgorithm population = new GeneticAlgorithm(POPULATION_SIZE);
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
            float avgFitness = 0.0f;
            float minFitness = float.PositiveInfinity;//Float.POSITIVE_INFINITY;
            float maxFitness = float.NegativeInfinity;//Float.NEGATIVE_INFINITY;
            string bestIndividual = "";
            string worstIndividual = "";
            for (int i = 0; i < size(); i++)
            {
                float currFitness = getGene(i).getFitness();
                avgFitness += currFitness;
                if (currFitness < minFitness)
                {
                    minFitness = currFitness;
                    worstIndividual = getGene(i).getPhenotype();
                }
                if (currFitness > maxFitness)
                {
                    maxFitness = currFitness;
                    bestIndividual = getGene(i).getPhenotype();
                    best.Add(getGene(i));
                }
            }
            if (size() > 0) { avgFitness = avgFitness / size(); }
            //string output = "Generation: " + generationCount;
          //  output += "\t AvgFitness: " + avgFitness;
          //  output += "\t MinFitness: " + minFitness + " (" + worstIndividual + ")";
          //  output += "\t MaxFitness: " + maxFitness + " (" + bestIndividual + ")";
            //System.out.println(output);
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
        Debug.Log(bestGene.getPhenotype());
        return bestGene;
    }
}

