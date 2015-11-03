/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package AIAlgorithms.EA;

import java.util.Random;

/**
 *
 * @author Zoiids
 */
public class Gene {
    
    /**
     * Random generator for the initial value
     */
    private Random ran;
    
    /**
     * the fitness is the current score the AI achieved in the last simulation.
     * the score is calculated from an average over a number of "generations"
     */
    protected float mFitness;
    
    /**
     
     */
    protected int mChromosome[];
    
    public Gene(){
        mChromosome = new int[GeneticAlgorithm.CHROMOSOME_SIZE];
        mFitness = 0.f;
        ran = new Random();
    }
    
    public void randomizeChromosome(){
        for(int i = 0; i < mChromosome.length; i++){
            mChromosome[i] = ran.nextInt(20) + 1;
        }
    }
    
    public Gene[] reproduce(Gene other){
        Gene[] result = new Gene[1];
        for(int j = 0; j < result.length; j++){
            Gene temp = new Gene();
            for(int i = 0; i < GeneticAlgorithm.CHROMOSOME_SIZE; i++){
                int select = ran.nextInt(2);
                if(select == 0){
                    temp.mChromosome[i] = this.mChromosome[i]; 
                }else{
                    temp.mChromosome[i] = other.mChromosome[i];
                }
            }
            result[j] = temp;
        }
        return result;
    }
    
    public void mutate(){
        for(int i = 0; i < mChromosome.length; i++){
            if(ran.nextBoolean()){
                mChromosome[i] ++;
            }else{
                mChromosome[i] --;
            }
            
            if(mChromosome[i] < 0){
                mChromosome[i] = 0;
            }else if (mChromosome[i] > 100){
                mChromosome[i] = 100;
            }
        }
    }

    public float getmFitness() {
        return mFitness;
    }

    public void setmFitness(float mFitness) {
        this.mFitness = mFitness;
    }
    
    public int getChromosomeElement(int index){
        return mChromosome[index];
    }
    
    public void setChromosomeElement(int index, int value){
        mChromosome[index] = value;
    }
    
    public int getChromosomeSize(){
        return mChromosome.length;
    }
    
    public String getPhenotype(){
        String res = "[";
        for(int i = 0; i < mChromosome.length; i++){
            res += "" + mChromosome[i] + ",";
        }
        res += "]";
        //Example: "[12,54,22]"
        return res;
    }
}
